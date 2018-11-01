using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Imi.Framework.Job.Configuration
{
    public class JobTypeCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new JobTypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JobTypeElement)element).Name;
        }
    }

    public class JobTypeElement : ConfigurationElement
    {

        [ConfigurationProperty("name", IsRequired = true)]
        public String Name
        {
            get
            { return (String)this["name"]; }
            set
            { this["name"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public String Type
        {
            get
            { return (String)this["type"]; }
            set
            { this["type"] = value; }
        }
    }
}
