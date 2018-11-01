using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Job.Engine;
using System.Threading;
using Imi.Framework.Shared.Configuration;
using System.IO;

namespace Imi.SupplyChain.Server.Console.Support
{
    public class ConsoleHost : ApplicationInstance
    {
        private JobManager jobManager;
        private ManualResetEvent stopWaitEvent;

        public event EventHandler Starting;

        public ConsoleHost(string instanceName)
            : base(instanceName)
        {
            //testThread = new Thread(Process);   
        }

        public void StartServer(string dllDirectory)
        {
            List<string> directories = new List<string>();

            if (!string.IsNullOrEmpty(dllDirectory))
            {
                directories.Add(dllDirectory);
            }

            StartServer(directories);
        }

        public void StartServer(IList<string> dllDirectories)
        {
            if (dllDirectories != null)
            {
                foreach (string directory in dllDirectories)
                {
                    string fullPath = Path.GetFullPath(directory);

                    DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
                    FileInfo[] files = directoryInfo.GetFiles("*.dll");

                    foreach (FileInfo copyfile in files)
                    {
                        string newFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,copyfile.Name);
                        if (File.Exists(newFile))
                        {
                            File.SetAttributes(newFile, FileAttributes.Archive);
                        }

                        File.Copy(copyfile.FullName, newFile,true);
                    }
                }
            }

            StartServer();
        }

        public void StartServer()
        {

            System.Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
            stopWaitEvent = new ManualResetEvent(false);

            try
            {
                jobManager = InstanceFactory.CreateInstance<JobManager>("Imi.Framework.Job", "Imi.Framework.Job.Engine.JobManager", InstanceName);

                if (Starting != null)
                {
                    jobManager.Starting += Starting;
                }

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

    }
}
