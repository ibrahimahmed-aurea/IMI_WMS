using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.SupplyChain.Deployment.Wrappers
{
    public class ProductWrapperList : List<ProductWrapper>
    {
        public ProductWrapperList() : base() { }

        public bool Exist(string productname)
        {
            foreach (ProductWrapper product in this)
            {
                if (product.Name == productname)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
