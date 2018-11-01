using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;

namespace Imi.Framework.Shared.Configuration
{
    public class InstanceDataCollection: ConfigurationElementCollection
    {
        public InstanceDataCollection()
        { 
        
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new InstanceDataElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((InstanceDataElement)element).Name;
        }

        public InstanceDataElement this[int index]
        {
            get
            {
                return (InstanceDataElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public InstanceDataElement this[string name]
        {
            get
            {
                return (InstanceDataElement)BaseGet(name);
            }
        }

        public int IndexOf(InstanceDataElement instance)
        {
            return BaseIndexOf(instance);
        }

        public void Add(InstanceDataElement instance)
        {
            BaseAdd(instance);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(InstanceDataElement instance)
        {
            if (BaseIndexOf(instance) >= 0)
                BaseRemove(instance.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
               
    }


    public class SharedConfigurationSection : ConfigurationSection
    { 
        // Declare the collection element.
        //UrlConfigElement url;

        public SharedConfigurationSection()
        {
            
        }

        
        [ConfigurationProperty("instances", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(InstanceDataCollection), 
            AddItemName="add", 
            ClearItemsName="clear",
            RemoveItemName="remove")]
        public InstanceDataCollection Instances
        {
            get
            {
                InstanceDataCollection instanceDataCollection =
                (InstanceDataCollection)base["instances"];
                return instanceDataCollection;
            }
        }
                
    }

    public class InstanceDataElement: ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get
            {
                return (string)this["name"];            
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("namespace")]
        public string Namespace
        {
            get
            {
                return (string)this["namespace"];
            }
            set
            {
                this["namespace"] = value;
            }
        }

        [ConfigurationProperty("file")]
        public string ConfigurationFile
        {
            get
            {
                return (string)this["file"];
            }
            set
            {
                this["file"] = value;
            }
        }
                
    }
}
