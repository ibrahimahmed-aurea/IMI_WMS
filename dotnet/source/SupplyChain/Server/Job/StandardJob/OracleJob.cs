#define ODP_NET
using System;
using System.Data;

#if ODP_NET
using Oracle.DataAccess.Client;
#else
using System.Data.OracleClient;
#endif

using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using Imi.Framework.Job;
using Imi.Framework.Job.Data;
using Imi.Framework.Job.Configuration;
using Imi.Framework.Shared.Diagnostics;
using Imi.Framework.Job.Engine;

namespace Imi.SupplyChain.Server.Job.StandardJob
{
    public abstract class OracleJob : ManagedJob
    {
        protected Database db = null;
        private object _syncLock;

        public OracleJob(string name, bool wait, JobArgumentCollection args)
            : base(name, wait, args)
        {
            _syncLock = new object();
        }

        protected abstract void CreateProcedure(IDbConnectionProvider connection);
        protected abstract void ExecuteProcedure(JobArgumentCollection args);
        protected abstract void CancelProcedure();

        protected void StartTransaction()
        {
            lock (_syncLock)
            {
                db.StartTransaction();
            }
        }

        protected void Commit()
        {
            lock (_syncLock)
            {
                db.Commit();
            }
        }

        protected void Rollback()
        {
            lock (_syncLock)
            {
                try
                {
                    if (db != null)
                        db.Rollback();
                }
                catch (Exception)
                {
                }
            }
        }

        protected bool IsConnected
        {
            get
            {
                lock (_syncLock)
                {
                    if (db != null)
                    {
                        return db.IsConnected;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        protected virtual void InternalExecuteProcedure(JobArgumentCollection args)
        {
            lock (_syncLock)
            {
                if (db == null)
                {
                    db = new Database(InstanceConfig.GetConnectionString());
                    CreateProcedure(db);
                }
            }

            ExecuteProcedure(args);
        }

        /// <summary>
        /// Execute is the main activity method, this is the code that is run
        /// when the Job is activated/signalled.
        /// </summary>
        public override void Execute()
        {
            bool retry = true;
            int retryTime = 1;

            while (retry && (JobState != JobState.Stopping))
            {
                try
                {
                    retry = false;

                    long start = DateTime.Now.Ticks;

                    Tracing.TraceEvent(TraceEventType.Verbose, 0, "Start ExecuteProcedure.");

                    InternalExecuteProcedure(args);

                    long stop = DateTime.Now.Ticks;
                    TimeSpan t = new TimeSpan(stop - start);

                    Tracing.TraceEvent(TraceEventType.Verbose, 0, "Stop ExecuteProcedure (used {0}s).", t.TotalSeconds.ToString("0.00"));
                }
                catch (OracleException ex)  
                {
                    if (JobState != JobState.Stopping)
                    {
                        Rollback();

                        if (!IsConnected)
                        {
                            retryTime = Math.Min(30, retryTime * 2);
                                                        
                            TraceOracleError(String.Format("Lost connection to database due to exception. Will attempt to logon in {0} seconds...", retryTime), ex);

                            //Force recreation of procedure during next retry
                            Disconnect();

                            Thread.Sleep(retryTime * 1000);
                            
                            retry = true;
                        }
                        else
                        {
                            TraceOracleError("Execute Oracle error.", ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (JobState != JobState.Stopping)
                    {
                        Rollback();
                        Tracing.TraceEvent(TraceEventType.Error, 0, "Execute caught exception.\n{0}", ex);
                    }
                    else
                    {
                        throw new JobShutdownException("Job was shutdown.", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Fixes newlines in oracle error message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        private void TraceOracleError(string message, OracleException ex)
        {
            String error = ex.Message;
            error = error.Replace(Environment.NewLine, "\n");
            error = error.Replace("\n", Environment.NewLine + "     ");
            Tracing.TraceEvent(TraceEventType.Error, 0, "{0}\r\n   {1}{2}", message, error, ex.StackTrace);
        }

        protected virtual void SignalEvents(Array eventArr) 
        {
            //
            // Handle Eventlist
            //
            if (eventArr != null)
            {
                for (int i = 0; i < eventArr.Length; i++)
                {
                    OnSignal((eventArr.GetValue(i)).ToString());
                    Tracing.TraceEvent(TraceEventType.Verbose, 0, "Signals {0}.", eventArr.GetValue(i).ToString());
                }
            }
        }

        private void Disconnect()
        {
            lock (_syncLock)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }

        public override void Init()
        {

        }

        /// <summary>
        /// Stop is called when the job is about to stop. This code
        /// is called in another thread and should just be used to signal
        /// a wish to stop.
        /// </summary>
        public override void Stop()
        {
            lock (_syncLock)
            {
                try
                {
                    CancelProcedure();
                }
                catch
                {
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disconnect();
            }

            base.Dispose(disposing);
        }


    }
}
