using System;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Cdc.MetaManager.GUI
{
    public static class Config
    {
        private static ConfigurationSettings current = null;

        private static ConfigurationSettings Current 
        {
            get
            {
                if (current == null)
                    current = new ConfigurationSettings();

                return current;
            }
            set
            {
                current = value;
            }
        }

        public static ConfigurationBackendSettings Backend
        {
            get
            {
                return Current.Backend;
            }
        }

        public static ConfigurationFrontendSettings Frontend
        {
            get
            {
                return Current.Frontend;
            }
        }

        public static ConfigurationApplicationSettings Global
        {
            get
            {
                return Current.Global;
            }
        }

        public static void SetPrefixes(string backendPrefix, string frontendPrefix)
        {
            Current.SetBackendFrontendPrefix(backendPrefix, frontendPrefix);
        }

        public static bool Save()
        {
            if (Current != null)
                return Current.Save();

            return false;
        }
    }

    public class ConfigurationSettings
	{
        private Configuration config = null;
        private ConfigurationFrontendSettings configFrontendSettings = null;
        private ConfigurationBackendSettings configBackendSettings = null;

        /// <summary>
        ///     Constructor for creating configuration settings that can fetch data from the
        ///     configuration file.
        ///     Use the "FrontendApplicationConfigPrefix" and "BackendApplicationConfigPrefix" to
        ///     set the prefix (defined in MdiChildForm).
        /// </summary>
        /// <param name="prefix">The prefix of the configuration parameters to fetch.</param>
        public ConfigurationSettings()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();

            fileMap.ExeConfigFilename = ConfigFilename; //"MetaManagerGUI.config";

            config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            Global = new ConfigurationApplicationSettings(config);
        }

        public void SetBackendFrontendPrefix(string backendPrefix, string frontendPrefix)
        {
            configFrontendSettings = new ConfigurationFrontendSettings(config, frontendPrefix);
            configBackendSettings = new ConfigurationBackendSettings(config, backendPrefix);
        }

        public string ConfigFilename 
        { 
            get
            {
                try
                {
                    string configFilename = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), ConfigurationManager.AppSettings["UserSettingsConfigFile"]));
                    return configFilename;
                }
                catch (Exception ex)
                {
                    throw new Exception("Please check that your App.Config file contains \"MetaManagerGUIConfigFile\" in the \"appSettings\" section and that it points to the correct file (can be a relative path to it)!", ex);
                }
            }
        }

        /// <summary>
        ///     Call this when you want to persist changes you've made to the configuration to disc.
        /// </summary>
        public bool Save()
        {
            try
            {
                config.Save(ConfigurationSaveMode.Full);
                return true;
            }
            catch 
            {
                // ConfigurationException may be happening when someone has saved the file so it detects that
                // it's not the same file anymore.
                return false;
            }
        }

        /// <summary>
        ///     All frontend specific configuration settings.
        /// </summary>
        public ConfigurationFrontendSettings Frontend 
        {
            get
            {
                return configFrontendSettings;
            }
        }

        /// <summary>
        ///     All backend specific configuration settings.
        /// </summary>
        public ConfigurationBackendSettings Backend
        {
            get
            {
                return configBackendSettings;
            }
        }

        /// <summary>
        ///     All global application settings.
        /// </summary>
        public ConfigurationApplicationSettings Global { get; private set; }
    }

    public abstract class ConfigurationMainSettings
    {
        protected Configuration Config { get; set; }

        public ConfigurationMainSettings() {}

        public ConfigurationMainSettings(Configuration config)
        {
            Config = config;
        }

        public string this[string key]
        {
            get
            {
                return GetValue(GetKey(key));
            }
            set
            {
                SetValue(GetKey(key), value);
            }
        }

        /// <summary>
        ///     Override this parameter to be able to change the key to be able to set a prefix
        ///     or suffix or whatever.
        /// </summary>
        /// <param name="key">The original key that is to be fetched.</param>
        /// <returns>The modified key returned to be found in the configuration settings file.</returns>
        protected abstract string GetKey(string key);

        protected virtual void DeleteKey(string key)
        {
            try
            {
                // Delete the key
                Config.AppSettings.Settings.Remove(GetKey(key));
            }
            catch { }
        }

        protected virtual void SetValue(string key, string value)
        {
            string theKey = GetKey(key);

            if (Config.AppSettings.Settings[theKey] != null)
            {
                Config.AppSettings.Settings[theKey].Value = value;
            }
            else
            {
                Config.AppSettings.Settings.Add(theKey, value);
            }
        }

        protected virtual void SetValue(string key, bool value)
        {
            SetValue(key, value.ToString());
        }

        protected virtual void SetValue(string key, double value)
        {
            SetValue(key, value.ToString());
        }

        protected virtual string GetValue(string key)
        {
            return GetValue(key, string.Empty);
        }

        protected virtual string GetValue(string key, string defaultValue)
        {
            KeyValueConfigurationElement elem = Config.AppSettings.Settings[GetKey(key)];

            return elem == null ? defaultValue : elem.Value;
        }

        protected virtual bool GetValueBool(string key)
        {
            return GetValueBool(key, false);
        }

        protected virtual bool GetValueBool(string key, bool defaultValue)
        {
            bool parsedValue;

            if (bool.TryParse(GetValue(key), out parsedValue))
                return parsedValue;
            else
                return defaultValue;
        }

        protected virtual double GetValueDouble(string key)
        {
            return GetValueDouble(key, 0);
        }

        protected virtual double GetValueDouble(string key, double defaultValue)
        {
            double parsedValue;

            if (double.TryParse(GetValue(key), out parsedValue))
                return parsedValue;
            else
                return defaultValue;
        }

    }

    public class ConfigurationApplicationSettings : ConfigurationMainSettings
    {
        public ConfigurationApplicationSettings(Configuration config)
        {
            Config = config;
        }

        protected override string GetKey(string key)
        {
            return string.Format("GlobalSettings{0}", key);
        }

        public string LastSelectedDeploymentGroup
        {
            get
            {
                return GetValue("LastSelectedDeploymentGroup");
            }
            set
            {
                SetValue("LastSelectedDeploymentGroup", value);
            }
        }

        public string LastSelectedImportChangeSourceView
        {
            get
            {
                return GetValue("LastSelectedImportChangeSourceView");
            }
            set
            {
                SetValue("LastSelectedImportChangeSourceView", value);
            }
        }
    }

    public abstract class ConfigurationPrefixSettings : ConfigurationMainSettings
    {
        /// <summary>
        ///     Gets the name of the prefix.
        /// </summary>
        public string Prefix { get; private set; }

        private List<string> configGroups = new List<string>();

        public ConfigurationPrefixSettings(Configuration config, string prefix)
        {
            if (config == null)
                throw new ArgumentException("The config may not be set to null!", "config");

            if (prefix == null)
                throw new ArgumentException("The prefix may not be set to null!", "prefix");

            Config = config;
            Prefix = prefix;
        }

        protected override string GetKey(string key)
        {
            return string.Format("{0}{1}", Prefix, key);
        }

        public List<string> GenerationFilter
        {
            get
            {
                List<string> list = new List<string>();

                string filterString = GetValue("GenerationFilter");

                string[] array = filterString.Split(new char[] { ',' });

                return new List<string>(array);
            }
            set
            {
                string newValue = "";
                foreach (string s in value)
                {
                    if (newValue == "")
                        newValue = s;
                    else
                        newValue += "," + s;
                }
                SetValue("GenerationFilter", newValue);
            }
        }

        public string SolutionName
        {
            get
            {
                return GetValue("SolutionName");
            }
            set
            {
                SetValue("SolutionName", value);
            }
        }

        public string SolutionFolder
        {
            get
            {
                return GetValue("SolutionFolder");
            }
            set
            {
                SetValue("SolutionFolder", value);
            }
        }

        public string ReferenceFolder
        {
            get
            {
                string returnVal = GetValue("ReferenceFolder");

                if (!string.IsNullOrEmpty(returnVal))
                    return returnVal;
                else
                    return ReferenceFolderDefault;
            }
            set
            {
                SetValue("ReferenceFolder", value);
            }
        }

        public string ReferenceFolderDefault
        {
            get
            {
                return Path.GetFullPath(@"..\..\..\..\..\..\..\dotnet\references");
            }
        }
    }

    public class ConfigurationFrontendSettings : ConfigurationPrefixSettings
    {
        public ConfigurationFrontendSettings(Configuration config, string prefix) : base(config, prefix) { }

        /// <summary>
        ///     Saving the last searched Module selected in the "Handle Dialogs" dialog for the
        ///     frontend.
        /// </summary>
        public string HandleDialogsLastSearchedModule
        {
            get
            {
                return GetValue("HandleDialogsLastSearchedModule");
            }
            set
            {
                SetValue("HandleDialogsLastSearchedModule", value);
            }
        }
    }

    public class ConfigurationBackendSettings : ConfigurationPrefixSettings
    {
        public ConfigurationBackendSettings(Configuration config, string prefix) : base(config, prefix) { }

        /// <summary>
        ///     Last selected service in the FindServiceMethod dialog.
        /// </summary>
        public string LastSelectedServiceInFindServiceMethod
        {
            get
            {
                return GetValue("LastSelectedServiceInFindServiceMethod");
            }
            set
            {
                SetValue("LastSelectedServiceInFindServiceMethod", value);
            }
        }

        /// <summary>
        ///     Last Imported Package Spec
        /// </summary>
        public string LastImportPackageSpec
        {
            get
            {
                return GetValue("LastImportPackageSpec");
            }
            set
            {
                SetValue("LastImportPackageSpec", value);
            }
        }

        public string XMLSchemaFolder
        {
            get
            {
                return GetValue("XMLSchemaFolder");
            }
            set
            {
                SetValue("XMLSchemaFolder", value);
            }
        }

        public string PLSQLPackageFolder
        {
            get
            {
                return GetValue("PLSQLPackageFolder");
            }
            set
            {
                SetValue("PLSQLPackageFolder", value);
            }
        }

        public string SaveXMLFileInitialDir
        {
            get
            {
                return GetValue("SaveXMLFileInitialDir");
            }
            set
            {
                SetValue("SaveXMLFileInitialDir", value);
            }
        }

        public string SaveLabelFileInitialDir
        {
            get
            {
                return GetValue("SaveLabelFileInitialDir");
            }
            set
            {
                SetValue("SaveLabelFileInitialDir", value);
            }
        }

        public void DeleteReportTestParameter(string reportName, string parameterName)
        {
            DeleteKey(string.Format("_REP_PAR_{0}_{1}", reportName, parameterName));
        }

        public void DeleteReportTestParameterIsNull(string reportName, string parameterName)
        {
            DeleteKey(string.Format("_REP_PAR_{0}_{1}_ISNULL", reportName, parameterName));
        }

        public string GetReportTestParameter(string reportName, string parameterName)
        {
            return GetValue(string.Format("_REP_PAR_{0}_{1}", reportName, parameterName), string.Empty);
        }

        public bool GetReportTestParameterIsNull(string reportName, string parameterName)
        {
            return GetValueBool(string.Format("_REP_PAR_{0}_{1}_ISNULL", reportName, parameterName), false);
        }

        public void SetReportTestParameter(string reportName, string parameterName, string value)
        {
            SetValue(string.Format("_REP_PAR_{0}_{1}", reportName, parameterName), value);
        }

        public void SetReportTestParameterIsNull(string reportName, string parameterName, bool value)
        {
            SetValue(string.Format("_REP_PAR_{0}_{1}_ISNULL", reportName, parameterName), value);
        }

        public void SetReportRawPrinter(string reportName, string value)
        {
            SetValue(string.Format("_REP_PRT_{0}", reportName), value);
        }

        public string GetReportRawPrinter(string reportName)
        {
            return GetValue(string.Format("_REP_PRT_{0}", reportName), string.Empty);
        }

    }
}
