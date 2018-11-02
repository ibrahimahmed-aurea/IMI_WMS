using System;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Configuration;
using Imi.SupplyChain.Server.Console.Support;
using Imi.Framework.Shared;
using Imi.Framework.Shared.Configuration;

namespace Imi.SupplyChain.Server.Console
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {XXXXXX
            System.Console.WriteLine("IMI Supply Chain Application Server");
            System.Console.WriteLine(string.Format("Version {0}", Assembly.GetExecutingAssembly().GetName().Version));
            System.Console.WriteLine("Copyright (c) Aptean\n");

            CommandLineParser cmd = new CommandLineParser(args);
            string instance = cmd["SystemId"];
                        
            if (String.IsNullOrEmpty(instance))
                instance = ConfigurationManager.AppSettings["SystemId"];

            if (String.IsNullOrEmpty(instance))
            {
                System.Console.WriteLine(string.Format("Error 0x0001 - No SystemId was supplied.\nUsage: {0} /SystemId <SystemId>",
                    Path.GetFileName(Assembly.GetExecutingAssembly().Location)));

                System.Console.ReadKey();

                return;
            }

            ConsoleHost host = new ConsoleHost(instance);
            host.StartServer();

        }
    }
}