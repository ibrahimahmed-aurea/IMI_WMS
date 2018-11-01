using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using Imi.Framework.Shared.Configuration;

namespace Imi.Framework.Messaging.Service
{
    public partial class MessagingServiceProcess : ServiceBase
    {
        private InstanceBase instance;
        private string assemblyName;
        private string instanceTypeName;
        private string instanceName;

        public MessagingServiceProcess(string assemblyName, string instanceTypeName, string instanceName)
        {
            InitializeComponent();

            this.assemblyName = assemblyName;
            this.instanceTypeName = instanceTypeName;
            this.instanceName = instanceName;

            instance = InstanceFactory.CreateInstance<InstanceBase>(assemblyName, instanceTypeName, instanceName);

            this.ServiceName = instance.applicationName + " (" + instanceName + ")";
        }
        
        protected override void OnStart(string[] args)
        {
            if (string.IsNullOrEmpty(assemblyName))
                throw new ArgumentException("Instance assembly not specified.");

            if (string.IsNullOrEmpty(instanceTypeName))
                throw new ArgumentException("Instance type name not specified.");

            if (string.IsNullOrEmpty(instanceName))
                throw new ArgumentException("Instance name not specified.");

            instance.Initialize();
            instance.Start();
                        
            base.OnStart(args);
                        
        }

        protected override void OnStop()
        {
            if (instance != null)
            {
                try
                {
                    instance.Stop();
                }
                finally
                {
                    try
                    {
                        AppDomain.Unload(instance.AppDomain);
                    }
                    catch
                    { 
                    }
                }
            }            
                        
            base.OnStop();
        }
    }
}
