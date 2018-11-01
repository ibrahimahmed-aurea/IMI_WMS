using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Imi.SupplyChain.Deployment.Wrappers;

namespace Imi.SupplyChain.Deployment.Entities
{
    public enum ProductType { Standard }

    [Serializable]
    public class Product
    {
        // Must be set in the serialized file
        public string ProductId;
        public string ProductName;
        public string Company;

        // Optional settings not mandatory in serialized file
        [OptionalField]
        public ProductType ProductType; // Defaults to Standard

        // Parameters for the product
        [OptionalField]
        public Parameters Parameters;

        // Constructor
        public Product()
        {
            // Set all default values for all fields
            ProductName = string.Empty;
            Company = string.Empty;
            ProductType = ProductType.Standard;
            Parameters = new Parameters();
        }
    }
}
