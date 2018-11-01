using System;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser.GUI
{
	public class ConfigurationSettings
	{
        private Configuration config = null;

        public ConfigurationSettings()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();

            fileMap.ExeConfigFilename = "CodeSmithTemplateParserGUI.config";

            config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        }

        public void Save()
        {
            config.Save(ConfigurationSaveMode.Full);
        }

        public string this[string key]
        {
            get
            {
                return GetValue(key);
            }
            set
            {
                SetValue(key, value);
            }
        }

        private void SetValue(string key, string value)
        {
            if (config.AppSettings.Settings[key] != null)
            {
                config.AppSettings.Settings[key].Value = value;
            }
            else
            {
                config.AppSettings.Settings.Add(key, value);
            }
        }

        private string GetValue(string key)
        {
            KeyValueConfigurationElement elem = config.AppSettings.Settings[key];

            return elem == null ? string.Empty : elem.Value;
        }

        private List<string> GetValues(string partKey)
        {
            List<string> newList = new List<string>();

            string noOfStrings = GetValue(string.Format("{0}_Count", partKey));

            if (!string.IsNullOrEmpty(noOfStrings))
            {
                int noStrings = int.Parse(noOfStrings);

                for (int i = 0; i < noStrings; i++)
                {
                    newList.Add(GetValue(string.Format("{0}{1}", partKey, i.ToString("0000"))));
                }
            }

            return newList;
        }

        private void SetValues(string partKey, List<string> values)
        {
            List<string> newList = new List<string>();

            string[] allKeys = config.AppSettings.Settings.AllKeys;

            foreach (string key in allKeys)
            {
                if (key == string.Format("{0}_Count", partKey))
                {
                    config.AppSettings.Settings.Remove(key);
                }
                else if (key.StartsWith(partKey))
                {
                    string test = key.Remove(0, partKey.Length);
                    int convTest;

                    if (int.TryParse(test, out convTest))
                    {
                        config.AppSettings.Settings.Remove(key);
                    }
                }
            }

            // Set the count
            SetValue(string.Format("{0}_Count", partKey), values.Count.ToString());

            // Set all values
            for (int i = 0; i < values.Count; i++ )
            {
                SetValue(string.Format("{0}{1}", partKey, i.ToString("0000")), values[i]);
            }
        }

        public List<string> NotCompileList 
        {
            get
            {
                return GetValues("NotCompileList");
            }
            set
            {
                SetValues("NotCompileList", value);
            }
        }

        public List<string> ReferenceList
        {
            get
            {
                return GetValues("ReferenceList");
            }
            set
            {
                SetValues("ReferenceList", value);
            }
        }

        public string AssemblyName
        {
            get
            {
                return GetValue("AssemblyName");
            }
            set
            {
                SetValue("AssemblyName", value);
            }
        }

        public bool DebugInfo
        {
            get
            {
                bool result = false;

                bool.TryParse(GetValue("DebugInfo"), out result);

                return result;
            }
            set
            {
                SetValue("DebugInfo", value.ToString());
            }
        }

        public string SourceDirectory
        {
            get
            {
                return GetValue("SourceDirectory");
            }
            set
            {
                SetValue("SourceDirectory", value);
            }
        }

        public string DestinationDirectory
        {
            get
            {
                return GetValue("DestinationDirectory");
            }
            set
            {
                SetValue("DestinationDirectory", value);
            }
        }

        public string SingleFileParse
        {
            get
            {
                return GetValue("SingleFileParse");
            }
            set
            {
                SetValue("SingleFileParse", value);
            }
        }

    }
}
