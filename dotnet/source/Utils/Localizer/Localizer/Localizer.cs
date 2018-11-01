using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Diagnostics;
using System.Xml;
using System.Configuration;

namespace Imi.Utils.Localizer
{
    public class Localizer
    {
        public static bool verbose;

        private static string _xlsFileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resx_Master.xls");

        private static AppSettingsSection settings = null;

        private static string _xmlFileName
        {
            get
            {
                return settings.Settings["resourceMasterFilePath"].Value;
            }
        }

        private static DateTime updateDateTime = new DateTime();

        private static XmlDocument _masterDoc;
        private static Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, XmlNode>>>>>>>>> _resourceDictionary;

        static Localizer()
        {
            settings = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap() { ExeConfigFilename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Localizer.config") }, ConfigurationUserLevel.None).AppSettings;
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(GetDefaultCultureName("resx"));
        }

        public static string GetDefaultCultureName(string resourceType)
        {

            //AppSettingsSection settings = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap() { ExeConfigFilename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Localizer.config") }, ConfigurationUserLevel.None).AppSettings;
            switch (resourceType)
            {
                case "resx":
                    return settings.Settings["resx_default_cultureName"].Value;
            }

            return string.Empty;
        }

        public static List<string> GetSupportedTranslationsCultureNames(string resourceType)
        {
            //AppSettingsSection settings = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap() { ExeConfigFilename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Localizer.config") }, ConfigurationUserLevel.None).AppSettings;

            List<string> returnList = new List<string>();

            switch (resourceType)
            {
                case "resx":
                    string cultureNames = settings.Settings["resx_translation_cultureNames"].Value;
                    foreach (string cultureName in cultureNames.Split(','))
                    {
                        returnList.Add(cultureName);
                    }
                    break;
            }

            return returnList;
        }

        public static void RemoveSupportedTranslationCultureName(string resourceType, string cultureName)
        {
            switch (resourceType)
            {
                case "resx":
                    string cultureNames = settings.Settings["resx_translation_cultureNames"].Value;
                    if (cultureNames.StartsWith(cultureName))
                    {
                        cultureNames = cultureNames.Replace(cultureName + ",", "");
                    }
                    else
                    {
                        cultureNames = cultureNames.Replace("," + cultureName, "");
                    }
                    settings.Settings["resx_translation_cultureNames"].Value = cultureNames;
                    settings.CurrentConfiguration.Save();
                    break;
            }
        }

        private static void LoadMaster(string tmpXmlFileName = "")
        {
            _masterDoc = new XmlDocument();

            try
            {
                if (string.IsNullOrEmpty(tmpXmlFileName))
                {
                    _masterDoc.Load(_xmlFileName);
                }
                else
                {
                    _masterDoc.Load(tmpXmlFileName);
                }
            }
            catch
            {
            }

            _resourceDictionary = new Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, XmlNode>>>>>>>>>();

            XmlNodeList resourceTypes = _masterDoc.GetElementsByTagName("ResourceType");

            if (resourceTypes.Count == 0)
            {
                _masterDoc = new XmlDocument();
                _masterDoc.CreateXmlDeclaration("1.0", "utf-8", "");
                _masterDoc.AppendChild(_masterDoc.CreateElement("Resources"));


                XmlElement resxType = _masterDoc.CreateElement("ResourceType");
                resxType.SetAttribute("Type", "resx");
                resxType.SetAttribute("Updated", DateTime.Now.ToString());

                _masterDoc.ChildNodes[0].AppendChild(resxType);

                resourceTypes = _masterDoc.GetElementsByTagName("ResourceType");
            }

            foreach (XmlNode resourceType in resourceTypes)
            {
                string typeName = resourceType.Attributes["Type"].Value;
                _resourceDictionary.Add(typeName, new KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, XmlNode>>>>>>>>(resourceType, new Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, XmlNode>>>>>>>()));

                foreach (XmlNode module in resourceType.ChildNodes)
                {
                    string moduleName = module.Attributes["Name"].Value;
                    _resourceDictionary[typeName].Value.Add(moduleName, new KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, XmlNode>>>>>>(module, new Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, XmlNode>>>>>()));

                    foreach (XmlNode resourceContainer in module.ChildNodes)
                    {
                        string resourceContainerKey = resourceContainer.Attributes["Key"].Value;

                        _resourceDictionary[typeName].Value[moduleName].Value.Add(resourceContainerKey, new KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, XmlNode>>>>(resourceContainer, new Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, XmlNode>>>()));

                        foreach (XmlNode resource in resourceContainer.ChildNodes)
                        {
                            string resourceKey = resource.Attributes["Key"].Value;
                            _resourceDictionary[typeName].Value[moduleName].Value[resourceContainerKey].Value.Add(resourceKey, new KeyValuePair<XmlNode, Dictionary<string, XmlNode>>(resource, new Dictionary<string, XmlNode>()));

                            foreach (XmlNode translation in resource.ChildNodes)
                            {
                                _resourceDictionary[typeName].Value[moduleName].Value[resourceContainerKey].Value[resourceKey].Value.Add(translation.Attributes["CultureName"].Value, translation);
                            }
                        }
                    }
                }
            }
        }

        public static Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, XmlNode>>>>>>>>> GetMasterData(string tmpXmlFileName = "")
        {
            LoadMaster(tmpXmlFileName);

            return _resourceDictionary;
        }

        public static void ExtractResourcesFromVSSolution(string solutionPath)
        {
            if (File.Exists(solutionPath))
            {
                try
                {
                    string[] fileContent = File.ReadAllLines(solutionPath);

                    List<string> projects = fileContent.Where(s => s.StartsWith("Project") && s.Contains(".csproj")).ToList();

                    List<string> projectPaths = new List<string>();

                    foreach (string project in projects)
                    {
                        int index = project.LastIndexOf(".csproj\"") + 7;
                        int startIndex = project.IndexOf(",") + 1;

                        string lastPartOfPath = project.Substring(startIndex, (index - startIndex));
                        lastPartOfPath = lastPartOfPath.Replace("\"", "");
                        lastPartOfPath = lastPartOfPath.Trim();


                        string holePath = Path.Combine(Path.GetDirectoryName(solutionPath), lastPartOfPath);

                        projectPaths.Add(holePath);
                    }

                    foreach (string projectPath in projectPaths)
                    {
                        ExtractResourcesFromResx(projectPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }

                    throw;
                }
            }
        }

        public static void ExtractResourcesFromResx(string projectPath)
        {
            if (File.Exists(projectPath))
            {
                System.Xml.XmlDocument project = new System.Xml.XmlDocument();
                project.Load(projectPath);

                string assemblyExtension = string.Empty;

                if (project.GetElementsByTagName("OutputType")[0].InnerText.ToUpper().Contains("EXE"))
                {
                    assemblyExtension = ".exe";
                }
                else if (project.GetElementsByTagName("OutputType")[0].InnerText.ToUpper() == "LIBRARY")
                {
                    assemblyExtension = ".dll";
                }

                string assemblyName = project.GetElementsByTagName("AssemblyName")[0].InnerText + assemblyExtension;
                string assemblyNameSpace = project.GetElementsByTagName("RootNamespace")[0].InnerText;

                List<string> files = new List<string>();
                System.Xml.XmlNodeList nodeList = project.GetElementsByTagName("EmbeddedResource");

                foreach (System.Xml.XmlNode node in nodeList)
                {
                    if (node.Attributes["Include"].Value.EndsWith(".resx"))
                    {
                        string resxFilePath = Path.Combine(Path.GetDirectoryName(projectPath), node.Attributes["Include"].Value);

                        string resourceNamespace = assemblyNameSpace;

                        if (node.Attributes["Include"].Value.Contains(@"\"))
                        {
                            resourceNamespace = assemblyNameSpace + "." + node.Attributes["Include"].Value.Substring(0, node.Attributes["Include"].Value.LastIndexOf(@"\")).Replace(@"\", ".");
                        }

                        if (File.Exists(resxFilePath))
                        {
                            files.Add(resxFilePath + "|" + resourceNamespace);
                        }
                    }
                }

                foreach (string file in files)
                {
                    string filePath = file.Split('|')[0];
                    string fileNamespace = file.Split('|')[1];

                    if (verbose)
                    {
                        Console.WriteLine("Processing file \"{0}\"...", filePath);
                    }

                    if (!File.Exists(filePath))
                    {
                        continue;
                    }

                    string tmpFileName = Guid.NewGuid().ToString() + ".resxextract";

                    System.Xml.XmlDocument tmpDoc = new System.Xml.XmlDocument();

                    tmpDoc.LoadXml("<Resources></Resources>");

                    System.Xml.XmlNode rootNode = tmpDoc.FirstChild;

                    string resxName = fileNamespace + "." + Path.GetFileName(filePath);

                    using (ResXResourceReader reader = new ResXResourceReader(filePath))
                    {
                        foreach (DictionaryEntry entry in reader)
                        {
                            System.Xml.XmlElement newElement = tmpDoc.CreateElement("ResourceEntry");
                            newElement.SetAttribute("ResxName", resxName);
                            newElement.SetAttribute("AssemblyName", assemblyName);
                            newElement.SetAttribute("Key", entry.Key.ToString());
                            newElement.SetAttribute("Text", entry.Value.ToString());

                            rootNode.AppendChild(newElement);
                        }
                    }

                    tmpDoc.Save(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), tmpFileName));
                }
            }
        }

        public static void UpdateResources(string resourceType, string fromSource)
        {
            LoadMaster();

            if (resourceType == "resx")
            {
                if (fromSource == "resxextract")
                {
                    updateDateTime = DateTime.Now;

                    string[] files = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "*.resxextract", SearchOption.TopDirectoryOnly);

                    Console.WriteLine(files.Length.ToString() + " Files Found");

                    List<string> updatedResourceContainers = new List<string>();

                    foreach (string file in files)
                    {

                        XmlDocument resourcesDoc = new XmlDocument();
                        resourcesDoc.Load(file);

                        foreach (System.Xml.XmlNode node in resourcesDoc.GetElementsByTagName("ResourceEntry"))
                        {
                            string resxName = node.Attributes["ResxName"].Value;
                            string assemblyName = node.Attributes["AssemblyName"].Value;
                            string resourceKey = node.Attributes["Key"].Value;
                            string text = node.Attributes["Text"].Value;

                            string resourceContainerKey = resxName + "," + assemblyName;

                            string moduleName = GetModuleFromAssemblyName(assemblyName);

                            UpdateMaster("resx", moduleName, resourceContainerKey, resourceKey, text, new Dictionary<string, string>());

                            if (!updatedResourceContainers.Contains(resourceContainerKey))
                            {
                                updatedResourceContainers.Add(resourceContainerKey);
                            }
                        }

                        File.Delete(file);
                    }

                    MarkDeletedResources("resx", updatedResourceContainers);

                }
                else if (fromSource == "oldxls")
                {
                    updateDateTime = DateTime.Now;

                    Application app = new Application();
                    try
                    {
                        Workbook workbook = null;

                        if (File.Exists(_xlsFileName))
                        {
                            Console.WriteLine("Transfering data from XLS to XML");
                            workbook = app.Workbooks.Open(_xlsFileName, 0, false, Missing.Value, Missing.Value, Missing.Value, false, XlPlatform.xlWindows, Missing.Value, true, false, 0, true, false, false);


                            try
                            {
                                Dictionary<int, string> Languages = new Dictionary<int, string>();

                                Worksheet sheet = (Worksheet)workbook.Worksheets.get_Item(1);

                                object[,] data = (object[,])sheet.UsedRange.get_Value(Missing.Value);

                                if (data.GetLength(0) > 1)
                                {
                                    for (int i = 5; i <= data.GetLength(1); i++)
                                    {
                                        if (data[1, i] != null && !string.IsNullOrEmpty(data[1, i].ToString()))
                                        {
                                            Languages.Add(i, data[1, i].ToString());
                                        }
                                    }

                                    for (int i = 2; i <= data.GetLength(0); i++)
                                    {
                                        if ((data[i, 1] != null) && !data[i, 1].ToString().StartsWith("#") && data[i, 4] != null && !string.IsNullOrEmpty(data[i, 4].ToString().Trim()))
                                        {
                                            string ResxKey = data[i, 1].ToString();
                                            string key = data[i, 2].ToString();
                                            DateTime Updated = DateTime.Now;
                                            DateTime.TryParse(data[i, 3].ToString(), out Updated);
                                            string text = data[i, 4].ToString();

                                            string assemblyName = ResxKey.Substring(ResxKey.IndexOf(","));

                                            string moduleName = GetModuleFromAssemblyName(assemblyName);

                                            Dictionary<string, string> translations = new Dictionary<string, string>();

                                            foreach (KeyValuePair<int, string> language in Languages)
                                            {
                                                translations.Add(language.Value, data[i, language.Key] == null ? "" : data[i, language.Key].ToString());
                                            }

                                            UpdateMaster("resx", moduleName, ResxKey, key, text, translations);

                                        }
                                    }
                                }
                            }
                            finally
                            {
                                workbook.Close(false, Missing.Value, Missing.Value);
                            }
                        }
                    }
                    finally
                    {

                        app.Quit();
                    }
                }
            }

            _masterDoc.Save(_xmlFileName);
        }

        public static void CreateOutput(string resourceType, string outputPath)
        {
            Console.WriteLine("Output start");
            LoadMaster();

            if (resourceType == "resx")
            {
                if (_resourceDictionary.ContainsKey(resourceType))
                {
                    Dictionary<string, ResXResourceWriter> writerDictionary = new Dictionary<string, ResXResourceWriter>();
                    Dictionary<string, List<string>> asmDictionary = new Dictionary<string, List<string>>();

                    List<string> supportedCultures = GetSupportedTranslationsCultureNames(resourceType);

                    Console.WriteLine("Creating folder structure");

                    bool done = false;
                    //Create folder structure
                    foreach (string cultureName in supportedCultures)
                    {
                        string path = Path.Combine(outputPath, cultureName);

                        if (Directory.Exists(path))
                        {
                            done = false;
                            while (!done)
                            {
                                try
                                {
                                    done = true;
                                    Directory.Delete(path, true);
                                }
                                catch
                                {
                                    done = false;
                                    Thread.Sleep(20);
                                }
                            }
                        }


                        done = false;
                        while (!done)
                        {
                            try
                            {
                                done = true;
                                Directory.CreateDirectory(Path.Combine(path, "resx"));
                            }
                            catch
                            {
                                done = false;
                                Thread.Sleep(20);

                            }
                        }

                        Thread.Sleep(20);
                    }

                    Console.WriteLine("Extracting translations");

                    foreach (string moduleName in _resourceDictionary[resourceType].Value.Keys)
                    {

                        if (!Convert.ToBoolean(_resourceDictionary[resourceType].Value[moduleName].Key.Attributes["Translate"].Value))
                        {
                            continue;
                        }


                        foreach (string resourceContainerKey in _resourceDictionary[resourceType].Value[moduleName].Value.Keys)
                        {
                            foreach (string resourceKey in _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value.Keys)
                            {
                                KeyValuePair<XmlNode, Dictionary<string, XmlNode>> resource = _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourceKey];

                                if (!Convert.ToBoolean(resource.Key.Attributes["Translate"].Value))
                                {
                                    continue;
                                }

                                foreach (string cultureName in supportedCultures)
                                {
                                    if (resource.Value.ContainsKey(cultureName))
                                    {
                                        if (!string.IsNullOrEmpty(resource.Value[cultureName].InnerText.Trim()))
                                        {
                                            string cultureOutputPath = Path.Combine(outputPath, cultureName);
                                            string[] resourceContainerKeyParts = resourceContainerKey.Split(',');

                                            string assemblyName = Path.Combine(cultureOutputPath, resourceContainerKeyParts[1]);
                                            assemblyName = Path.ChangeExtension(assemblyName, "resources.dll");

                                            string resxFileName = string.Format("{0}.{1}.resx", Path.Combine(cultureOutputPath, "resx", Path.GetFileNameWithoutExtension(resourceContainerKeyParts[0])), cultureName);

                                            if (!asmDictionary.ContainsKey(assemblyName))
                                            {
                                                asmDictionary[assemblyName] = new List<string>();
                                            }

                                            string writerKey = string.Format("{0};{1}", resourceContainerKey, cultureName);

                                            if (!writerDictionary.ContainsKey(writerKey))
                                            {
                                                asmDictionary[assemblyName].Add(resxFileName);

                                                writerDictionary.Add(writerKey, new ResXResourceWriter(resxFileName));
                                            }

                                            writerDictionary[writerKey].AddResource(resourceKey, resource.Value[cultureName].InnerText);
                                        }
                                    }
                                }

                            }
                        }
                    }

                    Console.WriteLine("Writing resx to disk");
                    //Write to disk
                    foreach (ResXResourceWriter writer in writerDictionary.Values)
                    {
                        try
                        {
                            writer.Generate();
                        }
                        finally
                        {
                            writer.Close();
                        }


                    }

                    Console.WriteLine("Compiling resx");
                    verbose = true;
                    Compile(asmDictionary);


                    Console.WriteLine("Cleaning up resx");
                    foreach (string cultureName in supportedCultures)
                    {
                        string path = Path.Combine(outputPath, cultureName);
                        path = Path.Combine(path, "resx");

                        if (Directory.Exists(path))
                        {
                            done = false;
                            while (!done)
                            {
                                try
                                {
                                    done = true;
                                    Directory.Delete(path, true);
                                }
                                catch
                                {
                                    done = false;
                                    Thread.Sleep(20);
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Output done");
        }

        public static void ConfirmChangesFromGUI(string resourceType, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, XmlNode>>>>>>>>> GUIMasterData, bool doDelete)
        {
            LoadMaster();

            if (GUIMasterData.ContainsKey(resourceType))
            {
                foreach (string moduleName in GUIMasterData[resourceType].Value.Keys)
                {
                    foreach (string resourceContainerKey in GUIMasterData[resourceType].Value[moduleName].Value.Keys)
                    {
                        //Check and update Translate attribute
                        //---------------------------------------------------------------------------------------------------------------
                        if (_resourceDictionary[resourceType].Value[moduleName].Key.Attributes["Translate"].Value != GUIMasterData[resourceType].Value[moduleName].Key.Attributes["Translate"].Value)
                        {
                            _resourceDictionary[resourceType].Value[moduleName].Key.Attributes["Translate"].Value = GUIMasterData[resourceType].Value[moduleName].Key.Attributes["Translate"].Value;
                        }
                        //---------------------------------------------------------------------------------------------------------------

                        foreach (string resourceKey in GUIMasterData[resourceType].Value[moduleName].Value[resourceContainerKey].Value.Keys)
                        {
                            //Check and update Translate attribute
                            //---------------------------------------------------------------------------------------------------------------
                            if (_resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourceKey].Key.Attributes["Translate"].Value != GUIMasterData[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourceKey].Key.Attributes["Translate"].Value)
                            {
                                _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourceKey].Key.Attributes["Translate"].Value = GUIMasterData[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourceKey].Key.Attributes["Translate"].Value;
                            }
                            //---------------------------------------------------------------------------------------------------------------

                            KeyValuePair<XmlNode, Dictionary<string, XmlNode>> resource = GUIMasterData[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourceKey];

                            if (!string.IsNullOrEmpty(resource.Key.Attributes["Change"].Value))
                            {
                                if (resource.Key.Attributes["Change"].Value.Contains("Deleted"))
                                {
                                    if (_resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value.ContainsKey(resourceKey))
                                    {
                                        if (resource.Key.Attributes["Change"].Value.Contains("_(Undo)") || !doDelete)
                                        {
                                            _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourceKey].Key.Attributes["Change"].Value = string.Empty;
                                        }
                                        else
                                        {

                                            XmlNode resourceToDelete = _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourceKey].Key;
                                            _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Key.RemoveChild(resourceToDelete);
                                            _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value.Remove(resourceToDelete.Attributes["Key"].Value);

                                        }
                                    }
                                }
                                else
                                {
                                    if (resource.Key.Attributes["Change"].Value.Contains("_(Updated)"))
                                    {
                                        Dictionary<string, string> translations = new Dictionary<string, string>();

                                        foreach (KeyValuePair<string, XmlNode> translation in resource.Value)
                                        {
                                            if (translation.Key != GetDefaultCultureName(resourceType))
                                            {
                                                translations.Add(translation.Key, translation.Value.InnerText);
                                            }
                                        }

                                        UpdateMaster(resourceType, moduleName, resourceContainerKey, resourceKey, string.Empty, translations);
                                    }
                                }
                            }
                        }
                    }
                }

                _masterDoc.Save(_xmlFileName);
            }
        }


        public static void Compile(Dictionary<string, List<string>> asmDictionary)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            info.RedirectStandardError = true;

            foreach (KeyValuePair<string, List<string>> item in asmDictionary)
            {
                string culture = null;
                string resources = null;

                foreach (string file in item.Value)
                {
                    string name = Path.GetFileNameWithoutExtension(file);
                    culture = name.Split('.').Last();

                    string resname = Path.ChangeExtension(file, "resources");

                    if (verbose)
                    {
                        Console.WriteLine(string.Format("Compiling resource file {0}...", Path.GetFileName(resname)));
                    }

                    string arg = string.Format("{0} {1}", file, resname);

                    info.FileName = "resgen";
                    info.Arguments = arg;
                    info.RedirectStandardError = true;

                    Process p = Process.Start(info);
                    p.WaitForExit();

                    if (p.ExitCode != 0)
                    {
                        throw new Exception(string.Format("Error executing command \"{0} {1}\".\n{2}", info.FileName, arg, p.StandardError.ReadToEnd()));
                    }

                    resources += " /embed:" + resname;
                }

                if (verbose)
                {
                    Console.WriteLine(string.Format("Linking assembly {0}...", Path.GetFileName(item.Key)));
                }

                string arg2 = string.Format("/t:lib /c:{0} /out:{1}{2}", culture, item.Key, resources);

                info.FileName = "al";
                info.Arguments = arg2;
                info.RedirectStandardError = true;

                Process p2 = Process.Start(info);
                p2.WaitForExit();

                if (p2.ExitCode != 0)
                {
                    throw new Exception(string.Format("Error executing command \"{0} {1}\".\n{2}", info.FileName, arg2, p2.StandardError.ReadToEnd()));
                }
            }
        }


        private static string GetModuleFromAssemblyName(string assemblyName)
        {
            string moduleName = "Other";

            if (assemblyName.Contains(".Abc."))
            {
                moduleName = "Abc";
            }
            else if (assemblyName.Contains(".Dock."))
            {
                moduleName = "Dock";
            }
            else if (assemblyName.Contains(".Gateway."))
            {
                moduleName = "Gateway";
            }
            else if (assemblyName.Contains(".GatewayMHS."))
            {
                moduleName = "GatewayMHS";
            }
            else if (assemblyName.Contains(".GatewayTMS."))
            {
                moduleName = "GatewayTMS";
            }
            else if (assemblyName.Contains(".OrderManagement."))
            {
                moduleName = "OrderManagement";
            }
            else if (assemblyName.Contains(".ProofOfDelivery."))
            {
                moduleName = "ProofOfDelivery";
            }
            else if (assemblyName.Contains(".Transportation."))
            {
                moduleName = "Transportation";
            }
            else if (assemblyName.Contains(".Warehouse."))
            {
                moduleName = "Warehouse";
            }
            else if (assemblyName.Contains(".XAM."))
            {
                moduleName = "XAM";
            }
            else if (assemblyName.Contains(".ActivityMonitor."))
            {
                moduleName = "ActivityMonitor";
            }
            else if (assemblyName.Contains(".OutputManager."))
            {
                moduleName = "OutputManager";
            }

            return moduleName;
        }

        private static void UpdateMaster(string resourceType, string moduleName, string resourceContainerKey, string resourcekey, string text, Dictionary<string, string> translations)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (!_resourceDictionary[resourceType].Value.ContainsKey(moduleName))
                {
                    XmlElement newModule = _masterDoc.CreateElement("Module");
                    newModule.SetAttribute("Name", moduleName);
                    newModule.SetAttribute("Translate", "True");
                    _resourceDictionary[resourceType].Key.AppendChild(newModule);
                    _resourceDictionary[resourceType].Value.Add(moduleName, new KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, XmlNode>>>>>>(newModule, new Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, XmlNode>>>>>()));
                }


                if (!_resourceDictionary[resourceType].Value[moduleName].Value.ContainsKey(resourceContainerKey))
                {
                    XmlElement newNode = _masterDoc.CreateElement("ResourceDictionary");
                    newNode.SetAttribute("Key", resourceContainerKey);
                    _resourceDictionary[resourceType].Value[moduleName].Key.AppendChild(newNode);
                    _resourceDictionary[resourceType].Value[moduleName].Value.Add(resourceContainerKey, new KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, XmlNode>>>>(newNode, new Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, XmlNode>>>()));
                }

                if (!_resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value.ContainsKey(resourcekey))
                {
                    XmlElement newNode = _masterDoc.CreateElement("Resource");
                    newNode.SetAttribute("Key", resourcekey);
                    newNode.SetAttribute("Updated", updateDateTime.ToString());
                    newNode.SetAttribute("Change", "New");
                    newNode.SetAttribute("Translate", "true");
                    _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Key.AppendChild(newNode);
                    _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value.Add(resourcekey, new KeyValuePair<XmlNode, Dictionary<string, XmlNode>>(newNode, new Dictionary<string, XmlNode>()));

                    XmlElement newTranslation = _masterDoc.CreateElement("Text");
                    newTranslation.SetAttribute("CultureName", GetDefaultCultureName(resourceType));
                    newTranslation.InnerText = text;

                    newNode.AppendChild(newTranslation);
                    _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Value.Add(GetDefaultCultureName(resourceType), newTranslation);
                }
                else
                {
                    _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Key.Attributes["Updated"].Value = updateDateTime.ToString();

                    string existingText = _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Value[GetDefaultCultureName(resourceType)].InnerText;

                    if (existingText != text)
                    {
                        _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Value[GetDefaultCultureName(resourceType)].InnerText = text;
                        _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Key.Attributes["Change"].Value = "Changed";
                    }
                }
            }
            else
            {
                if (_resourceDictionary.ContainsKey(resourceType))
                {
                    if (_resourceDictionary[resourceType].Value.ContainsKey(moduleName))
                    {
                        if (_resourceDictionary[resourceType].Value[moduleName].Value.ContainsKey(resourceContainerKey))
                        {
                            if (_resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value.ContainsKey(resourcekey))
                            {
                                _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Key.Attributes["Change"].Value = string.Empty;
                            }
                        }
                    }
                }
            }



            if (translations != null)
            {
                if (_resourceDictionary.ContainsKey(resourceType))
                {
                    if (_resourceDictionary[resourceType].Value.ContainsKey(moduleName))
                    {
                        if (_resourceDictionary[resourceType].Value[moduleName].Value.ContainsKey(resourceContainerKey))
                        {
                            if (_resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value.ContainsKey(resourcekey))
                            {
                                //Update and Add translations
                                foreach (string cultureName in GetSupportedTranslationsCultureNames(resourceType))
                                {
                                    if (translations.ContainsKey(cultureName))
                                    {
                                        if (_resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Value.ContainsKey(cultureName))
                                        {
                                            string currentText = _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Value[cultureName].InnerText;

                                            if (currentText != translations[cultureName])
                                            {
                                                _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Value[cultureName].InnerText = translations[cultureName];
                                                _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Key.Attributes["Updated"].Value = updateDateTime.ToString();
                                            }
                                        }
                                        else
                                        {
                                            XmlElement newTranslation = _masterDoc.CreateElement("Text");
                                            newTranslation.SetAttribute("CultureName", cultureName);
                                            newTranslation.InnerText = translations[cultureName];

                                            _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Key.AppendChild(newTranslation);
                                            _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Value.Add(cultureName, newTranslation);
                                        }
                                    }
                                    else
                                    {
                                        if (!_resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Value.ContainsKey(cultureName))
                                        {
                                            XmlElement newTranslation = _masterDoc.CreateElement("Text");
                                            newTranslation.SetAttribute("CultureName", cultureName);
                                            newTranslation.InnerText = string.Empty;

                                            _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Key.AppendChild(newTranslation);
                                            _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Value.Add(cultureName, newTranslation);
                                        }
                                    }
                                }
                                //Remove unsupported translations
                                List<string> supportedCultures = GetSupportedTranslationsCultureNames(resourceType);
                                string defaultCultureName = GetDefaultCultureName(resourceType);
                                int index = 0;
                                while (index < _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Value.Count)
                                {
                                    KeyValuePair<string, XmlNode> cultureInFile = _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Value.ElementAt(index);
                                    if (cultureInFile.Key != defaultCultureName)
                                    {
                                        if (!supportedCultures.Contains(cultureInFile.Key))
                                        {
                                            _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Key.RemoveChild(cultureInFile.Value);
                                            _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Value.Remove(cultureInFile.Key);
                                            continue;
                                        }
                                    }
                                    index++;
                                }
                            }
                        }
                    }
                }
            }

        }

        private static void MarkDeletedResources(string resourceType, List<string> updatedResourceContainers)
        {
            foreach (string moduleName in _resourceDictionary[resourceType].Value.Keys)
            {
                foreach (string resourceContainerKey in _resourceDictionary[resourceType].Value[moduleName].Value.Keys)
                {
                    if (updatedResourceContainers.Contains(resourceContainerKey))
                    {
                        foreach (KeyValuePair<XmlNode, Dictionary<string, XmlNode>> resource in _resourceDictionary[resourceType].Value[moduleName].Value[resourceContainerKey].Value.Values)
                        {
                            if (Convert.ToDateTime(resource.Key.Attributes["Updated"].Value).ToString() != updateDateTime.ToString())
                            {
                                resource.Key.Attributes["Change"].Value = "Deleted";
                            }
                            else if (resource.Key.Attributes["Change"].Value == "Deleted")
                            {
                                resource.Key.Attributes["Change"].Value = string.Empty;
                            }
                        }
                    }
                }
            }
        }


    }
}

