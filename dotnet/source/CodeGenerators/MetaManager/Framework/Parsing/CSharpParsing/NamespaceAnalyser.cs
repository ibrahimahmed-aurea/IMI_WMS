using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;

namespace Cdc.Framework.Parsing.CSharpParsing
{
    public static class NamespaceAnalyser
    {
        private static Dictionary<string, AssemblyReference> namespaceDictionary = new Dictionary<string, AssemblyReference>();
        private static Dictionary<string, AssemblyReference> assemblyDictionary = new Dictionary<string, AssemblyReference>();


        private static void Initialize()
        {
            Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream dictionaryFile =
                thisExe.GetManifestResourceStream("Cdc.Framework.Parsing.CSharpParsing.NamespaceDictionary.xml");

            StreamReader r = new StreamReader(dictionaryFile);
            XmlSerializer ser = new XmlSerializer(typeof(Crossreference));
            Crossreference cr = ser.Deserialize(r) as Crossreference;

            foreach (AssemblyReference assemblyRef in cr.Assemblies)
            {
                assemblyDictionary[assemblyRef.Name] = assemblyRef;
            }

            foreach (NamespaceReference namespaceRef in cr.Namespaces)
            {
                namespaceDictionary.Add(namespaceRef.Name, assemblyDictionary[namespaceRef.AssemblyName]);
            }

        }

        public static List<string> NotFoundNamespaces { get; set; }

        /// <summary>
        /// Creates unique dictionary of assemblies and the source needed for codegeneration
        /// </summary>
        /// <param name="namespaces"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetAssemblyReferences(List<string> namespaces)
        {
            if (namespaceDictionary.Count == 0)
            {
                Initialize();
            }

            Dictionary<string, string> assemblyList = new Dictionary<string, string>();
            List<string> localNotFoundNamespaces = new List<string>();

            foreach (string refNamespace in namespaces)
            {
                if (namespaceDictionary.ContainsKey(refNamespace))
                {
                    AssemblyReference assem = namespaceDictionary[refNamespace];
                    assemblyList[assem.Name] = assem.Source;
                    //List<string> dependencies = ExpandDependencies(assem);

                    //foreach (string assemName in dependencies)
                    //{
                    //    AssemblyReference refAssem = assemblyDictionary[assemName];
                    //    assemblyList[refAssem.Name] = refAssem.Source;
                    //}
                }
                else
                {
                    localNotFoundNamespaces.Add(refNamespace);
                }
            }

            string[] assemNames = assemblyList.Keys.ToArray();

            foreach (string assemName in assemNames)
            {
                AssemblyReference assem = assemblyDictionary[assemName];
                List<string> dependencies = ExpandDependencies(assem);
                foreach (string assemRefName in dependencies)
                {
                    AssemblyReference refAssem = assemblyDictionary[assemRefName];
                    assemblyList[refAssem.Name] = refAssem.Source;
                }
            }

            NotFoundNamespaces = new List<string>();
            NotFoundNamespaces.AddRange(localNotFoundNamespaces);

            return assemblyList;
        }

        private static List<string> ExpandDependencies(AssemblyReference assem)
        {
            List<string> result = new List<string>();

            if (string.IsNullOrEmpty(assem.Depends))
                return result;

            string[] moreAssem = assem.Depends.Split(new char[] { ',' });

            result.AddRange(moreAssem);

            foreach (string refAssemblyName in moreAssem)
            {
                AssemblyReference refAssem = assemblyDictionary[refAssemblyName];
                List<string> dependencies = ExpandDependencies(refAssem);
                if (dependencies.Count > 0)
                {
                    result.AddRange(dependencies);
                }
            }

            return result;
        }


    }
}
