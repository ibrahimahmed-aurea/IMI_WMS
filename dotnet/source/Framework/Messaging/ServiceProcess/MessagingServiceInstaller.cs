using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Reflection;
using System.IO;
using Imi.Framework.Messaging;


namespace Imi.Framework.Messaging.Service
{
    [RunInstaller(true)]
    public partial class MessagingServiceInstaller : Installer
    {

        public MessagingServiceInstaller()
        {
            InitializeComponent();
        }
                
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            if (Context == null)
                throw new ArgumentNullException("InstallContext");

            if (Context.Parameters["AssemblyName"] == null)
                throw new ArgumentNullException("assemblyName");

            if (Context.Parameters["InstanceTypeName"] == null)
                throw new ArgumentNullException("instanceTypeName");

            if (Context.Parameters["InstanceName"] == null)
                throw new ArgumentNullException("instanceName");

            Assembly asm = Assembly.LoadFrom(Path.Combine(Environment.CurrentDirectory, Context.Parameters["AssemblyName"]) + ".dll");

            InstanceBase instance = (InstanceBase)asm.CreateInstance(Context.Parameters["InstanceTypeName"], true, BindingFlags.CreateInstance, null, new object[] { Context.Parameters["InstanceName"] }, null, null);

            string name = instance.applicationName + " (" + Context.Parameters["InstanceName"] + ")";
            string description = instance.applicationName;

            serviceInstaller.ServiceName = name;
            serviceInstaller.DisplayName = name;

            base.Uninstall(savedState);

        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            if (Context == null)
                throw new ArgumentNullException("InstallContext");

            if (Context.Parameters["AssemblyName"] == null)
                throw new ArgumentNullException("assemblyName");

            if (Context.Parameters["InstanceTypeName"] == null)
                throw new ArgumentNullException("instanceTypeName");

            if (Context.Parameters["InstanceName"] == null)
                throw new ArgumentNullException("instanceName");

                        
            Assembly asm = Assembly.LoadFrom(Path.Combine(Environment.CurrentDirectory, Context.Parameters["AssemblyName"]) + ".dll");

            InstanceBase instance = (InstanceBase)asm.CreateInstance(Context.Parameters["InstanceTypeName"], true, BindingFlags.CreateInstance, null, new object[] { Context.Parameters["InstanceName"] }, null, null);

            string name = instance.applicationName + " (" + Context.Parameters["InstanceName"] + ")";
            string description = instance.applicationName;
            
            serviceInstaller.ServiceName = name;
            serviceInstaller.DisplayName = name;
        
            base.Install(stateSaver);

            Microsoft.Win32.RegistryKey serviceDescriptionKey = null;
            
            try
            {
                serviceDescriptionKey =
                  Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Services\" + serviceInstaller.ServiceName, true);
                
                serviceDescriptionKey.SetValue("Description", description);

                string imagePath = serviceDescriptionKey.GetValue("ImagePath") as string;
                serviceDescriptionKey.SetValue("ImagePath", string.Format("{0} \"{1}\" \"{2}\" \"{3}\"", imagePath, Context.Parameters["assemblyName"], Context.Parameters["instanceTypeName"], Context.Parameters["instanceName"]));
                 
            }
            finally
            {
                if (serviceDescriptionKey != null)
                    serviceDescriptionKey.Close();
            }

        }
    }
}