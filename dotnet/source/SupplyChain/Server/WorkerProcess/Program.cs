using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Security.Permissions;
using System.Diagnostics;
using System.Reflection;

namespace Imi.SupplyChain.Server.WorkerProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //AppDomainSetup setup = new AppDomainSetup();
                //setup.ConfigurationFile = args[0];
                //setup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

                //AppDomain domain = AppDomain.CreateDomain("WorkerProcess", null, setup, new PermissionSet(PermissionState.Unrestricted));

                

                WorkerProcessHost host = AppDomain.CurrentDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(WorkerProcessHost).FullName) as WorkerProcessHost;
                host.Run(args[0], Convert.ToInt32(args[1]));
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("IMI Supply Chain Server", string.Format("Failed to start worker process: {0}", ex), EventLogEntryType.Error);
                throw;
            }
        }
    }
}
