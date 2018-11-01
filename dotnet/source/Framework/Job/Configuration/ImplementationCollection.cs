using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Imi.Framework.Job.Configuration
{
    public class ImplementationCollection : ParameterCollection
    {

        [ConfigurationProperty("name", IsRequired = true)]
        public String Name
        {
            get
            { return (String)this["name"]; }
            set
            { this["name"] = value; }
        }

        [ConfigurationCollection(typeof(ParameterCollection),
                AddItemName = "Parameter")]
        public ParameterCollection ParameterList
        {
            get
            { return (ParameterCollection)this["ParameterList"]; }
            set
            { this["ParameterList"] = value; }
        }

    }

    public class ParameterCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ParameterElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ParameterElement)element).Name;
        }
    }

    public class ParameterElement : ConfigurationElement
    {

        [ConfigurationProperty("name", IsRequired = true)]
        public String Name
        {
            get
            { return (String)this["name"]; }
            set
            { this["name"] = value; }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public String Value
        {
            get
            { return (String)this["value"]; }
            set
            { this["value"] = value; }
        }

    }


}
