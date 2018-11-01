using System;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Threading;

using Imi.Framework.Job.Engine;
using Imi.Framework.Shared;
using Imi.Framework.Shared.Configuration;
using Imi.Framework.Shared.Diagnostics;

namespace Imi.Wms.Server.Console
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class ConsoleHost
    {
        private JobManager jobManager;
        private ManualResetEvent stopWaitEvent;

        public void StartServer(string systemId)
        {
            System.Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
            stopWaitEvent = new ManualResetEvent(false);
        
            try
            {
                jobManager = InstanceFactory.CreateInstance<JobManager>("Imi.Framework.Job", "Imi.Framework.Job.Engine.JobManager", systemId);
                
                jobManager.StartServer();

                System.Console.WriteLine("Press Ctrl+C to terminate...");
                
                stopWaitEvent.WaitOne();

                System.Console.WriteLine("Shutdown in progress, please wait...");
                jobManager.Shutdown();

                // Unload the AppDomain
                AppDomain.Unload(jobManager.AppDomain);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(string.Format("Job manager caught exception."));
                System.Console.WriteLine(ex);
            }
            finally
            {
                // Ensure that all objects finalize before the application ends 
                // by calling the garbage collector and waiting.
                System.Console.WriteLine("Waiting for garbage collector...");
                GC.Collect();
                GC.WaitForPendingFinalizers();
                System.Console.WriteLine("Press any key to exit...");
                System.Console.ReadKey();
            }
        }

        void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            stopWaitEvent.Set();
        }

        public static void Main(string[] args)
        {
            System.Console.WriteLine("IMI Server");
            System.Console.WriteLine(string.Format("version {0}", Assembly.GetExecutingAssembly().GetName().Version));
            System.Console.WriteLine("Copyright © 1993-2005 by Industri-Matematik International\n");

            CommandLineParser cmd = new CommandLineParser(args);
            
            string SystemId = cmd["SystemId"];

            if (String.IsNullOrEmpty(SystemId))
            {
                System.Console.WriteLine(string.Format("Error 0x0001 - No SystemId was supplied.\nUsage: {0} /SystemId <SystemId>",
                    Path.GetFileName(Assembly.GetExecutingAssembly().Location)));

                System.Console.ReadKey();

                return;
            }

            ConsoleHost host = new ConsoleHost();
            host.StartServer(SystemId);
        }
    }
}