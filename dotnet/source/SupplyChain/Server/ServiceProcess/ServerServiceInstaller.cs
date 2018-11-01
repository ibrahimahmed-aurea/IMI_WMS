using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;

namespace Imi.SupplyChain.Server.Service
{
    /// <summary>
    /// Summary description for ProjectInstaller.
    /// </summary>
    [RunInstaller(true)]
    public class WmsServerServiceInstaller : System.Configuration.Install.Installer
    {

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller serviceInstaller;

        public override void Uninstall(IDictionary savedState)
        {
            if (Context != null)
            {
                if (Context.Parameters.Count > 0)
                {
                    String systemId = Context.Parameters["systemid"];
                    if ((systemId != null) && (systemId != ""))
                    {
                        this.serviceInstaller.ServiceName = ServerServiceProcess.GetServiceName(systemId);
                        this.serviceInstaller.DisplayName = this.serviceInstaller.ServiceName;
                    }
                }
            }

            // call the original Install() method to create root key
            base.Uninstall(savedState);
        }

        public override void Install(IDictionary savedState)
        {
            String systemId = "";

            if (Context != null)
            {
                if (Context.Parameters.Count > 0)
                {
                    systemId = Context.Parameters["systemid"];

                    if ((systemId != null) && (systemId != ""))
                    {
                        this.serviceInstaller.ServiceName = ServerServiceProcess.GetServiceName(systemId);
                        this.serviceInstaller.DisplayName = this.serviceInstaller.ServiceName;
                    }
                }
            }

            // call the original Install() method to create root key
            base.Install(savedState);

            Microsoft.Win32.RegistryKey ServiceDescription = null;
            try
            {
                ServiceDescription =
                  Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Services\" + serviceInstaller.ServiceName, true);

                ServiceDescription.SetValue("Description", ServerServiceProcess.GetServiceDescription(systemId));
                if ((systemId != null) && (systemId != ""))
                {
                    string imagePath = ServiceDescription.GetValue("ImagePath") as string;
                    ServiceDescription.SetValue("ImagePath", string.Format("{0} {1}", imagePath, systemId));
                }

                ServiceDescription.Close();
            }
            catch (Exception)
            {
            }

        }


        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public WmsServerServiceInstaller()
        {
            // This call is required by the Designer.
            InitializeComponent();
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


        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller
            // 
            this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller.Password = null;
            this.serviceProcessInstaller.Username = null;
            // 
            // serviceInstaller
            // 
            this.serviceInstaller.ServiceName = "IMI Supply Chain Application Server (Unknown)";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller,
            this.serviceInstaller});

        }
        #endregion
    }
}
