using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Job;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace Imi.SupplyChain.Server.Job.DotNet
{
    public class Job : ManagedJob
    {
        private string typeName;
        private object jobInstance;
        private MethodInfo jobMethod;
        private object lockObject = new object();

        public Job(string name, bool wait, JobArgumentCollection args)
            : base(name, wait, args)
        {
            typeName = args["type"];
            
            if (string.IsNullOrEmpty(typeName))
            {
                throw new NullReferenceException(string.Format("Error in job {0}. The type parameter is null, must be defined.", name));
            }

            jobMethod = null;
            Type jobType = Type.GetType(typeName);

            if (jobType != null)
            {
                jobInstance = Activator.CreateInstance(jobType);

                if (jobInstance != null)
                {
                    jobMethod = jobType.GetMethod("Execute");
                }
            }

            if (jobMethod == null)
            {
                throw new ArgumentException(string.Format("Failed to initialize job {0}", name));
            }

        }

        public override void Init()
        {
            Tracing.TraceEvent(TraceEventType.Verbose, 0, "Enter Init");
        }

        public override void Execute()
        {
            lock (lockObject)
            {
                if (jobMethod != null)
                {
                    try
                    {
                        long start = DateTime.Now.Ticks;

                        Tracing.TraceEvent(TraceEventType.Verbose, 0, "Start Execute.");

                        jobMethod.Invoke(jobInstance, null);

                        long stop = DateTime.Now.Ticks;
                        TimeSpan t = new TimeSpan(stop - start);

                        Tracing.TraceEvent(TraceEventType.Verbose, 0, "Stop Execute (used {0}s).", t.TotalSeconds.ToString("0.00"));
                    }
                    catch (Exception ex)
                    {
                        if (JobState != JobState.Stopping)
                        {
                            Tracing.TraceEvent(TraceEventType.Error, 0, "Execute caught unexpected exception.\n{0}", ex);
                        }
                        else
                        {
                            throw new JobShutdownException("Job was shutdown.", ex);
                        }
                    }
                }
            }
        }

        public override void Stop()
        {
            Tracing.TraceEvent(TraceEventType.Verbose, 0, "Enter Stop");
            lock(lockObject)
            {
                jobMethod = null;
            }
        }
    }
}
