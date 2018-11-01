using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Deployment.CLI.exe
{
    class Program
    {
        public struct CommandLineParameters
        {
            public static string INSTANCE = "Instance";
            public static string ACTION = "Action";
            public static string KITFILE = "KitFile";
        }

        private struct ActionParameters
        {
            public const string IMPORT = "Import";
            public const string PUBLISH = "Publish";
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

                Imi.Framework.Shared.CommandLineParser parser = new Imi.Framework.Shared.CommandLineParser(args);

                string action = parser.GetParameterValue(CommandLineParameters.ACTION);

                if (!string.IsNullOrEmpty(action))
                {
                    ActionImplemetation actionImplementation = new ActionImplemetation();
                    actionImplementation.StatusChanged += new ActionImplemetation.StatusChangedDelegate(actionImplementation_StatusChanged);
                    try
                    {
                        switch (action)
                        {
                            case ActionParameters.IMPORT:
                                string kitfile = parser.GetParameterValue(CommandLineParameters.KITFILE);
                                string instance = parser.GetParameterValue(CommandLineParameters.INSTANCE);
                                if (!string.IsNullOrEmpty(kitfile) && !string.IsNullOrEmpty(instance))
                                {
                                    actionImplementation.Import(kitfile, instance);
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return 1;
                    }
                }
            }

            return 0;
        }

        static void actionImplementation_StatusChanged(string message, int value, int min, int max)
        {
            try
            {
                Console.SetCursorPosition(0, 0);
            }
            catch
            {
            }
            
            if (max > 0)
            {
                Console.WriteLine(("Progress:     " + Math.Round((double)(((double)value / max) * 100), 2).ToString("0.00") + " %").PadRight(60));
            }
            else if (max == -1)
            {
                Console.WriteLine("                                       ");
            }
            else
            {
                Console.WriteLine("");
            }

            Console.WriteLine("==================================INFORMATION===================================");
            if (max == -1)
            {
                Console.WriteLine(message.PadRight(60));
                Console.WriteLine(" ".PadRight(160));
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine(message.PadRight(160));
            }
            
        }
    }
}
