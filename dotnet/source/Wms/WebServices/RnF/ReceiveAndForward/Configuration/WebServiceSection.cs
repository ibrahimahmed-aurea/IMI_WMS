using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Wms.WebServices.OutboundMapper.Configuration
{

    public class OutboundMapperSection : ConfigurationSection
    {
        public const string SectionKey = "imi.wms.webservice.outboundmapper";

        public OutboundMapperSection()
        {
        }

        [ConfigurationProperty("CommunicationChannelList")]
        [ConfigurationCollection(typeof(CommunicationChannelCollection),
              AddItemName = "CommunicationChannel")]
        public CommunicationChannelCollection CommunicationChannelList
        {
            get
            { return (CommunicationChannelCollection)this["CommunicationChannelList"]; }
            set
            { this["CommunicationChannelList"] = value; }
        }

    }

    public class CommunicationChannelCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CommunicationChannelElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CommunicationChannelElement)element).Name;
        }
    }

    public class CommunicationChannelElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public String Name
        {
            get
            { return (String)this["name"]; }
            set
            { this["name"] = value; }
        }

        [ConfigurationProperty("forwarder", IsRequired = true)]
        public String Forwarder
        {
            get
            { return (String)this["forwarder"]; }
            set
            { this["forwarder"] = value; }
        }

        [ConfigurationProperty("debugtransaction", IsRequired = true)]
        public String DebugTransaction
        {
            get
            { return (String)this["debugtransaction"]; }
            set
            { this["debugtransaction"] = value; }
        }

        [ConfigurationProperty("debugmessages", IsRequired = true)]
        public String DebugMessages
        {
            get
            { return (String)this["debugmessages"]; }
            set
            { this["debugmessages"] = value; }
        }

        [ConfigurationProperty("transactionfile", IsRequired = true)]
        public String TransactionFile
        {
            get
            { return (String)this["transactionfile"]; }
            set
            { this["transactionfile"] = value; }
        }

        [ConfigurationProperty("directory", IsRequired = true)]
        public String Directory
        {
            get
            { return (String)this["directory"]; }
            set
            { this["directory"] = value; }
        }

        [ConfigurationProperty("userdata", IsRequired = true)]
        public String Userdata
        {
            get
            { return (String)this["userdata"]; }
            set
            { this["userdata"] = value; }
        }
    }


}