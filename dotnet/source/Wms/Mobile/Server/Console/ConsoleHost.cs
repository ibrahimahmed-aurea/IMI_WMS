using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using Imi.Framework.Shared;
using Imi.Framework.Messaging;
using Imi.Framework.Messaging.Engine;
using Imi.Framework.Shared.Configuration;
using System.Configuration;

namespace Imi.Wms.Mobile.Server.Console
{
    class ConsoleHost
    {
        static InstanceBase instance;
        static ManualResetEvent stopWaitEvent;
                            
        static void Main(string[] args)
        {
            System.Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPressEventHandler);
            System.Console.BufferWidth = 1000;

            stopWaitEvent = new ManualResetEvent(false);

            CommandLineParser parser = new CommandLineParser(args);

            string instanceName = parser["instance"];

            if (String.IsNullOrEmpty(instanceName))
            {
                instanceName = ConfigurationManager.AppSettings["instance"];
            }
            
            instance = InstanceFactory.CreateInstance<InstanceBase>("Imi.Wms.Mobile.Server", "Imi.Wms.Mobile.Server.ServerInstance", instanceName);

            try
            {

                instance.Initialize();
                instance.Start();

                System.Console.WriteLine("Press Ctrl+C to terminate...");

                stopWaitEvent.WaitOne();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            finally
            {
                instance.Stop();
            }
            
            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey();

            Environment.Exit(0);
        }
                

        static void CancelKeyPressEventHandler(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            stopWaitEvent.Set();
        }
             
    }
}
