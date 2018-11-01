using System;
using System.Collections.Generic;
using System.Text;
using Imi.SupplyChain.Deployment.Wrappers;
using Imi.SupplyChain.Deployment.Entities;

namespace Imi.SupplyChain.Deployment
{
    public class ProductStandard : Product
    {
        public string InstallPath { get; set; }

        private VersionWrapperList versions = null;

        // List of all versions for the product. 
        // Should always be sorted in latest version first.
        public VersionWrapperList Versions
        {
            get { return versions; }
        }

        public string LatestVersion
        {
            get
            {
                // If any versions exist then the first in the list
                // is returned since this should be the latest version.
                if (Versions.Count > 0)
                {
                    return Versions[0].Name;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        // Product data, publish information and instances for the product
        private ProductStandardData data = null;

        public void SetProductData(ProductStandardData productData)
        {
            if (productData != null)
            {
                data = productData;
            }
        }

        // Procedure for fetching the productdata used when serializing
        public ProductStandardData GetProductData()
        {
            return data;
        }

        // IsPublished
        public bool IsPublished 
        {
            get
            {
                return data.IsPublished;
            }
            set
            {
                data.IsPublished = value;
            }
        }

        // VirtualDirectoryName
        public string VirtualDirectoryName
        {
            get
            {
                return data.VirtualDirectoryName;
            }
            set
            {
                data.VirtualDirectoryName = value;
            }
        }

        // VirtualDirectoryPath
        public string VirtualDirectoryPath
        {
            get
            {
                return data.VirtualDirectoryPath;
            }
            set
            {
                data.VirtualDirectoryPath = value;
            }
        }

        // Instances
        public Instances Instances
        {
            get
            {
                return data.Instances;
            }
            set
            {
                data.Instances = value;
            }
        }

        public static ProductStandard Create(Product product)
        {
            ProductStandard newProduct = null;

            if (product != null)
            {
                newProduct = new ProductStandard(product);
            }

            return newProduct;
        }

        // Constructor
        public ProductStandard() : this(null) {}

        public ProductStandard(Product product)
        {
            // Set all default values for all fields
            versions = new VersionWrapperList();
            data = new ProductStandardData();
            Parameters = new Parameters();

            if (product != null)
            {
                // Set product info
                Company = product.Company;
                ProductName = product.ProductName;
                ProductType = product.ProductType;
                ProductId = product.ProductId;
                Parameters.AddRange(product.Parameters);
            }
        }

    }
}
