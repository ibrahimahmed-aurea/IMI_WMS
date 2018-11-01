using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.IO;
using System.Reflection;
using Imi.Framework.Job.Engine;

namespace Imi.Wms.Server.ServiceProcess
{
    public class ServiceProcess : System.ServiceProcess.ServiceBase
    {
        private static String ServiceNamePrefix = "IMIServer_";
        private String systemId = "";
        private JobManager jm;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public static String GetServiceName(String systemId)
        {
            return (String.Format("{0}{1}", ServiceNamePrefix, systemId));
        }

        public static String GetServiceDescription(String systemId)
        {
            return (String.Format("IMI Server (version {0})", Assembly.GetExecutingAssembly().GetName().Version));
        }

        public ServiceProcess(String systemId)
        {
            // This call is required by the Windows.Forms Component Designer.
            InitializeComponent();
            this.systemId = systemId;
            this.ServiceName = ServiceProcess.GetServiceName(systemId);
        }

        // The main entry point for the process
        static void Main()
        {
            // Determine the systemId
            String sId = null;

            int pos = Environment.CommandLine.LastIndexOf(" ");

            if ((pos > -1) && (pos < Environment.CommandLine.Length))
            {
                sId = Environment.CommandLine.Substring(pos + 1);
            }

            System.ServiceProcess.ServiceBase[] ServicesToRun;

            // More than one user Service may run within the same process. To add
            // another service to this process, change the following line to
            // create a second service object. 
            ServicesToRun = new System.ServiceProcess.ServiceBase[] { new ServiceProcess(sId) };
            System.ServiceProcess.ServiceBase.Run(ServicesToRun);
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
            if ((systemId == null) || (systemId == ""))
            {
                throw new ArgumentNullException("systemId", "Could not determine SystemId, start of instance fails.");
            }

            try
            {
                jm = new JobManager(systemId, true);
                EventLog.WriteEntry(String.Format("Job Manager for SystemId = {0} has started.", systemId), EventLogEntryType.Information);
            }
            catch (Exception e)
            {
                throw (new Exception("Failed to start JobManager due to exception.", e));
            }
        }

        /// <summary>
        /// Stop this service.
        /// </summary>
        protected override void OnStop()
        {
            EventLog.WriteEntry("Shutdown started", EventLogEntryType.Information);
            jm.Shutdown();
            EventLog.WriteEntry("Shutdown finished", EventLogEntryType.Information);

            // Ensure that all objects finalize before the application ends 
            // by calling the garbage collector and waiting.
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
