using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Cdc.UXPublish.Config
{
    public class ProductElement : ConfigurationElement
    {
		private static ConfigurationPropertyCollection s_properties;
		private static ConfigurationProperty s_propName;
        private static ConfigurationProperty s_propCompany;
        private static ConfigurationProperty s_propInstallPath;
        private static ConfigurationProperty s_propVirtualDirectoryRoot;

        static ProductElement()
		{
			s_propName = new ConfigurationProperty(
				"name",
				typeof(string),
				null,
				ConfigurationPropertyOptions.IsRequired
				);

            s_propCompany = new ConfigurationProperty(
                "company",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
                );

			s_propInstallPath = new ConfigurationProperty(
				"installpath",
				typeof(string),
				null,
                ConfigurationPropertyOptions.IsRequired
				);

            s_propVirtualDirectoryRoot = new ConfigurationProperty(
                "virtualdirectoryroot",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
                );

			s_properties = new ConfigurationPropertyCollection();

			s_properties.Add(s_propName);
            s_properties.Add(s_propCompany);
            s_properties.Add(s_propInstallPath);
            s_properties.Add(s_propVirtualDirectoryRoot);
		}

        public string Name
        {
            get { return (string)base[s_propName]; }
            set { base[s_propName] = value; }
        }

        public string Company
        {
            get { return (string)base[s_propCompany]; }
            set { base[s_propCompany] = value; }
        }

        public string InstallPath
        {
            get { return (string)base[s_propInstallPath]; }
            set { base[s_propInstallPath] = value; }
        }

        public string VirtualDirectoryRoot
        {
            get { return (string)base[s_propVirtualDirectoryRoot]; }
            set { base[s_propVirtualDirectoryRoot] = value; }
        }
        
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return s_properties;
            }
        }

    }
}
