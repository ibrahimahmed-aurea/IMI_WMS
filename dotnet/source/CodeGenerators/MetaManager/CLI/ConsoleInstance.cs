using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.BusinessLogic;
using Imi.Framework.Shared.Configuration;

namespace Cdc.MetaManager.CLI
{
    public class ConsoleInstance : ApplicationInstance
    {
        private static long startticks;
        private static object lockObj = new object();

        public struct CommandLineOptionParameters
        {
            public static string IGNORECHECKOUTS = "IgnoreCheckOuts";


            public static List<string> GetAllOptionParameters()
            {
                List<string> parameters = new List<string>();

                parameters.Add(IGNORECHECKOUTS);

                return parameters;
            }
        }

        public struct ActionValues
        {
            public const string GENERATE = "generate";
        }

        public ConsoleInstance(string instanceName)
            : base(instanceName)
        {
        }

        public int Run(string action, List<string> frontendApplications, List<string> backendApplications, Dictionary<string, string> options)
        {

            startticks = DateTime.Now.Ticks;

            MetaManagerServices.GetConfigurationManagementService().StatusChanged += new StatusChangedDelegate(Program_StatusChanged);
            MetaManagerServices.GetModelService().StatusChanged += new StatusChangedDelegate(Program_StatusChanged);



            switch (action)
            {
                case ActionValues.GENERATE:
                    {
                        if (frontendApplications.Count > 0 && backendApplications.Count > 0)
                        {
                            overallProgressMax = "3";
                            bool ignoreCheckOuts = true;

                            if (options.ContainsKey(CommandLineOptionParameters.IGNORECHECKOUTS))
                            {
                                ignoreCheckOuts = Convert.ToBoolean(options[CommandLineOptionParameters.IGNORECHECKOUTS]);

                                if (ignoreCheckOuts) { overallProgress = "1"; }
                            }
                            else
                            {
                                overallProgress = "1";
                            }

                            try
                            {
                                MetaManagerServices.GetModelService().GenerateApplication(frontendApplications, backendApplications, ignoreCheckOuts);

                                Program_StatusChanged("Code generation compleate", 0, 0, 0);
                            }
                            catch
                            {
                                return 1;
                            }
                        }
                        else
                        {
                            Program_StatusChanged("Both frontend and backend application must be specefied", 0, 0, 0);
                            return 1;
                        }
                        break;
                    }
            }

            return 0;
        }

        private static string overallProgress = "0";
        private static string overallProgressMax = "0";

        static void Program_StatusChanged(string message, int value, int min, int max)
        {
            lock (lockObj)
            {
                try
                {
                    Console.SetCursorPosition(0, 0);
                }
                catch
                { 
                }

                Console.WriteLine("Running time: " + Math.Round(TimeSpan.FromTicks(DateTime.Now.Ticks - startticks).TotalMinutes, 2).ToString("00.00") + " minutes");

                if (message.StartsWith("[") & message.EndsWith("]"))
                {
                    string[] info = message.Split(',');

                    if (info[2].StartsWith("END"))
                    {
                        overallProgress = info[1];
                    }
                }

                Console.WriteLine("Overall Progress: " + overallProgress + "/" + overallProgressMax + " Completed.".PadRight(5));

                if (max != 0)
                {
                    Console.WriteLine(("Progress:     " + Math.Round((double)(((double)value / max) * 100), 2).ToString("0.00") + " %").PadRight(60));
                }
                else
                {
                    Console.WriteLine("");
                }
                Console.WriteLine("==================================INFORMATION===================================");
                Console.WriteLine(message.PadRight(240));
            }
        }
    }
}
