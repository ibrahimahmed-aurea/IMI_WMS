using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Imi.Framework.Job.Configuration;
using Imi.Framework.Shared.Diagnostics;

namespace Imi.Framework.Job
{
    public delegate void SignalEventDelegate(string eventName);

    public class JobArgumentCollection : Dictionary<string, string>, ICloneable
    {
        public JobArgumentCollection()
        { 
            
        }
        
        public object Clone()
        {
            JobArgumentCollection clonedCollection = new JobArgumentCollection();

            foreach (string key in Keys)
            {
                clonedCollection.Add(key,this[key]);
            }

            return clonedCollection;
        }
    }

    public class JobShutdownException : Exception
    {
        public JobShutdownException(string message)
            : base(message)
        {
        }

        public JobShutdownException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public enum JobState
    {
        Init,
        Wait,
        Run,
        Stopping,
        Stopped
    }

    public abstract class ManagedJob : IDisposable
    {
        private string name;
        protected JobArgumentCollection args;
        private bool wait;
        private TraceSource traceSource;
        private JobState state;
        public SignalEventDelegate Signal;
        private bool disposed;
        
        protected void OnSignal(string eventName)
        {
            if (Signal != null)
                Signal(eventName);
        }

        protected ManagedJob()
        {
            this.name = "Unnamed";
        }

        protected ManagedJob(string name, bool wait, JobArgumentCollection args)
        {
            this.name = name;
            this.wait = wait;
            this.args = args;
            this.traceSource = new TraceSource(name);
        }

        ~ManagedJob()
        {
            Dispose(false);
        }

        public JobState JobState
        {
            get
            {
                return state;
            }
            internal set
            {
                state = value;
            }
        }

        internal void SetArguments(JobArgumentCollection args)
        {
            this.args = args;
        }

        internal bool Wait
        {
            get
            {
                return (wait);
            }
        }
        
        internal void InternalInit()
        {
            disposed = false;
            Init();
            Tracing.TraceEvent(TraceEventType.Information, 0, "Started.");
        }

        public abstract void Init();

        internal void InternalExecute()
        {
            Tracing.TraceEvent(TraceEventType.Information, 0, "Executing.");
            Execute();
        }

        public abstract void Execute();

        internal void InternalStop()
        {
            Stop();
            Tracing.TraceEvent(TraceEventType.Information, 0, "Stopped.");
            Thread.Sleep(0);
        }

        public abstract void Stop();
                
        public virtual TraceSource Tracing
        {
            get
            {
                return traceSource;
            }
        }

        public virtual void UpdateTraceLevel()
        {
        
        }
                
        public string Name
        {
            get
            {
                return (name);
            }
            set
            {
                name = value;
            }
        }

        internal JobArgumentCollection GetArguments()
        {
            //Return a clone so that args cannot be manipulated directly
            if (args == null)
                return null;
            else
                return (JobArgumentCollection)args.Clone();
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Tracing.TraceEvent(TraceEventType.Verbose, 0, "Disposed.");
                }
            }

            disposed = true;
        }

        #endregion
    }


}
