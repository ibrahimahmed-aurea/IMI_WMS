using System;
using System.Collections.Generic;
using System.Text;
using Imi.SupplyChain.Deployment.Wrappers;
using System.IO;
using System.Text.RegularExpressions;
using Imi.SupplyChain.Deployment.Entities;

namespace Imi.SupplyChain.Deployment
{
    public class ProductStandardList : List<ProductStandard>
    {
        public ProductStandard Get(string productId)
        {
            // Try to find the product with the same id 
            foreach (ProductStandard product in this)
            {
                if (product.ProductId == productId)
                {
                    return product;
                }
            }

            return null;
        }

        public bool Exists(string productId)
        {
            // Try to find the product with the same id
            foreach (ProductStandard product in this)
            {
                if (product.ProductId == productId)
                {
                    return true;
                }
            }

            return false;
        }

        // If there are any published products in the list
        public bool AnyPublished()
        {
            foreach (ProductStandard product in this)
            {
                if (product.IsPublished)
                {
                    return true;
                }
            }

            return false;
        }

        public void CreateOrUpdate(Product product, string productDir)
        {
            ProductStandard standardProduct = ProductStandard.Create(product);

            // Set InstallPath
            standardProduct.InstallPath = productDir;

            // Find all product versions
            try
            {
                string[] versionDirs = Directory.GetDirectories(Path.Combine(standardProduct.InstallPath, Constants.SubPathToVersions));

                // Check if any versions was found
                if (versionDirs.GetUpperBound(0) >= 0)
                {
                    // Loop through all directories
                    foreach (string versionDir in versionDirs)
                    {
                        // Finds the last directory part (X.X.X.X)
                        string directoryName = Path.GetFileName(versionDir);

                        // Check if directory name is valid
                        if (VerifyString.IsVersion(directoryName))
                        {
                            // Try to find the .manifest file (should only be one) that is
                            // the application manifest.
                            string[] foundFiles = Directory.GetFiles(versionDir, "*.manifest");

                            // Only one file should be found!
                            if (foundFiles.GetUpperBound(0) == 0)
                            {
                                standardProduct.Versions.Add(new VersionWrapper(directoryName, versionDir, foundFiles[0]));
                            }
                        }
                    }

                    // Sort versions in latest version first order
                    standardProduct.Versions.Sort(new VersionWrapperComparer());
                    standardProduct.Versions.Reverse();
                }
            }
            catch
            {
                standardProduct.Versions.Clear();
            }

            // Only add product if there are versions installed
            if (standardProduct.Versions.Count > 0)
            {
                // Try to read the Product data from the directory
                standardProduct.SetProductData(SerializeHandler.GetProductStandardData(standardProduct.InstallPath));

                // Check if the product already exist
                if (Exists(standardProduct.ProductId))
                {
                    // Remove the existing product
                    Remove(Get(standardProduct.ProductId));
                }

                // Add product to the list
                Add(standardProduct);
            }
        }
    }
}
