using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using Imi.Framework.Job.RemoteInterface;

namespace Imi.Framework.Job.Engine
{
    public class RemoteInterface : IRemoteInterface, IDisposable
    {
        private JobManager jobManager;
        private DateTime psTimeStamp = DateTime.MinValue;
        private string psList;
        private IChannel channel;
        private ObjRef remoteRef;
        private RemoteInterfaceProxy proxy;
        private bool disposed;
        
        public RemoteInterface(JobManager jobManager)
        {
            this.jobManager = jobManager;
            psList = "";
        }

        ~RemoteInterface()
        {
            Dispose(false);
        }

        public void Initialize(int port, string uri)
        {
            channel = new HttpChannel(port);
            ChannelServices.RegisterChannel(channel, false);

            // Display full stack trace to simplify debugging
            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;

            jobManager.Tracing.TraceEvent(TraceEventType.Verbose, 0, "CustomErrorsModes is {0}", RemotingConfiguration.CustomErrorsMode);

            proxy = new RemoteInterfaceProxy(this);

            // Creates the single instance of RemoteInstanceCtrl. 
            remoteRef = RemotingServices.Marshal(proxy, uri);

            jobManager.Tracing.TraceEvent(TraceEventType.Verbose, 0, "ObjRef.URI: {0}", remoteRef.URI);
        }

        public string StopAll()
        {
            jobManager.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"StopAll\"");
            jobManager.Tracing.TraceEvent(TraceEventType.Information, 0, "Stopping all jobs.");
            jobManager.ThreadManager.StopAll(false);

            return ("ok");
        }

        public string StopJob(string name)
        {
            jobManager.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"StopJob {0}\"", name);
            jobManager.ThreadManager.StopThread(name);
            return ("ok");
        }

        public string StartAll()
        {
            jobManager.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"StartAll\"");
            jobManager.Tracing.TraceEvent(TraceEventType.Information, 0, "Starting all jobs.");

            jobManager.ThreadManager.StartAll(false);

            foreach (ManagedThread managedThread in jobManager.ThreadManager.GetThreads())
            {
                if (managedThread.CurrentState == JobState.Stopping)
                {
                    return "One ore more jobs failed to start since they were still in status \"Stopping\".";
                }
            }

            return "ok";
        }

        public string StartJob(string name)
        {
            if (jobManager.ThreadManager.StartThread(name))
                return "ok";
            else
                return "Job is not in status \"Wait\" or \"Stopped\".";
        }

        public string Ps()
        {
            jobManager.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"Ps\"");

            lock (psList)
            {
                TimeSpan ts = DateTime.Now.Subtract(psTimeStamp);

                if (ts.TotalSeconds > 1.0)
                {
                    psList = jobManager.Ps();
                    psTimeStamp = DateTime.Now;
                }
            }

            return psList;
        }

        public DateTime Time()
        {
            jobManager.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"Time\"");
            return (DateTime.Now);
        }

        public string SetParameter(string jobName, string parameterName, string parameterValue)
        {
            jobManager.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"SetParameter\" for \"{0} {1}={2}\"", jobManager, parameterName, parameterValue);

            JobArgumentCollection args = jobManager.ThreadManager.GetArguments(jobName);

            // will overwrite old values if they exists
            args[parameterName] = parameterValue;

            jobManager.ThreadManager.SetArguments(jobName, args);

            return ("ok");
        }

        public string GetParameter(string jobName, string parameterName)
        {
            jobManager.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"GetParameter\" for \"{0} {1}\"", jobManager, parameterName);
            JobArgumentCollection args = jobManager.ThreadManager.GetArguments(jobName);

            if (args.ContainsKey(parameterName))
                return args[parameterName];
            else
                return "";

        }

        public string SetTraceLevel(string name, string level)
        {
            jobManager.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"SetTraceLevel\" for \"{0} level={1}\"", name, level);
            jobManager.ThreadManager.SetTraceLevel(name, level);
            return ("ok");
        }


        public string GetTraceLevel(string name)
        {
            jobManager.Tracing.TraceEvent(TraceEventType.Verbose, 0, "Got command \"GetTraceLevel\" for \"{0}\"", name);
            return jobManager.ThreadManager.GetTraceLevel(name);
        }
        
        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (proxy != null)
                    {
                        RemotingServices.Disconnect(proxy);
                    }

                    if (channel != null)
                    {
                        ChannelServices.UnregisterChannel(channel);
                    }
                }
            }

            disposed = true;
        }

        #endregion
    }
}
