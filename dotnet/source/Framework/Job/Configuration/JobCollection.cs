using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;


namespace Imi.Framework.Job.Configuration
{
    public class JobCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new JobElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JobElement)element).Name;
        }
    }

    public enum LogTypeLoglevel
    {

        /// <remarks/>
        Off,

        /// <remarks/>
        Information,

        /// <remarks/>
        Error,

        /// <remarks/>
        Warning,

        /// <remarks/>
        Verbose,
    }

    public class LogElement : ConfigurationElement
    {

        [ConfigurationProperty("logLevel", IsRequired = true)]
        public LogTypeLoglevel LogLevel
        {
            get
            { return (LogTypeLoglevel)this["logLevel"]; }
            set
            { this["logLevel"] = value; }
        }

        [ConfigurationProperty("fileName", IsRequired = true)]
        public String FileName
        {
            get
            { return (String)this["fileName"]; }
            set
            { this["fileName"] = value; }
        }

        [ConfigurationProperty("maxLogSize", IsRequired = true)]
        public ulong MaxLogSize
        {
            get
            { return (ulong)this["maxLogSize"]; }
            set
            { this["maxLogSize"] = value; }
        }

    }

    public class JobElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public String Name
        {
            get
            { return (String)this["name"]; }
            set
            { this["name"] = value; }
        }

        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            { return (bool)this["enabled"]; }
            set
            { this["enabled"] = value; }
        }

        [ConfigurationProperty("scheduleName", IsRequired = true)]
        public String ScheduleName
        {
            get
            { return (String)this["scheduleName"]; }
            set
            { this["scheduleName"] = value; }
        }

        [ConfigurationProperty("waitForEvent", IsRequired = true)]
        public bool WaitForEvent
        {
            get
            { return (bool)this["waitForEvent"]; }
            set
            { this["waitForEvent"] = value; }
        }

        [ConfigurationProperty("Log", IsRequired = false)]
        public LogElement Log
        {
            get
            { return (LogElement)this["Log"]; }
            set
            { this["Log"] = value; }
        }

        [ConfigurationProperty("Implementation", IsRequired = true)]
        [ConfigurationCollection(typeof(ParameterCollection),
                AddItemName = "Parameter")]
        public ImplementationCollection Implementation
        {
            get
            { return (ImplementationCollection)this["Implementation"]; }
            set
            { this["Implementation"] = value; }
        }

    }

}
