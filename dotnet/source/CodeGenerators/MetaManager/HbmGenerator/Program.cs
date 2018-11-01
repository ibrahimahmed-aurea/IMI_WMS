using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Cdc.HbmGenerator.Support;
using System.Text.RegularExpressions;

namespace Cdc.HbmGenerator
{
    class Program
    {
        private const string usage = "HbmGenerator assemblyFileName namespaceToSearch [outputDirectory]";

        static void Main(string[] args)
        {
            //
            // Name of dll, output, output directory
            //
            if (args.Length < 1)
            {
                throw new ArgumentException("Wrong number of parameters. {0}", usage);
            }

            string fileName = args[0];
            string namespaceToSearch = args[1];
            string directoryName = @".\";
            bool forceAttribute = false;

            if (args.Length >= 3)
            {
                directoryName = args[2];
                if(! Directory.Exists(directoryName))
                    throw new ArgumentException(string.Format("Directory does not exist ({0}). {1}", directoryName, usage));
            }

            if (args.Length == 4)
            {
                string force = args[3];
                forceAttribute = force.ToLower().Equals("true");
                Console.WriteLine("Force is true"); // togr
            }
            else
                Console.WriteLine("Force is false"); // togr


            if (!string.IsNullOrEmpty(fileName))
            {
                if (! File.Exists(fileName))
                {
                    throw new ArgumentException(string.Format("Assembly is missing ({0}). {1}", fileName, usage));
                }
            }
            else
            {
                throw new ArgumentException(string.Format("No filename was given. {0}", usage));
            }

            if (string.IsNullOrEmpty(namespaceToSearch))
            {
                throw new ArgumentException(string.Format("No namespace was given. {0}", usage));
            }
            else
            {
                if (!Regex.Match(namespaceToSearch, @"^\w+(\.\w+)*$").Success)
                {
                    throw new ArgumentException("Namespace has invalid format. Should be something like \"XXX.YYY.ZZZ\" without the quotes.");
                }
            }

            Console.WriteLine("Directory is " + directoryName);
            GeneratorEngine.Generate(fileName, namespaceToSearch, directoryName, forceAttribute);
            Console.ReadLine();


        }
    }
}
