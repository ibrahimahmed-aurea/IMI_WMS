using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Cdc.UXPublish.Config
{
    public class ProductElementCollection : ConfigurationElementCollection
    {
        public ProductElementCollection()
		{
		}

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get
            {
                return "product";
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return new ConfigurationPropertyCollection();
            }
        }


        public ProductElement this[int index]
        {
            get
            {
                return (ProductElement)base.BaseGet(index);
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                base.BaseAdd(index, value);
            }
        }

        public new ProductElement this[string name]
        {
            get
            {
                return (ProductElement)base.BaseGet(name);
            }
        }

        public void Add(ProductElement item)
        {
            base.BaseAdd(item);
        }

        public void Remove(string name)
        {
            base.BaseRemove(name);
        }

        public void Remove(ProductElement item)
        {
            base.BaseRemove(GetElementKey(item));
        }

        public void RemoveAt(int index)
        {
            base.BaseRemoveAt(index);
        }

        public void Clear()
        {
            base.BaseClear();
        }

        public string GetKey(int index)
        {
            return (string)base.BaseGetKey(index);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ProductElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as ProductElement).Name;
        }

    }
}
