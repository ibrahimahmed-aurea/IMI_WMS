using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Cdc.CodeGeneration.Caching;

namespace Cdc.Framework.Parsing.CSharpParsing
{
    public class ProjectAnalyser
    {
        private static ProjectInfo UpdateProjectNamespaces(ProjectInfo projectInfo)
        {
            IEnumerable<string> projectFileNames = FileCacheManager.GetSessionFilesInDirectory(projectInfo.ProjectDirectory.FullName, ".cs");

            List<string> usedNamespaces = new List<string>();
            List<string> notFoundNamespaces = new List<string>();
            List<string> localNamespaces = new List<string>();

            foreach (string fileName in projectFileNames)
            {
                string sourceCode = FileCacheManager.GetCachedUsingStatement(fileName);

                if (!string.IsNullOrEmpty(sourceCode))
                {
                    List<string> nameSpaces = SourceParser.Parse(sourceCode);
                    if (nameSpaces.Count > 0)
                    {
                        string localNamespace = nameSpaces[0];
                        nameSpaces.RemoveAt(0);
                        usedNamespaces.AddRange(nameSpaces);
                        localNamespaces.Add(localNamespace);
                    }
                }
            }

            // Add unique namespaces
            projectInfo.LocalNamespaces = new List<string>();
            projectInfo.LocalNamespaces.AddRange(localNamespaces.Distinct());
            projectInfo.UsedNamespaces = new List<string>();
            projectInfo.UsedNamespaces.AddRange(usedNamespaces.Distinct());

            return projectInfo;
        }

        public static string ReferenceDirectory { get; set; }

        public static List<string> NotFoundNamespaces { get; set; }

        public static List<string> GetServiceReferences(ProjectInfo projectInfo, string servicePath, string release)
        {
            Dictionary<string, string> foundServiceDlls = new Dictionary<string, string>();

            //
            // Find all servicecontract and datacontract namespaces
            // that are in the notFound list
            //
            foreach (string ns in NotFoundNamespaces)
            {
                if (ns.EndsWith("ServiceContracts") || ns.EndsWith("DataContracts"))
                    foundServiceDlls[ns] = null;
            }

            //
            // Scan directory for the files
            //
            DirectoryInfo di = new DirectoryInfo(servicePath);

            Dictionary<string, string> finalList = new Dictionary<string, string>();

            foreach (string serviceNamespace in foundServiceDlls.Keys)
            {
                string fileName = string.Format("{0}.dll", serviceNamespace);
                FileInfo[] files = di.GetFiles(fileName, SearchOption.AllDirectories);

                if(files.Length > 0)
                {
                    string releaseDir = string.Format(@"bin\{0}\", release);

                    foreach (FileInfo fi in files)
                    {
                        if(fi.FullName.Contains(releaseDir))
                        {
                            finalList[serviceNamespace] = files[0].FullName;
                            break;
                        }
                    }
                }
            }


            //
            // Create source for project file
            //
            Dictionary<string, string> finalUsedAssemblyDictionary = new Dictionary<string, string>();

            foreach (string serviceNamespace in finalList.Keys)
            {
                string fileName = finalList[serviceNamespace];

                if (fileName == null)
                    continue;

                Assembly sa = Assembly.ReflectionOnlyLoadFrom(fileName);

                string relativeFilename = PathHelper.RelativePathTo(projectInfo.ProjectDirectory.FullName, fileName);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("<Reference Include=\"{0}, processorArchitecture=MSIL\">", sa.FullName));
                sb.AppendLine("  <SpecificVersion>False</SpecificVersion>");
                sb.AppendLine(string.Format("  <HintPath>{0}</HintPath>", relativeFilename));
                sb.AppendLine("</Reference>");

                sb.Replace(", PublicKeyToken=null", "");

                finalUsedAssemblyDictionary[serviceNamespace] = sb.ToString();
            }

            return finalUsedAssemblyDictionary.Values.ToList<string>();
        }


        public static List<string> GetAssemblyReferences(ProjectInfo projectInfo, List<ProjectInfo> localProjects)
        {
            projectInfo = ProjectAnalyser.UpdateProjectNamespaces(projectInfo);

            Dictionary<string, string> usedAssemblyDictionary = NamespaceAnalyser.GetAssemblyReferences(projectInfo.UsedNamespaces);

            NotFoundNamespaces = NamespaceAnalyser.NotFoundNamespaces;

            if (localProjects != null)
            {
                string[] localCopy = NotFoundNamespaces.ToArray();

                for (int i = 0; i < localCopy.Length; i++)
                {
                    string ns = localCopy[i];

                    foreach (ProjectInfo localProject in localProjects)
                    {
                        if (localProject.Namespace.Equals(ns))
                        {
                            // No need to reference yourself
                            if (projectInfo != localProject)
                            {
                                string referenceSource = GetLocalProjectReferenceSource(projectInfo, localProject);
                                usedAssemblyDictionary[localProject.Namespace] = referenceSource;
                            }
                            NotFoundNamespaces.Remove(ns);
                        }
                    }
                }

                if (NotFoundNamespaces.Count > 0)
                {
                    localCopy = NotFoundNamespaces.ToArray();

                    for (int i = 0; i < localCopy.Length; i++)
                    {
                        string ns = localCopy[i];
                        string bestMatch = null;
                        ProjectInfo bestProject = null;

                        foreach (ProjectInfo localProject in localProjects)
                        {
                            if (ns.Contains(localProject.Namespace))
                            {
                                if ((bestMatch == null) || (bestMatch.Length < localProject.Namespace.Length))
                                {
                                    // No need to reference yourself
                                    if (projectInfo != localProject)
                                    {
                                        bestProject = localProject;
                                    }
                                    bestMatch = localProject.Namespace;
                                }
                            }
                        }

                        if (bestMatch != null)
                        {
                            if (bestProject != null)
                            {
                                string referenceSource = GetLocalProjectReferenceSource(projectInfo, bestProject);
                                usedAssemblyDictionary[bestProject.Namespace] = referenceSource;
                            }
                            NotFoundNamespaces.Remove(ns);
                        }

                    }
                }

            }

            Dictionary<string, string> finalUsedAssemblyDictionary = new Dictionary<string, string>();
            // Set referencepaths
            string relativeRefPath = PathHelper.RelativePathTo(projectInfo.ProjectDirectory.FullName, ReferenceDirectory);

            foreach (string sourceKey in usedAssemblyDictionary.Keys)
            {
                if (usedAssemblyDictionary[sourceKey].Contains(@"{references}"))
                    finalUsedAssemblyDictionary[sourceKey] = usedAssemblyDictionary[sourceKey].Replace(@"{references}", relativeRefPath);
                else
                    finalUsedAssemblyDictionary[sourceKey] = usedAssemblyDictionary[sourceKey];
            }

            return finalUsedAssemblyDictionary.Values.ToList<string>();
        }

        private static string GetLocalProjectReferenceSource(ProjectInfo projectInfo, ProjectInfo refProject)
        {
            string relativePath = PathHelper.RelativePathTo(projectInfo.ProjectDirectory.FullName, refProject.ProjectDirectory.FullName);

            string projectFilename = new FileInfo(refProject.ProjectFile).Name;

            string source = string.Format("    <ProjectReference Include=\"{0}\">" + Environment.NewLine +
                                          "      <Project>{{{1}}}</Project>" + Environment.NewLine +
                                          "      <Name>{2}</Name>" + Environment.NewLine +
                                          "    </ProjectReference>",
                                          System.IO.Path.Combine(relativePath, projectFilename), refProject.ProjectGuid, refProject.Namespace);

            return (source);
        }
    }
}
