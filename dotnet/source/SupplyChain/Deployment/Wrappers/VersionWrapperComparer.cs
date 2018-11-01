using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Imi.SupplyChain.Deployment.Wrappers;

namespace Imi.SupplyChain.Deployment
{
    public class VersionWrapperComparer : IComparer<VersionWrapper>
    {
        public int Compare(VersionWrapper v1, VersionWrapper v2)
        {
            if (v1 == null || v2 == null)
            {
                return 0;
            }

            return VersionHandler.VersionStringCompare(v1.Name, v2.Name);
        }
    }
}
