using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Reflection;
using Imi.Framework.Job;
using Imi.Framework.Job.Engine;
using Imi.Framework.Job.Interfaces;
using Imi.Framework.Shared.Diagnostics;
using Imi.Framework.Shared.Configuration;
using Imi.Framework.Shared.IO;
using Imi.Framework.Job.Configuration;

namespace Imi.Framework.Job.Engine
{
    
    /// <summary>
    /// Summary description for JobManager.
    /// </summary>
    public sealed class JobManager : ApplicationInstance
    {
        private ThreadManager threadManager;
        private SubscriptionManager subscriptionManager;
        private JobFactory jobFactory;
        private Scheduler scheduler;
        private JournalManager journalManager;
        private RemoteInterface remoteInterface;
        private TraceSource traceSource;
        private string name = "JobManager";

        public event EventHandler Starting;

        public JobManager(string instanceName) : base(instanceName)
        {
        }

        public void StartServer()
        {
            this.name = this.GetType().Name;

            Trace.AutoFlush = true;

            traceSource = new TraceSource(name);

            traceSource.Switch.Level = SourceLevels.Information;

            try
            {
                Tracing.TraceEvent(TraceEventType.Information, 0, "Starting job manager.");
                Tracing.TraceEvent(TraceEventType.Information, 0, "Reading configuration for SystemId: \"{0}\".", InstanceName);
                InstanceConfig.LoadInstance(InstanceName);

                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionEventHandler);


                Tracing.TraceEvent(TraceEventType.Information, 0, "Setting log level to \"{0}\".",
                        InstanceConfig.CurrentInstance.Log.LogLevel.ToString());
                
                //Init tracing
                Tracing.Switch.Level = ConfigurationHelper.ConvertLogLevel(InstanceConfig.CurrentInstance.Log.LogLevel);
                Tracing.Listeners.Add(new RollingFileTraceListener(InstanceConfig.CurrentInstance.Log.FileName, InstanceConfig.CurrentInstance.Log.MaxLogSize));

                Tracing.TraceEvent(TraceEventType.Information, 0, "Tracing enabled.");

                if (Starting != null)
                {
                    Starting(this, null);
                }

                Tracing.TraceEvent(TraceEventType.Information, 0, "Creating thread manager.");
                threadManager = new ThreadManager();

                Tracing.TraceEvent(TraceEventType.Information, 0, "Creating subscription manager.");
                subscriptionManager = new SubscriptionManager();

                Tracing.TraceEvent(TraceEventType.Information, 0, "Creating job factory.");
                jobFactory = new JobFactory();

                Tracing.TraceEvent(TraceEventType.Information, 0, "Creating scheduler.");
                
                // Create scheduler
                scheduler = new Scheduler("Scheduler", false, null);
                scheduler.Signal += new SignalEventDelegate(this.JobSignal);

                //Inherit trace settings from job manager
                string schedulerFileName = Path.Combine(Path.GetDirectoryName(InstanceConfig.CurrentInstance.Log.FileName), "Scheduler.log");
                scheduler.Tracing.Switch.Level = Tracing.Switch.Level;
                scheduler.Tracing.Listeners.Add(new RollingFileTraceListener(schedulerFileName, InstanceConfig.CurrentInstance.Log.MaxLogSize));

                ManagedThread managedThread = threadManager.CreateSystemThread(scheduler, true, scheduler.Wait);

                Tracing.TraceEvent(TraceEventType.Information, 0, "Creating schedules.");
                                
                ManagedSchedule[] schedules = jobFactory.CreateSchedules();

                foreach (ManagedSchedule schedule in schedules)
                {
                    scheduler.AddSchedule(schedule);
                }

                Tracing.TraceEvent(TraceEventType.Information, 0, "Creating jobs.");
               
                journalManager = new JournalManager();

                List<ManagedJob> jobs = JobFactory.CreateJobs();

                foreach (ManagedJob job in jobs)
                {
                    // Setup signalling
                    job.Signal += new SignalEventDelegate(this.JobSignal);
                                        
                    managedThread = threadManager.CreateThread(job, false, job.Wait);

                    subscriptionManager.AddSubscriber(managedThread as ISubscriber, new string[] { job.Name });
                    // Setup journalling
                    managedThread.JournalManager = journalManager;
                    journalManager.AddJob(job.Name);
                }
                                
                // Start scheduler
                Tracing.TraceEvent(TraceEventType.Information, 0, "Starting scheduler.");
                threadManager.StartThread("Scheduler");

                // Enable remote interface

                Tracing.TraceEvent(TraceEventType.Information, 0, "Starting remote interface.");

                remoteInterface = new RemoteInterface(this);
                remoteInterface.Initialize(InstanceConfig.CurrentInstance.Port, InstanceConfig.CurrentInstance.URI);

                // Autostart
                if (InstanceConfig.CurrentInstance.AutoStart)
                {
                    Tracing.TraceEvent(TraceEventType.Information, 0, "Autostart is enabled, starting all jobs.");
                    threadManager.StartAll(false);
                }

                InstanceConfig.ConfigChanged += new ConfigChangedEvent(this.ConfigFileChanged);
            }
            catch (Exception ex)
            {
                Tracing.TraceData(TraceEventType.Error, 0, ex);
                // Shutdown if we get exception since scheduler might be running
                Shutdown();
                throw;
            }
        }

        void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
        {
            if ((Tracing.Switch.Level & SourceLevels.Critical) == SourceLevels.Critical)
                Tracing.TraceData(TraceEventType.Critical, 0, e.ExceptionObject);
        }

        public ThreadManager ThreadManager
        {
            get
            {
                return threadManager;
            }
        }

        public TraceSource Tracing
        {
            get
            {
                return traceSource;
            }
        }

        private void ConfigFileChanged()
        {
            if (InstanceConfig.LoadErrorMessage != "")
            {
                Tracing.TraceEvent(TraceEventType.Error, 0, InstanceConfig.LoadErrorMessage);
                return;
            }
            
            threadManager.UpdateArgumentsForAllJobs(jobFactory.CreateArgsForAllJobs());
        }

        private void JobSignal(string eventName)
        {
            SignalEvent(eventName);
        }

        public void RemoveJob(string name)
        {
            try
            {
                subscriptionManager.RemoveSubscriber(name);
            }
            finally
            {
                threadManager.StopThread(name);
            }
        }
        
        public void SignalEvent(string eventName)
        {
            ICollection subscriberCollection = subscriptionManager.GetSubscriberList(eventName);

            if (subscriberCollection != null)
            {
                foreach (ISubscriber subscriber in subscriberCollection)
                {
                    ISubscriber sw = subscriber as ISubscriber;
                    AutoResetEvent waitEvent = sw.GetWaitObject();
                    if (waitEvent != null)
                        waitEvent.Set();
                }
                Thread.Sleep(0); // Yield
            }
        }
        
        public void StartUp()
        {
            threadManager.StartAll(true);
        }

        public void Shutdown()
        {
            Tracing.TraceEvent(TraceEventType.Information, 0, "Shutting down.");

            if (remoteInterface != null)
            {
                Tracing.TraceEvent(TraceEventType.Information, 0, "Stopping remote interface.");
                remoteInterface.Dispose();
            }

            Tracing.TraceEvent(TraceEventType.Information, 0, "Stopping all jobs.");

            if (threadManager != null)
            {
                threadManager.StopAll(true);

                foreach (ManagedThread thread in threadManager.GetThreads())
                {
                    //Wait for thread to terminate
                    thread.Join();
                }
            }
        }

        public string Ps()
        {
            JobRuntimeType[] jobs = journalManager.GetJobs();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(JobRuntimeType[]));
            StringBuilder sb = new StringBuilder();
            xmlSerializer.Serialize(new StringWriter(sb), jobs);
            return (sb.ToString().Replace("utf-16", "utf-8"));
        }
    }
}
