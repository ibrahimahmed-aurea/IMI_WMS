using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Imi.Framework.Job.Configuration;
using Imi.Framework.Job.Interfaces;
using Imi.Framework.Shared.Diagnostics;

namespace Imi.Framework.Job.Engine
{
    
    /// <summary>
    /// The ManagedThread class is basically a wrapper class, that adds some common
    /// functionality to the specialized Job classes such as OracleJob.
    /// This way each class does not have to implement waiting for a signal for example.
    /// It also doubles as a datacarrier for the ThreadManager class.
    /// </summary>
    public class ManagedThread : ISubscriber
    {
        private Thread _thread;
        private ManagedJob _job;
        private string _name;
        private bool _wait;
        private bool _updateArgs;
        private AutoResetEvent _waitFlag;
        private JobArgumentCollection _args;
        private ThreadType _type = ThreadType.Unknown;
        private IJournalManager _journalManager;

        internal ManagedThread(ManagedJob job, ThreadType type, bool signal, bool wait)
        {
            _job = job;
            _name = job.Name;
            _args = job.GetArguments();

            if (wait)
            {
                _waitFlag = new AutoResetEvent(signal);
                _wait = true;
            }

            CurrentState = JobState.Stopped;
            _thread = new Thread(new ThreadStart(Run));
            _type = type;
        }
        
        public IJournalManager JournalManager
        {
            get
            {
                return (_journalManager);
            }
            set
            {
                _journalManager = value;
            }
        }

        public JobState CurrentState
        {
            get
            {
                lock (_job)
                {
                    return _job.JobState;
                }
            }
            set
            {
                lock (_job)
                {
                    //Do not allow change of status while stopping
                    if ((_job.JobState == JobState.Stopping) && (value != JobState.Stopped))
                        return;

                    _job.JobState = value;

                    if (_journalManager != null)
                    {
                        switch (value)
                        {
                            case JobState.Init:
                                _journalManager.JobStart(_name, _thread.ManagedThreadId);
                                break;
                            case JobState.Run:
                                _journalManager.JobRun(_name);
                                break;
                            case JobState.Wait:
                                _journalManager.JobWait(_name);
                                break;
                            case JobState.Stopping:
                                _journalManager.JobStopping(_name);
                                break;
                            case JobState.Stopped:
                                _journalManager.JobStop(_name);
                                break;
                        }
                    }
                }
        
            }
        }

        internal void Join()
        {
            Thread temp = _thread;

            if (temp != null && temp.IsAlive)
            {
                try
                {
                    temp.Join();
                }
                catch (ThreadStateException)
                { 
                }
            }
        }
        
        public bool IsSystemThread
        {
            get
            {
                return (_type == ThreadType.System);
            }
        }
        
        public void Run()
        {
            DateTime startTime;

            try
            {
                if (_updateArgs)
                {
                    lock (_args)
                    {
                        _job.SetArguments(_args);
                    }
                }

                CurrentState = JobState.Init;
                _job.InternalInit();

                while (CurrentState != JobState.Stopping)
                {
                    try
                    {
                        if (_wait)
                        {
                            CurrentState = JobState.Wait;
                            _waitFlag.WaitOne();

                            if (CurrentState == JobState.Stopping)
                                break;
                        }
                        else
                        {
                            //Wait atleast one second to avoid spin
                            Thread.Sleep(1000);
                        }

                        if (_updateArgs)
                        {
                            lock (_args)
                            {
                                _updateArgs = false;
                                _job.SetArguments(_args);
                            }
                        }

                        startTime = DateTime.Now;

                        CurrentState = JobState.Run;
                        _job.InternalExecute();

                        if (_journalManager != null)
                            _journalManager.JobEndRun(_job.Name, DateTime.Now.Subtract(startTime));
                    }
                    catch (JobShutdownException)
                    {
                        _job.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Thread stopped.");
                        break;
                    }
                    catch (ThreadAbortException)
                    {
                        _job.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Thread aborted.");
                        break;
                    }
                    catch (ThreadInterruptedException)
                    {
                        _job.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Thread interrupted.");
                        continue;
                    }
                    catch (Exception ex)
                    {
                        _job.Tracing.TraceData(TraceEventType.Error, 0, ex);
                        break;
                    }
                }
            }
            finally
            {
                try
                {
                    _job.Dispose();
                }
                catch (Exception ex)
                {
                    _job.Tracing.TraceData(TraceEventType.Error, 0, ex);
                }
                finally
                {
                    CurrentState = JobState.Stopped;
                    _thread = null;
                }

            }
        }

        public bool Stop()
        {
            //Do not allow stop while job is in state Init, Stopping or Stopped.
            if ((CurrentState == JobState.Stopping) 
                || (CurrentState == JobState.Stopped) 
                || (CurrentState == JobState.Init))
                    return false;

            if (_thread == null)
                return false;

            try
            {
                Thread temp = _thread;

                try
                {
                    CurrentState = JobState.Stopping;

                    try
                    {
                        //Signal if job is waiting
                        if (_waitFlag != null)
                            _waitFlag.Set();
                    }
                    finally
                    {
                        _job.InternalStop();
                    }
                }
                finally
                {
                    bool joined = false;

                    //Wait 20 sec for thread to terminate
                    if (temp.IsAlive)
                    {
                        try
                        {
                            joined = temp.Join(20000);
                        }
                        finally
                        {
                            if ((temp.IsAlive) && (!joined))
                                temp.Abort();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _job.Tracing.TraceData(TraceEventType.Error, 0, ex);
            }

            return true;

        }

        public bool Start()
        {
            if (CurrentState == JobState.Stopped)
            {
                try
                {
                    if (_thread == null)
                    {
                        _thread = new Thread(new ThreadStart(Run));
                    }

                    _thread.Start();
                    
                    return true;
                }
                catch (Exception ex)
                {
                    _job.Tracing.TraceData(TraceEventType.Error, 0, ex);
                }
                
                return false;
            }
            else if (CurrentState == JobState.Wait)
            {
                // Trigger job already running
                if (_waitFlag != null)
                {
                    _waitFlag.Set();
                }

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Expose AutoResetEvent for signalling logic.
        /// </summary>
        /// <returns></returns>
        public AutoResetEvent GetWaitObject()
        {
            return (_waitFlag);
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Used to set args for a running thread.
        /// args is not complete i.e. it could be empty.
        /// </summary>
        /// <param name="args"></param>
        internal void SetArguments(JobArgumentCollection args)
        {
            lock (args)
            {
                _args = args;
                //Force update in Run() method
                _updateArgs = true;
            }
        }
                
        internal JobArgumentCollection GetArguments()
        {
            lock (_args)
            {
                if (_args == null)
                    return null;
                else
                    return (JobArgumentCollection)_args.Clone();
            }
        }

        internal SourceLevels TraceLevel
        {
            get
            {
                return _job.Tracing.Switch.Level;
            }
            set
            {
                _job.Tracing.Switch.Level = value;
                _job.UpdateTraceLevel();
            }
        }
    }
}
