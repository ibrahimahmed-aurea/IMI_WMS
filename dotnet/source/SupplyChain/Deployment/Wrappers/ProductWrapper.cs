using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.SupplyChain.Deployment.Wrappers
{
    public class ProductWrapper
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string InstallPath { get; set; }
        public string VirtualDirectoryRoot { get; set; }

        public ProductWrapper(string name, string company, string installPath, string virtualDirectoryRoot)
        {
            Name = name;
            Company = company;
            InstallPath = installPath;
            VirtualDirectoryRoot = virtualDirectoryRoot;
        }
    }
}
