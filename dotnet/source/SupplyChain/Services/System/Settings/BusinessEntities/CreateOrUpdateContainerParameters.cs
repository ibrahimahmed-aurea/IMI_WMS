using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Settings.BusinessEntities
{
    public class CreateOrUpdateContainerParameters
    {
        public Container Container { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder(base.ToString());
            result.Append(string.Format("\r\n{0}={1}", "Container", Container));
            return result.ToString();
        }	
    }
}
