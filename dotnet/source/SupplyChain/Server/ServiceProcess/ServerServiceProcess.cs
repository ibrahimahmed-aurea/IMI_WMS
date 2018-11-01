using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.IO;
using System.Reflection;
using Imi.Framework.Job.Engine;
using Imi.Framework.Shared.Configuration;

namespace Imi.SupplyChain.Server.Service
{
    public class ServerServiceProcess : System.ServiceProcess.ServiceBase
    {
        private const string serviceName = "IMI Supply Chain Application Server ({0})";
        private string systemId = "";
        private JobManager jobManager;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public static string GetServiceName(string systemId)
        {
            return string.Format(serviceName, systemId);
        }

        public static string GetServiceDescription(string systemId)
        {
            return string.Format("IMI Supply Chain Application Server (v{0})", Assembly.GetExecutingAssembly().GetName().Version);
        }

        public ServerServiceProcess(string systemId)
        {
            // This call is required by the Windows.Forms Component Designer.
            InitializeComponent();
            this.systemId = systemId;
            this.ServiceName = ServerServiceProcess.GetServiceName(systemId);
        }
        
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Set things in motion so your service can do its work.
        /// </summary>
        protected override void OnStart(string[] args)
        {
            if (string.IsNullOrEmpty(systemId))
            {
                Exception ex = new ArgumentException("systemId", "Could not determine SystemId, start of instance failed.");
                EventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
                throw ex;
            }

            try
            {
                jobManager = InstanceFactory.CreateInstance<JobManager>("Imi.Framework.Job", "Imi.Framework.Job.Engine.JobManager", systemId);
                jobManager.StartServer();
                EventLog.WriteEntry(string.Format("Job Manager for SystemId = {0} has started.", systemId), EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                ex = new Exception("Failed to start Job Manager due to exception.", ex);
                EventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
                throw ex;
            }
        }

        /// <summary>
        /// Stop this service.
        /// </summary>
        protected override void OnStop()
        {
            EventLog.WriteEntry("Shutdown started.", EventLogEntryType.Information);

            try
            {
                jobManager.Shutdown();
                AppDomain.Unload(jobManager.AppDomain);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(string.Format("Exception during shutdown {0}.",ex.Message), EventLogEntryType.Warning);
            }
            finally
            {
                EventLog.WriteEntry("Shutdown finished.", EventLogEntryType.Information);

                // Ensure that all objects finalize before the application ends 
                // by calling the garbage collector and waiting.
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
