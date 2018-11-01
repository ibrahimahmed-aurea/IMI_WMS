using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.SupplyChain.Deployment.Wrappers
{
    public class VersionWrapperList : List<VersionWrapper>
    {
        public VersionWrapperList() : base() { }

        public bool Exists(string version)
        {
            foreach (VersionWrapper ver in this)
            {
                if (ver.Name == version)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
