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
using Cdc.UXPublish.Config;
using Cdc.UXPublish.Wrappers;

namespace Cdc.UXPublish
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

        public IDictionary<string, string> GetSettingsFromPattern(string pattern)
        {
            Dictionary<string, string> dictionary = new Dictionary<string,string>();
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

        public ProductWrapperList GetProducts()
        {
            ProductSection productSection = config.GetSection("products") as ProductSection;
            ProductWrapperList productList = new ProductWrapperList();

            foreach (ProductElement element in productSection.Products)
            {
                productList.Add(new ProductWrapper(element.Name, element.Company, element.InstallPath, element.VirtualDirectoryRoot));
            }

            return productList;
        }

        public void SaveProduct(string name, string company, string installPath, string virtualDirectoryRoot)
        {
            ProductSection productSection = config.GetSection("products") as ProductSection;

            ProductElement element = new ProductElement();

            element.Name = name;
            element.Company = company;
            element.InstallPath = installPath;
            element.VirtualDirectoryRoot = virtualDirectoryRoot;

            // Add the product
            productSection.Products.Add(element);

            config.Save();
        }

        public void RemoveProduct(string name)
        {
            ProductSection productSection = config.GetSection("products") as ProductSection;
            productSection.Products.Remove(name);
            config.Save();
        }

    }
}
