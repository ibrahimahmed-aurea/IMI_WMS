using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Imi.Wms.Voice.Vocollect.Configuration
{

    public class VocollectSection : ConfigurationSection
    {
        public const string SectionKey = "imi.wms.voice.vocollect";

        public VocollectSection()
        {
        }

        [ConfigurationProperty("xsltPath", IsRequired = true)]
        public string XsltPath
        {
            get
            { return (String)this["xsltPath"]; }
            set
            { this["xsltPath"] = value; }
        }

        [ConfigurationProperty("database", IsRequired = true)]
        public string Database
        {
            get
            { return (string)this["database"]; }
            set
            { this["database"] = value; }
        }

        [ConfigurationProperty("logPath", IsRequired = true)]
        public string LogPath
        {
            get
            { return (string)this["logPath"]; }
            set
            { this["logPath"] = value; }
        }

        [ConfigurationProperty("maxLogSize", IsRequired = true)]
        public ulong MaxLogSize
        {
            get
            { return (ulong)this["maxLogSize"]; }
            set
            { this["maxLogSize"] = value; }
        }

        [ConfigurationProperty("odrConfirmationByte", IsRequired = true)]
        public string ODRConfirmationByte
        {
            get
            { return (String)this["odrConfirmationByte"]; }
            set
            { this["odrConfirmationByte"] = value; }
        }

        [ConfigurationProperty("codePageName", IsRequired = true)]
        public string CodePageName
        {
            get
            { return (String)this["codePageName"]; }
            set
            { this["codePageName"] = value; }
        }

        [ConfigurationProperty("tcpAdapter", IsRequired = true)]
        public TcpAdapterElement TcpAdapter
        {
            get
            { return (TcpAdapterElement)this["tcpAdapter"]; }
            set
            { this["tcpAdapter"] = value; }
        }

        [ConfigurationProperty("messages")]
        [ConfigurationCollection(typeof(MessageCollection),
                AddItemName = "add")]
        public MessageCollection MessageCollection
        {
            get
            { return (MessageCollection)this["messages"]; }
            set
            { this["messages"] = value; }
        }

    }

    public class MessageCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MessageElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MessageElement)element).Id;
        }

        public MessageElement this[string id]
        {
            get
            {
                return (MessageElement)BaseGet(id);
            }
        }
               
    }

    public class MessageElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true)]
        public string Id
        {
            get
            { return (String)this["id"]; }
            set
            { this["id"] = value; }
        }

        [ConfigurationProperty("text", IsRequired = true)]
        public string Text
        {
            get
            { return (String)this["text"]; }
            set
            { this["text"] = value; }
        }

    }

    public class TcpAdapterElement : ConfigurationElement
    {

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get
            { return (int)this["port"]; }
            set
            { this["port"] = value; }
        }

    }



}
