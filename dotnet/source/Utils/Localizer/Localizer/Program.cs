using System;
using System.IO;
using System.Reflection;

namespace Imi.Utils.Localizer
{    
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("IMI Supply Chain Localization Tool");
            Console.WriteLine("Version " + Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine("Copyright (c) 2013 Aptean. All rights reserved.");
            Console.WriteLine("");
            //try
            //{
                CommandLineParser parser = new CommandLineParser(args);

                if ((parser.IsEnabled("h") || parser.IsEnabled("help")) || parser.IsEnabled("?"))
                {
                    PrintUsage();
                }
                else
                {

                    int count = args.Length;
                    bool compile = false;
                    if (parser.IsEnabled("v") || parser.IsEnabled("verbose"))
                    {
                        Imi.Utils.Localizer.Localizer.verbose = true;
                        count--;
                    }

                    if (parser.IsEnabled("c") || parser.IsEnabled("compile"))
                    {
                        compile = true;
                        count--;
                    }

                    if (args[0] == "OUTPUT")
                    {
                        Imi.Utils.Localizer.Localizer.CreateOutput(args[1], args[2]);
                    }
                    else if (args[0] == "SOLUTIONEXTRACT")
                    {
                        Imi.Utils.Localizer.Localizer.ExtractResourcesFromVSSolution(args[1]);
                    }
                    else if (args[0] == "PROJECTEXTRACT")
                    {
                        Imi.Utils.Localizer.Localizer.ExtractResourcesFromResx(args[1]);
                    }
                    else if (args[0] == "UPDATERESXFROMFILE")
                    {
                        Imi.Utils.Localizer.Localizer.UpdateResources("resx", "resxextract");
                    }
                    else if (args[0] == "UPDATEXMLFROMOLDXLS")
                    {
                        Imi.Utils.Localizer.Localizer.UpdateResources("resx", "oldxls");
                    }
                    else
                    {
                        PrintUsage();
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    if (ex.InnerException != null)
            //    {
            //        Console.WriteLine(ex.InnerException.Message);
            //    }
            //    throw;
            //}

            Console.WriteLine("IMI Supply Chain Localization Tool: EXIT OK");
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("localizer inputPath outputFile.xls [/v]");
            Console.WriteLine("localizer inputFile.xls outputPath [/v] [/c]");
            Console.WriteLine("");
            Console.WriteLine("Where:");
            Console.WriteLine("inputPath            Path to resx input files.");
            Console.WriteLine("outputFile.xls       Excel file to generate or update.");
            Console.WriteLine("inputFile.xls        Excel file to generate localized resources from.");
            Console.WriteLine("outputPath           Path to write localized resource files.");
            Console.WriteLine("");
            Console.WriteLine("Converts one or more resx files to Excel format. If inputPath is not");
            Console.WriteLine("specified, localized resx files will be generated from inputFile.xls.");
            Console.WriteLine("");
            Console.WriteLine("Options:");
            Console.WriteLine("/c[ompile]           Compiles localized resources into assemblies.");
            Console.WriteLine("/v[erbose]           Output trace information.");
            Console.WriteLine("/h[elp] or /?        Display this usage message.");
            Console.WriteLine("");
        }
    }
}

