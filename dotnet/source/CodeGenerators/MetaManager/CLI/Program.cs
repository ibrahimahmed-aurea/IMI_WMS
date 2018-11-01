using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Shared.Configuration;


namespace Cdc.MetaManager.CLI
{
    class Program
    {
        public struct CommandLineParameters
        {
            public static string INSTANCE = "Instance";
            public static string FRONTEND = "Frontend";
            public static string BACKEND = "Backend";
            public static string ACTION = "Action";

        }

        static int Main(string[] args)
        {

            if (args.Length > 0)
            {
                try
                {
                    Console.Clear();
                }
                catch
                { 
                }

                string instanceName = string.Empty;


                Imi.Framework.Shared.CommandLineParser parser = new Imi.Framework.Shared.CommandLineParser(args);

                instanceName = parser.GetParameterValue(CommandLineParameters.INSTANCE);

                List<string> frontendParameters = new List<string>();
                List<string> backendParameters = new List<string>();

                frontendParameters = parser.GetNamedParameterValues(CommandLineParameters.FRONTEND);
                backendParameters = parser.GetNamedParameterValues(CommandLineParameters.BACKEND);

                List<string> frontendApplications = new List<string>();
                List<string> backendApplications = new List<string>();

                if (frontendParameters != null)
                {
                    foreach (string parameter in frontendParameters)
                    {
                        foreach (string splitParam in parameter.Split(','))
                        {
                            if (!frontendApplications.Contains(splitParam) && !string.IsNullOrEmpty(splitParam))
                            {
                                frontendApplications.Add(splitParam);
                            }
                        }
                    }
                }

                if (backendParameters != null)
                {
                    foreach (string parameter in backendParameters)
                    {
                        foreach (string splitParam in parameter.Split(','))
                        {
                            if (!backendApplications.Contains(splitParam) && !string.IsNullOrEmpty(splitParam))
                            {
                                backendApplications.Add(splitParam);
                            }
                        }
                    }
                }

                string action = parser.GetParameterValue(CommandLineParameters.ACTION);

                Dictionary<string, string> options = new Dictionary<string, string>();

                foreach (string optionParameter in ConsoleInstance.CommandLineOptionParameters.GetAllOptionParameters())
                {
                    if (!string.IsNullOrEmpty(parser.GetParameterValue(optionParameter)))
                    {
                        options.Add(optionParameter, parser.GetParameterValue(optionParameter));
                    }
                }

                if (!string.IsNullOrEmpty(instanceName) && !string.IsNullOrEmpty(action))
                {

                    ConsoleInstance instance = InstanceFactory.CreateInstance<ConsoleInstance>("MetaManager.CLI", "Cdc.MetaManager.CLI.ConsoleInstance", instanceName);

                    return instance.Run(action, frontendApplications, backendApplications, options);

                }
                else
                {
                    Console.WriteLine("No instance or action specified");

                   return 1;
                }

            }

            return 0;
        }


    }
}
