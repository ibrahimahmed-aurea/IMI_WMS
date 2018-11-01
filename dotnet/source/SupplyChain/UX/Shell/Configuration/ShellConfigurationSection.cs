using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Xml;
using Microsoft.Practices.CompositeUI.Configuration;

namespace Imi.SupplyChain.UX.Shell.Configuration
{
    public class ShellConfigurationSection : ConfigurationSection
    {
        public const string SectionKey = "imi.supplychain.ux.shell";

        public ShellConfigurationSection()
        {
        }

        [ConfigurationProperty("helpBaseUri", IsRequired = false)]
        public string HelpBaseUri
        {
            get
            { return (string)this["helpBaseUri"]; }
            set
            { this["helpBaseUri"] = value; }
        }

        [ConfigurationProperty("hostName", IsRequired = true)]
        public string HostName
        {
            get
            { return (string)this["hostName"]; }
            set
            { this["hostName"] = value; }
        }

        [ConfigurationProperty("hostPort", IsRequired = true)]
        public int HostPort
        {
            get
            { return int.Parse(this["hostPort"].ToString()); }
            set
            { this["hostPort"] = value; }
        }

        [ConfigurationProperty("instanceNameLocalInstall", IsRequired = true)]
        public string InstanceNameLocalInstall
        {
            get
            { return this["instanceNameLocalInstall"].ToString(); }
            set
            { this["instanceNameLocalInstall"] = value; }
        }

        [ConfigurationProperty("themeName", IsRequired = true)]
        public string ThemeName
        {
            get
            { return (string)this["themeName"]; }
            set
            { this["themeName"] = value; }
        }

        [ConfigurationProperty("themeTintColor", IsRequired = true)]
        public string ThemeTintColor
        {
            get
            { return (string)this["themeTintColor"]; }
            set
            { this["themeTintColor"] = value; }
        }

        [ConfigurationProperty("isGlassEnabled", IsRequired = true)]
        public bool IsGlassEnabled
        {
            get
            { return (bool)this["isGlassEnabled"]; }
            set
            { this["isGlassEnabled"] = value; }
        }

        [ConfigurationProperty("HttpListenerPort", IsRequired = false)]
        public string HttpListenerPort
        {
            get
            { return (string)this["HttpListenerPort"]; }
            set
            { this["HttpListenerPort"] = value; }
        }

        [ConfigurationProperty("hideUnauthorizedMenuItems", IsRequired = false)]
        public bool HideUnauthorizedMenuItems
        {
            get
            { return bool.Parse(this["hideUnauthorizedMenuItems"].ToString()); }
            set
            { this["hideUnauthorizedMenuItems"] = value; }
        }

        [ConfigurationProperty("languages")]
        [ConfigurationCollection(typeof(LanguageCollection),
                AddItemName = "language")]
        public LanguageCollection LanguageElementCollection
        {
            get
            {
                return (LanguageCollection)this["languages"];
            }
            set
            {
                this["languages"] = value;
            }
        }

        [ConfigurationProperty("services")]
        [ConfigurationCollection(typeof(ServiceElementCollection),
                AddItemName = "add")]
        public ServiceElementCollection ServiceElementCollection
        {
            get
            {
                return (ServiceElementCollection)this["services"];
            }
            set
            {
                this["services"] = value;
            }
        }
    }

    public class LanguageCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LanguageElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LanguageElement)element).Name;
        }
    }

    public class LanguageElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public String Name
        {
            get
            {
                return (String)this["name"];
            }
            set
            {
                this["id"] = value;
            }
        }

        [ConfigurationProperty("culture", IsRequired = true)]
        public string Culture
        {
            get
            {
                return (string)this["culture"];
            }
            set
            {
                this["culture"] = value;
            }
        }
    }

}
