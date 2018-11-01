using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Cdc.UXPublish.Config
{
    public class ProductSection : ConfigurationSection
    {
        private static ConfigurationPropertyCollection s_properties;
        private static ConfigurationProperty s_propProducts;

        static ProductSection()
        {
            s_propProducts = new ConfigurationProperty(
                "",
                typeof(ProductElementCollection),
                null,
                ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsDefaultCollection
                );

            s_properties = new ConfigurationPropertyCollection();
            s_properties.Add(s_propProducts);
        }

        public ProductElementCollection Products
        {
            get 
            { 
                return (ProductElementCollection)base[s_propProducts];
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return s_properties;
            }
        }

    }
}
