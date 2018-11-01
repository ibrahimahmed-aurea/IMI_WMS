using System;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Imi.SupplyChain.Deployment.Wrappers;

namespace Imi.SupplyChain.Deployment
{
    public class ConfigurationSettings
    {
        private Configuration config = null;

        public ConfigurationSettings(string fileName)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();

            fileMap.ExeConfigFilename = fileName;

            config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        }

        public void Save()
        {
            config.Save(ConfigurationSaveMode.Full);

            // Force a reload of the changed section.
            ConfigurationManager.RefreshSection("appSettings");
        }

        protected virtual void SetValue(string key, string value)
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

        public void Set(string key, string value)
        {
            SetValue(key, value);
        }

        public string Get(string key)
        {
            return GetValue(key);
        }

        protected virtual string GetValue(string key)
        {
            KeyValueConfigurationElement elem = config.AppSettings.Settings[key];

            return elem == null ? string.Empty : elem.Value;
        }

        protected virtual bool GetValueBool(string key)
        {
            bool parsedValue;

            if (bool.TryParse(GetValue(key), out parsedValue))
                return parsedValue;
            else
                return false;
        }

        protected virtual IDictionary<string, string> GetSettingsFromPattern(string pattern)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Regex regEx = new Regex(pattern);

            foreach (string key in config.AppSettings.Settings.AllKeys)
            {
                if (regEx.Match(key).Success)
                {
                    // Add match to dictionary
                    dictionary.Add(key, config.AppSettings.Settings[key].Value);
                }
            }

            return dictionary;
        }

        // ---------------------------------------------------------------------------

        public string GetStagingArea()
        {
            return GetValue("StagingArea");
        }

        public void SetStagingArea(string value)
        {
            SetValue("StagingArea", value);
        }

        public string GetWebserverName()
        {
            return GetValue("WebserverName");
        }

        public void SetWebserverName(string value)
        {
            SetValue("WebserverName", value);
        }

        public string GetWebserverPort()
        {
            return GetValue("WebserverPort");
        }

        public void SetWebserverPort(string value)
        {
            SetValue("WebserverPort", value);
        }

        public string GetMainVirtualDirectoryName()
        {
            return GetValue("MainVirtualDirectoryName");
        }

        public void SetMainVirtualDirectoryName(string value)
        {
            SetValue("MainVirtualDirectoryName", value);
        }

        public string GetMainVirtualDirectoryPath()
        {
            return GetValue("MainVirtualDirectoryPath");
        }

        public void SetMainVirtualDirectoryPath(string value)
        {
            SetValue("MainVirtualDirectoryPath", value);
        }

        public string GetInternetGuestAccount()
        {
            return GetValue("InternetGuestAccount");
        }

        public void SetInternetGuestAccount(string value)
        {
            SetValue("InternetGuestAccount", value);
        }

        public string GetLastImportDirectory()
        {
            return GetValue("LastImportDirectory");
        }

        public void SetLastImportDirectory(string value)
        {
            SetValue("LastImportDirectory", value);
        }

        public string GetCertificateFile()
        {
            return GetValue("CertificateFile");
        }

        public void SetCertificateFile(string value)
        {
            SetValue("CertificateFile", value);
        }

        public bool GetAskForCertificatePassword()
        {
            return GetValueBool("AskForCertificatePassword");
        }

        public void SetAskForCertificatePassword(bool value)
        {
            SetValue("AskForCertificatePassword", value.ToString());
        }
    }
}
