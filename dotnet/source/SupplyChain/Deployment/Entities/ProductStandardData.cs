using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.Deployment.Entities
{
    [Serializable]
    public class ProductStandardData
    {
        public bool IsPublished;

        public string VirtualDirectoryName;
        public string VirtualDirectoryPath;

        public Instances Instances;

        // Constructor
        public ProductStandardData()
        {
            // Set all default values for all fields
            VirtualDirectoryName = string.Empty;
            VirtualDirectoryPath = string.Empty;
            Instances = new Instances();
        }

    }
}
