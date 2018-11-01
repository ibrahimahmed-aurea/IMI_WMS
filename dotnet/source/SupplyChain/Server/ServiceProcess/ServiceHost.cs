using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;

namespace Imi.SupplyChain.Server.Service
{
    static class ServiceHost
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            // Determine the systemId
            string systemId = null;

            int pos = Environment.CommandLine.LastIndexOf(" ");

            if ((pos > -1) && (pos < Environment.CommandLine.Length))
            {
                systemId = Environment.CommandLine.Substring(pos + 1);
            }

            System.ServiceProcess.ServiceBase[] ServicesToRun;

            // More than one user Service may run within the same process. To add
            // another service to this process, change the following line to
            // create a second service object. 
            ServicesToRun = new System.ServiceProcess.ServiceBase[] { new ServerServiceProcess(systemId) };
            System.ServiceProcess.ServiceBase.Run(ServicesToRun);
        }
    }
}
