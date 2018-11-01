using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser.TemplateObjects;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser
{
    public static class FileTemplateParser
    {

        private const string headerText = 
@"//
// Generated code from template: {0}
// Created on: {1}
//
{2}
";

        public static string CreateAssembly(IList<string> sourceFileList, IList<string> referencesFileList, string assemblyName, bool includeDebugInformation, out CompilerErrorCollection compilerErrors)
        {
            string pathToAssembly = string.Empty;
            compilerErrors = new CompilerErrorCollection();

            if (sourceFileList != null && sourceFileList.Count > 0)
            {
                CSharpCodeProvider provider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });

                CompilerParameters options = new CompilerParameters();
                options.GenerateInMemory = false;
                options.OutputAssembly = assemblyName;
                options.IncludeDebugInformation = includeDebugInformation;

                options.ReferencedAssemblies.Add("System.dll");
                options.ReferencedAssemblies.Add("System.Core.dll");
                options.ReferencedAssemblies.Add("System.Data.dll");
                options.ReferencedAssemblies.Add("System.Xml.dll");
                options.ReferencedAssemblies.Add("System.Xml.Linq.dll");
                options.ReferencedAssemblies.Add("System.Workflow.Activities.dll");
                options.ReferencedAssemblies.Add("System.Workflow.ComponentModel.dll");

                if (referencesFileList.Count > 0)
                {
                    foreach (string reference in referencesFileList)
                    {
                        options.ReferencedAssemblies.Add(reference);
                    }
                }

                CompilerResults results = provider.CompileAssemblyFromFile(options, sourceFileList.ToArray());

                if (results.Errors.Count > 0)
                {
                    compilerErrors.AddRange(results.Errors);
                }
                else
                {
                    results.CompiledAssembly.GetName().Version = new Version("1.0.0.0");

                    pathToAssembly = results.PathToAssembly;
                }
            }

            return pathToAssembly;
        }

        public static IList<string> GetSourceFiles(string sourceDir, IList<string> ignoreEntities)
        {
            List<string> sourceList = new List<string>();

            if (Directory.Exists(sourceDir))
            {
                string[] Files = Directory.GetFileSystemEntries(sourceDir);

                foreach (string Element in Files)
                {
                    bool ignore = false;

                    // Check if should be ignored
                    if (ignoreEntities != null)
                    {
                        var foundEntityList = from ignoreEntity in ignoreEntities
                                              where Element.StartsWith(ignoreEntity, StringComparison.CurrentCultureIgnoreCase)
                                              select ignoreEntity;

                        ignore = foundEntityList.Count() > 0;
                    }

                    if (!ignore)
                    {
                        // Sub directories
                        if (Directory.Exists(Element))
                        {
                            sourceList.AddRange(GetSourceFiles(Element, ignoreEntities));
                        }
                        else
                        {
                            if (Path.GetExtension(Element).ToUpper().Equals(".CS"))
                            {
                                // Add the filename to the list
                                sourceList.Add(Element);
                            }
                        }
                    }
                }
            }

            return sourceList;
        }

        private static string rootDirectory = null;

        public static IList<string> ParseTemplates(string sourceDir, string destinationDir, bool includeSubDirs, string nameSpace, string defaultClassInherit)
        {
            string temp = rootDirectory;

            try
            {
                if (temp == null)
                {
                    rootDirectory = sourceDir;
                }

                // Parse all templates
                IList<string> parsedTemplates = DoParseTemplates(sourceDir, destinationDir, includeSubDirs, nameSpace, defaultClassInherit);

                // Add the CodeTemplate embedded resource to the destination directory
                AddEmbeddedResourcesToDestinationDir(destinationDir, nameSpace);


                return parsedTemplates;
            }
            finally
            {
                if (rootDirectory != temp)
                {
                    rootDirectory = temp;
                }
            }
        }

        private static IList<string> DoParseTemplates(string sourceDir, string destinationDir, bool includeSubDirs, string nameSpace, string defaultClassInherit)
        {
            String[] Files;
            TemplateParser templateParser = new TemplateParser();
            List<string> parsedTemplates = new List<string>();

            if (!Directory.Exists(destinationDir))
                Directory.CreateDirectory(destinationDir);

            Files = Directory.GetFileSystemEntries(sourceDir);

            foreach (string Element in Files)
            {
                // Sub directories
                if (Directory.Exists(Element) && includeSubDirs)
                {
                    parsedTemplates.AddRange(DoParseTemplates(Element, Path.Combine(destinationDir, Path.GetFileName(Element)), includeSubDirs, nameSpace, defaultClassInherit));
                }
                else
                {
                    string destinationFileName = Path.Combine(destinationDir, Path.GetFileNameWithoutExtension(Element) + ".cs");

                    // Check if it's a template file
                    if (Path.GetExtension(Element).ToUpper().Equals(".CST"))
                    {
                        string className = Path.GetFileNameWithoutExtension(Element);

                        // Open the file and get content
                        string templateContent = File.ReadAllText(Element);

                        List<TemplateObject> list = templateParser.Parse(templateContent);

                        if (list.Count > 0 && !string.IsNullOrEmpty(className))
                        {
                            // Add this file to parsed templates list
                            parsedTemplates.Add(Element);

                            // Add header to template code
                            string shortFileName = Element.Replace(rootDirectory, "");
                            if (shortFileName.StartsWith(@"\"))
                            {
                                shortFileName = "." + shortFileName;
                            }
                            else
                            {
                                shortFileName = @".\" + shortFileName;
                            }

                            string classContent = ParsedClassResult.CreateContents(nameSpace, className, defaultClassInherit, list, shortFileName);

                            classContent = string.Format(headerText, shortFileName, DateTime.Now.ToLocalTime(), classContent);
                            File.WriteAllText(destinationFileName, classContent);
                        }
                    }
                }
            }
            return parsedTemplates;
        }

        private static void AddEmbeddedResourcesToDestinationDir(string destinationDir, string nameSpace)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();

            List<string> resourceList = executingAssembly.GetManifestResourceNames().ToList();

            if (resourceList.Count > 0)
            {
                foreach (string resource in resourceList)
                {
                    StreamReader sr = new StreamReader(executingAssembly.GetManifestResourceStream(resource));
                    string resourceContent = sr.ReadToEnd();
                    sr.Close();

                    // Replace the namespace to the one given
                    resourceContent = resourceContent.Replace("NAMESPACE_IS_REPLACED_WHEN_WRITTEN_TO_DISC", nameSpace);

                    // Find the last XXXX.XXX in the string.
                    string filename = Regex.Match(resource, @"(\w+\.\w+)$").Value;

                    if (!string.IsNullOrEmpty(filename))
                    {
                        StreamWriter sw = new StreamWriter(File.Open(Path.Combine(destinationDir, filename), FileMode.Create));
                        sw.Write(resourceContent);
                        sw.Close();
                    }
                }
            }
        }

    }
}
