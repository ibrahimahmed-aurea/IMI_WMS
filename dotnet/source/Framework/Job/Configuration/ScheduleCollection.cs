using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;


namespace Imi.Framework.Job.Configuration
{
    public class ScheduleCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ScheduleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ScheduleElement)element).Name;
        }
    }

    public class ScheduleElement : ConfigurationElement
    {

        [ConfigurationProperty("name", IsRequired = true)]
        public String Name
        {
            get
            { return (String)this["name"]; }
            set
            { this["name"] = value; }
        }

        [ConfigurationProperty("description", IsRequired = true)]
        public String Description
        {
            get
            { return (String)this["description"]; }
            set
            { this["description"] = value; }
        }

        [ConfigurationProperty("RuleSet", IsRequired = true)]
        [ConfigurationCollection(typeof(RuleCollection),
                AddItemName = "Rule")]
        public RuleCollection RuleSet
        {
            get
            { return (RuleCollection)this["RuleSet"]; }
            set
            { this["RuleSet"] = value; }
        }


    }

    public class RuleCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RuleElement)element).Sequence;
        }
    }

    public class RuleElement : ConfigurationElement
    {
        private static int lastSequence;
        private static Object lockObject = new Object();

        private int _sequence;

        public RuleElement()
        {
            lock (lockObject)
            {
                lastSequence++;
            }
            _sequence = lastSequence;
        }

        public int Sequence
        {
            get
            { return _sequence; }
        }

        [ConfigurationProperty("second", IsRequired = true)]
        public String Second
        {
            get
            { return (String)this["second"]; }
            set
            { this["second"] = value; }
        }

        [ConfigurationProperty("minute", IsRequired = true)]
        public String Minute
        {
            get
            { return (String)this["minute"]; }
            set
            { this["minute"] = value; }
        }

        [ConfigurationProperty("hour", IsRequired = true)]
        public String Hour
        {
            get
            { return (String)this["hour"]; }
            set
            { this["hour"] = value; }
        }

        [ConfigurationProperty("dayOfWeek", IsRequired = true)]
        public String DayOfWeek
        {
            get
            { return (String)this["dayOfWeek"]; }
            set
            { this["dayOfWeek"] = value; }
        }

        [ConfigurationProperty("dayOfMonth", IsRequired = true)]
        public String DayOfMonth
        {
            get
            { return (String)this["dayOfMonth"]; }
            set
            { this["dayOfMonth"] = value; }
        }

        [ConfigurationProperty("month", IsRequired = true)]
        public String Month
        {
            get
            { return (String)this["month"]; }
            set
            { this["month"] = value; }
        }

    }


}
