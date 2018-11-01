using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Imi.Framework.Job.Configuration;
using Imi.Framework.Shared.Diagnostics;

namespace Imi.Framework.Job
{
    public delegate void SignalEventDelegate(string eventName);
    public delegate void JobExecuteDelegate(string[] args);

    public class JobShutdownException : Exception
    {
        public JobShutdownException(string message)
            : base(message)
        {
        }
    }
    /// <summary>
    /// Summary description for JobThread
    /// </summary>
    public abstract class JobThread
    {
        protected string name;
        protected string[] args;
        protected bool wait;
        protected LogType logType;
        private TraceSwitch traceSwitch;

        public SignalEventDelegate Signal;
        
        protected void OnSignal(string eventName)
        {
            if (Signal != null)
                Signal(eventName);
        }

        protected JobThread()
        {
            this.name = "NoName";
        }

        protected JobThread(string name, bool wait, LogType logType, string[] args)
        {
            this.name = name;
            this.wait = wait;
            this.args = args;
            this.logType = logType;
            this.traceSwitch = new TraceSwitch(name, name + " Trace Switch");
            this.traceSwitch.Level = InstanceConfig.ConvertLogLevel(logType.LogLevel);
        }

        public bool Wait
        {
            get
            {
                return (wait);
            }
        }

        internal void InternalInit()
        {
            ContextTraceListener contextListener = ((ContextTraceListener)Trace.Listeners["ContextTraceListener"]);

            if (contextListener != null)
            {
                if (!contextListener.IsContextInitialized(this.name))
                {
                    try
                    {
                        contextListener.InitializeContext(this.name,
                            new RollingFileTraceListener(logType.FileName, logType.MaxFileSize));
                    }
                    catch (IOException ex)
                    {
                        contextListener.InitializeContext(this.name,
                            new TextWriterTraceListener(Console.Out));

                        if (Tracing.TraceError)
                        {
                            Trace.WriteLine("Failed to initialize file logging, directing to console instead.");
                            Trace.WriteLine(ex);
                        }
                    }
                }
            }

            Init();
        }

        public abstract void Init();

        public abstract void Execute(string[] newArgs);

        internal void InternalStop()
        {
            try
            {
                Stop();
            }
            finally
            {
                ContextTraceListener.Reset();
            }
        }
        
        public abstract void Stop();
        
        public TraceSwitch Tracing
        {
            get
            {
                return traceSwitch;
            }
        }

        public virtual string Name
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
    }


}
