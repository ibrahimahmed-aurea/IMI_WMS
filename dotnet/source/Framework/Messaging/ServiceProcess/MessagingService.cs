using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using Imi.Framework.Messaging;

namespace Imi.Framework.Messaging.ServiceProcess
{
    public partial class MessagingService : ServiceBase
    {
        private InstanceBase instance;

        public MessagingService()
        {
            InitializeComponent();
        }

        

        protected override void OnStart(string[] args)
        {
            instance = InstanceFactory.CreateInstance("", "");
            instance.Initialize();
            instance.Start();
        }

        protected override void OnStop()
        {
            if (instance != null)
                InstanceFactory.UnloadInstance(instance);

            instance = null;
        }
    }
}
