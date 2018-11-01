using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Imi.Wms.Mobile.Server.Configuration
{

    public class ServerSection : ConfigurationSection
    {
        public const string SectionKey = "imi.wms.mobile.server";

        public ServerSection()
        {
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

        [ConfigurationProperty("managerPort", IsRequired = true)]
        public int ManagerPort
        {
            get
            { return (int)this["managerPort"]; }
            set
            { this["managerPort"] = value; }
        }

        [ConfigurationProperty("sessionIdleTimeout", IsRequired = true)]
        public int SessionIdleTimeout
        {
            get
            { return (int)this["sessionIdleTimeout"]; }
            set
            { this["sessionIdleTimeout"] = value; }
        }

        [ConfigurationProperty("stateTimeout", IsRequired = true)]
        public int StateTimeout
        {
            get
            { return (int)this["stateTimeout"]; }
            set
            { this["stateTimeout"] = value; }
        }

        [ConfigurationProperty("desktopHeapSizeInKB", IsRequired = true)]
        public ulong DesktopHeapSizeInKB
        {
            get
            { return (ulong)this["desktopHeapSizeInKB"]; }
            set
            { this["desktopHeapSizeInKB"] = value; }
        }
                
        [ConfigurationProperty("tcpAdapter", IsRequired = true)]
        public TcpAdapterElement TcpAdapter
        {
            get
            { return (TcpAdapterElement)this["tcpAdapter"]; }
            set
            { this["tcpAdapter"] = value; }
        }

        [ConfigurationProperty("applications")]
        [ConfigurationCollection(typeof(ApplicationCollection),
                AddItemName = "application")]
        public ApplicationCollection ApplicationCollection
        {
            get
            {
                return (ApplicationCollection)this["applications"];
            }
            set
            {
                this["applications"] = value;
            }
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

        [ConfigurationProperty("socketIdleTimeout", IsRequired = true)]
        public int SocketIdleTimeout
        {
            get
            { return (int)this["socketIdleTimeout"]; }
            set
            { this["socketIdleTimeout"] = value; }
        }

    }

    public class ApplicationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ApplicationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ApplicationElement)element).Name;
        }
    }

    public class ApplicationElement : ConfigurationElement
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
                this["name"] = value;
            }
        }

        [ConfigurationProperty("executablePath", IsRequired = true)]
        public string ExecutablePath
        {
            get
            {
                return (string)this["executablePath"];
            }
            set
            {
                this["executablePath"] = value;
            }
        }

        [ConfigurationProperty("arguments", IsRequired = true)]
        public string Arguments
        {
            get
            {
                return (string)this["arguments"];
            }
            set
            {
                this["arguments"] = value;
            }
        }
    }
}
