using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Imi.Wms.WebServices.OutboundTesterMAPI
{

    public class WebServiceSection : ConfigurationSection
    {
        public const string SectionKey = "imi.wms.webservice";

        public WebServiceSection()
        {
        }

        [ConfigurationProperty("PartnerList")]
        [ConfigurationCollection(typeof(PartnerCollection),
                AddItemName = "Partner")]
        public PartnerCollection PartnerList
        {
            get
            { return (PartnerCollection)this["PartnerList"]; }
            set
            { this["PartnerList"] = value; }
        }

    }


    public class PartnerCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PartnerElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PartnerElement)element).Name;
        }
    }

    public class PartnerElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public String Name
        {
            get
            { return (String)this["name"]; }
            set
            { this["name"] = value; }
        }

        [ConfigurationProperty("database", IsRequired = true)]
        public String Database
        {
            get
            { return (String)this["database"]; }
            set
            { this["database"] = value; }
        }
    }


}