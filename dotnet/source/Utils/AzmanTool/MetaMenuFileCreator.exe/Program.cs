using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuthorizationParser;

namespace MetaMenuFileCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "?")
            {
                Console.WriteLine("MetaMenuFileCreator\r\n");
                Console.WriteLine("Usage: MetaMenuFileCreator [Metadata Path] [MetaMenu Output File Path]");
            }
            else
            {
                bool paramsOk = false;
                StringBuilder errorMessage = new StringBuilder();

                string metadataDirectory = "";
                string targetLocationDirectory = "";

                if (args.Length == 2)
                {
                    metadataDirectory = args[0];
                    targetLocationDirectory = args[1];

                    if (metadataDirectory.EndsWith("metadata"))
                    {
                        if (System.IO.Directory.Exists(metadataDirectory))
                        {
                            if (System.IO.Directory.Exists(targetLocationDirectory))
                            {
                                paramsOk = true;
                            }
                            else
                            {
                                errorMessage.AppendLine("Output directory does not exist.");
                            }
                        }
                        else
                        {
                            errorMessage.AppendLine("Metadata directory does not exist.");
                        }
                    }
                    else
                    {
                        errorMessage.AppendLine("Metadata path needs to end with \\Metadata.");
                    }
                }
                else
                {
                    errorMessage.AppendLine("Wrong number of arguments");
                }

                if (paramsOk)
                {

                    bool result = MetaDataParser.CreateMetaMenu(targetLocationDirectory, metadataDirectory);

                    if (result == false)
                    {
                        Console.WriteLine("Generating MetaMenu.xml Faild!");
                        Environment.Exit(-1);
                    }
                    else
                    {
                        Console.WriteLine("Generating MetaMenu.xml was Successful");
                    }
                }
                else
                {
                    Console.WriteLine(errorMessage.ToString());
                    Environment.Exit(-1);
                }
            }

            //Console.WriteLine("\r\n\r\nPress any key to Exit.");
            //Console.ReadKey();

        }
    }
}
