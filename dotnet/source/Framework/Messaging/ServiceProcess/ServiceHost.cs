using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System;

namespace Imi.Framework.Messaging.Service
{
    static class ServiceHost
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            string[] args = Environment.CommandLine.Split(new string[] { "\" \"", "\"" }, StringSplitOptions.RemoveEmptyEntries);
            string assemblyName = args[1];
            string instanceTypeName = args[2];
            string instanceName = args[3];
            
            ServiceBase[] ServicesToRun;

            ServicesToRun = new ServiceBase[] { new MessagingServiceProcess(assemblyName, instanceTypeName, instanceName) };

            ServiceBase.Run(ServicesToRun);
        }
    }
}