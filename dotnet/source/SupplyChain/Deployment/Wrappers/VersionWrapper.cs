using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.SupplyChain.Deployment.Wrappers
{
    public class VersionWrapper
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string FoundFile { get; set; }

        public VersionWrapper(string name, string path, string foundFile)
        {
            Name = name;
            Path = path;
            FoundFile = foundFile;
        }
    }
}
