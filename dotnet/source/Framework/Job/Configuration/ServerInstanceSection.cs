using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Imi.Framework.Job.Configuration
{

    public class ServerInstanceSection : ConfigurationSection
    {
        public const string SectionKey = "imi.supplychain.server";

        public ServerInstanceSection()
        {
        }

        [ConfigurationProperty("systemId", IsRequired = true)]
        public string SystemId
        {
            get
            { return (string)this["systemId"]; }
            set
            { this["systemId"] = value; }
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            { return (string)this["name"]; }
            set
            { this["name"] = value; }
        }

        [ConfigurationProperty("database", IsRequired = true)]
        public string Database
        {
            get
            { return (string)this["database"]; }
            set
            { this["database"] = value; }
        }

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get
            { return (int)this["port"]; }
            set
            { this["port"] = value; }
        }

        [ConfigurationProperty("uri", IsRequired = true)]
        public string URI
        {
            get
            { return (string)this["uri"]; }
            set
            { this["uri"] = value; }
        }

        [ConfigurationProperty("autoStart", IsRequired = true)]
        public bool AutoStart
        {
            get
            { return (bool)this["autoStart"]; }
            set
            { this["autoStart"] = value; }
        }

        [ConfigurationProperty("Log", IsRequired = true)]
        public LogElement Log
        {
            get
            { return (LogElement)this["Log"]; }
            set
            { this["Log"] = value; }
        }

        [ConfigurationProperty("ScheduleList")]
        [ConfigurationCollection(typeof(ScheduleCollection),
                AddItemName = "Schedule")]
        public ScheduleCollection ScheduleList
        {
            get
            { return (ScheduleCollection)this["ScheduleList"]; }
            set
            { this["ScheduleList"] = value; }
        }

        [ConfigurationProperty("JobTypeList")]
        [ConfigurationCollection(typeof(JobTypeCollection),
                AddItemName = "JobType")]
        public JobTypeCollection JobTypeList
        {
            get
            { return (JobTypeCollection)this["JobTypeList"]; }
            set
            { this["JobTypeList"] = value; }
        }

        [ConfigurationProperty("JobList")]
        [ConfigurationCollection(typeof(JobCollection),
                AddItemName = "Job")]
        public JobCollection JobList
        {
            get
            { return (JobCollection)this["JobList"]; }
            set
            { this["JobList"] = value; }
        }
    }

}