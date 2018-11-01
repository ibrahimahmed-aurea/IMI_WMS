using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Cdc.HbmGenerator.Support;
using System.IO;
using System.Text.RegularExpressions;
using Cdc.HbmGenerator.Output;

namespace Cdc.HbmGenerator
{
    public class GeneratorEngine
    {
        public static void Generate(string fileName, string namespaceToSearch, string directoryName, bool forceAttribute)
        {
            string[] files = Directory.GetFiles(Path.GetDirectoryName(fileName), "*.dll");
            
            string targetPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            foreach (string file in files)
            {
                if (!File.Exists(Path.Combine(targetPath, Path.GetFileName(file))))
                    File.Copy(file, Path.Combine(targetPath, Path.GetFileName(file)));
            }

            Assembly assembly = Assembly.LoadFile(fileName);
            List<string> classes = AssemblyReflector.ListClasses(assembly);

            Console.WriteLine("Directory is " + directoryName);

            // Search for this namespace
            string regexFind = "^" + namespaceToSearch.Replace(".", @"\.") + @"\.\w+$";
            
            foreach (string orgClassName in classes)
            {
                try
                {
                    // Ignore classes that doesn't exist in the given namespace
                    if (!Regex.Match(orgClassName, regexFind).Success)
                    {
                        Console.WriteLine(" Skip " + orgClassName); // togr

                        continue;
                    }

                    Type orgType = assembly.GetType(orgClassName);
                    Class reflectClass = ClassReflector.ReflectType(orgType,forceAttribute);

                    if (reflectClass == null)
                    {
                        Console.WriteLine(" Skip2 " + orgClassName); // togr
                        continue;
                    }

                    Console.WriteLine(" Generate " + orgClassName); // togr

                    string hibernateMap = HibernateMapGenerator.Generate(reflectClass);

                    string shortFileName = string.Format(@"{0}.hbm.xml", reflectClass.Type.Name);
                    string outputFileName = string.Format(@"{0}\{1}", directoryName, shortFileName);

                    bool writeFile = true;

                    Console.WriteLine("file is " + outputFileName);

                    // Check if outputfile exist
                    if (File.Exists(outputFileName))
                    {
                        // Read contents
                        string existingContent = File.ReadAllText(outputFileName, Encoding.UTF8);

                        // Check if contents have changed
                        if (existingContent == hibernateMap)
                        {
                            writeFile = false;
                        }
                    }

                    if (writeFile)
                    {
                        try
                        {
                            if (File.Exists(outputFileName))
                            {
                                // Set attributes to Archive because file could be readonly
                                File.SetAttributes(outputFileName, FileAttributes.Archive);
                            }

                            // Write the file to disc
                            StreamWriter w = new StreamWriter(outputFileName, false, Encoding.UTF8);
                            w.Write(hibernateMap);
                            w.Close();
                            Console.WriteLine("Created file {0}", shortFileName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed to create file {0} with exception {1}", shortFileName, ex);
                        }
                    }
                }
                catch (Exception outerEx)
                {
                    Console.WriteLine("Failed to generate code for class {0} with exception {1}", orgClassName, outerEx);
                }
            }
            Console.WriteLine("Done!");
        }
    }
}
