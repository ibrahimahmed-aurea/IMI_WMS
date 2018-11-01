using System;
using System.Collections.Generic;
using System.Text;
using Imi.SupplyChain.Deployment.Entities;
using System.IO;
using System.Xml.Serialization;

namespace Imi.SupplyChain.Deployment
{
    public static class SerializeHandler
    {
        public const string ProductFilename = "Product.xml";
        public const string ProductDataFilename = "ProductData.xml";
        public const string InstancesFilename = "Instances.xml";

        static SerializeHandler() {}

        public static Product GetProduct(string path)
        {
            string fileName = Path.Combine(path, ProductFilename);

            if (File.Exists(fileName))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Product));
                    Product product = null;

                    using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        product = (Product)serializer.Deserialize(fileStream);
                        fileStream.Close();
                    }

                    return product;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public static bool SaveProduct(Product product, string path)
        {
            string fileName = Path.Combine(path, ProductFilename);

            if (Directory.Exists(path))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Product));

                    using (StreamWriter writer = new StreamWriter(fileName))
                    {
                        serializer.Serialize(writer, product);
                        writer.Close();
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public static ProductStandardData GetProductStandardData(string path)
        {
            string fileName = Path.Combine(path, ProductDataFilename);

            if (File.Exists(fileName))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ProductStandardData));
                    ProductStandardData product = null;

                    using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        product = (ProductStandardData)serializer.Deserialize(fileStream);
                        fileStream.Close();
                    }

                    return product;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public static bool SaveProductStandardData(ProductStandardData product, string path)
        {
            string fileName = Path.Combine(path, ProductDataFilename);

            if (Directory.Exists(path))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ProductStandardData));

                    using (StreamWriter writer = new StreamWriter(fileName))
                    {
                        serializer.Serialize(writer, product);
                        writer.Close();
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}
