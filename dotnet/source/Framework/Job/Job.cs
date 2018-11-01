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

    public class JobArgumentCollection : KeyedCollection<string, string>, ICloneable
    {
        public JobArgumentCollection()
        { 
        
        }
        
        protected override string GetKeyForItem(string item)
        {
            string[] arr = item.Split('=');

            if (arr.Length > 0)
                return arr[0];
            else
                return item;
        }

        protected override void InsertItem(int index, string item)
        {
            base.InsertItem(index, item);

            string[] arr = item.Split('=');

            if (arr.Length > 1)
            {
                base.Dictionary[arr[0]] = arr[1];
            }
        }

        public new bool Contains(string key)
        {
            return Dictionary.ContainsKey(key);
        }

        public new int IndexOf(string key)
        { 
            return Items.IndexOf(key + "=" + Dictionary[key]);
        }

        protected override void SetItem(int index, string item)
        {
            base.SetItem(index, item);
            
            string[] arr = item.Split('=');

            if (arr.Length > 1)
            {
                base.Dictionary[arr[0]] = arr[1];
            }
        }
        
        public object Clone()
        {
            JobArgumentCollection clonedCollection = new JobArgumentCollection();

            foreach (string arg in this)
            {
                clonedCollection.Add(arg);
            }

            return clonedCollection;
        }

        public string[] ToArray()
        {
            List<string> argList = new List<string>();

            foreach (string arg in this)
                argList.Add(arg);

            return argList.ToArray();
        }
    }

    public class JobShutdownException : Exception
    {
        public JobShutdownException(string message)
            : base(message)
        {
        }
    }
    
    /// <summary>
    /// Summary description for ManagedJob
    /// </summary>
    public abstract class Job
    {
        protected string name;
        protected JobArgumentCollection args;
        protected bool wait;
        protected LogType logType;
        private TraceSwitch traceSwitch;

        public SignalEventDelegate Signal;
        
        protected void OnSignal(string eventName)
        {
            if (Signal != null)
                Signal(eventName);
        }

        protected Job()
        {
            this.name = "NoName";
        }

        protected Job(string name, bool wait, LogType logType, JobArgumentCollection args)
        {
            this.name = name;
            this.wait = wait;
            this.args = args;
            this.logType = logType;
            this.traceSwitch = new TraceSwitch(name, name + " Trace Switch");
            this.traceSwitch.Level = InstanceConfig.ConvertLogLevel(logType.LogLevel);
            
        }

        internal void UpdateArgs(JobArgumentCollection args)
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
                            Trace.WriteLine("Failed to initialize file logging, directing to console.");
                            Trace.WriteLine(ex);
                        }
                    }
                }
            }

            Init();
        }

        public abstract void Init();
        
        internal void InternalExecute()
        {
            Execute();
        }

        public abstract void Execute();

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

        public virtual void SetTraceLevel(string level)
        {
            switch (level.ToLower())
            {
                case "off":
                    Tracing.Level = TraceLevel.Off;
                    break;
                case "error":
                    Tracing.Level = TraceLevel.Error;
                    break;
                case "warning":
                    Tracing.Level = TraceLevel.Warning;
                    break;
                case "info":
                    Tracing.Level = TraceLevel.Info;
                    break;
                case "verbose":
                    Tracing.Level = TraceLevel.Verbose;
                    break;
            }
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

        internal JobArgumentCollection GetArgs()
        {
            //Return a clone so that args cannot be manipulated directly
            return args;
        }
    }


}
