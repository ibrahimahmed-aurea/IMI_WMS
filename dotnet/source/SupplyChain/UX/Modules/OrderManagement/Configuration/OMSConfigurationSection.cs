using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Xml;
using Microsoft.Practices.CompositeUI;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Microsoft.Practices.ObjectBuilder;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Configuration
{
    public class OMSConfigurationSection : ConfigurationSection
    {
        public const string SectionKey = "imi.supplychain.ux.modules.ordermanagement";

        public int HostPort { get; set; }

        public OMSConfigurationSection()
        {
        }

        public string getConfigValue(string key)
        {
            object obj = this[key];
            if (obj == null)
                return null;
            else
                return obj.ToString();
        }

        [ConfigurationProperty("hostName", IsRequired = true)]
        public string HostName
        {
            get
            { return (string)this["hostName"]; }
            set
            { this["hostName"] = value; }
        }

        [ConfigurationProperty("sendDomainUserId", IsRequired = true)]
        public bool SendDomainUserId
        {
            get
            { return bool.Parse(this["sendDomainUserId"].ToString()); }
            set
            { this["sendDomainUserId"] = value; }
        }

        [ConfigurationProperty("sendSecurityToken", IsRequired = true)]
        public bool SendSecurityToken
        {
            get
            { return bool.Parse(this["sendSecurityToken"].ToString()); }
            set
            { this["sendSecurityToken"] = value; }
        }

        [ConfigurationProperty("kickstartURL", IsRequired = false)]
        public string KickstartURL
        {
            get
            { return (string)(this["kickstartURL"]); }
            set
            { this["kickstartURL"] = value; }
        }

        [ConfigurationProperty("AA01_URL", IsRequired = false)]
        public string AA01_URL
        {
            get
            { return (string)(this["AA01_URL"]); }
            set
            { this["AA01_URL"] = value; }
        }

        [ConfigurationProperty("AX01_URL", IsRequired = false)]
        public string AX01_URL
        {
            get
            { return (string)(this["AX01_URL"]); }
            set
            { this["AX01_URL"] = value; }
        }

        [ConfigurationProperty("AX02_URL", IsRequired = false)]
        public string AX02_URL
        {
            get
            { return (string)(this["AX02_URL"]); }
            set
            { this["AX02_URL"] = value; }
        }

        [ConfigurationProperty("AX03_URL", IsRequired = false)]
        public string AX03_URL
        {
            get
            { return (string)(this["AX03_URL"]); }
            set
            { this["AX03_URL"] = value; }
        }

        [ConfigurationProperty("AX04_URL", IsRequired = false)]
        public string AX04_URL
        {
            get
            { return (string)(this["AX04_URL"]); }
            set
            { this["AX04_URL"] = value; }
        }

        [ConfigurationProperty("AX05_URL", IsRequired = false)]
        public string AX05_URL
        {
            get
            { return (string)(this["AX05_URL"]); }
            set
            { this["AX05_URL"] = value; }
        }

        [ConfigurationProperty("AX06_URL", IsRequired = false)]
        public string AX06_URL
        {
            get
            { return (string)(this["AX06_URL"]); }
            set
            { this["AX06_URL"] = value; }
        }

        [ConfigurationProperty("AX07_URL", IsRequired = false)]
        public string AX07_URL
        {
            get
            { return (string)(this["AX07_URL"]); }
            set
            { this["AX07_URL"] = value; }
        }

        [ConfigurationProperty("AX08_URL", IsRequired = false)]
        public string AX08_URL
        {
            get
            { return (string)(this["AX08_URL"]); }
            set
            { this["AX08_URL"] = value; }
        }

        [ConfigurationProperty("AX09_URL", IsRequired = false)]
        public string AX09_URL
        {
            get
            { return (string)(this["AX09_URL"]); }
            set
            { this["AX09_URL"] = value; }
        }

        [ConfigurationProperty("aomHostPorts")]
        [ConfigurationCollection(typeof(AomHostPortCollection), AddItemName = "aomHostPort")]
        public AomHostPortCollection AomHostPortElementCollection
        {
            get
            {
                return (AomHostPortCollection)this["aomHostPorts"];
            }
            set
            {
                this["aomHostPorts"] = value;
            }
        }
    }

    public class AomHostPortCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new HostPortElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HostPortElement)element).Language;
        }
    }

    public class HostPortElement : ConfigurationElement
    {
        [ConfigurationProperty("language", IsRequired = true)]
        public String Language
        {
            get
            {
                return (String)this["language"];
            }
            set
            {
                this["language"] = value;
            }
        }

        [ConfigurationProperty("number", IsRequired = true)]
        public string Number
        {
            get
            {
                return (string)this["number"];
            }
            set
            {
                this["number"] = value;
            }
        }
    }
}

