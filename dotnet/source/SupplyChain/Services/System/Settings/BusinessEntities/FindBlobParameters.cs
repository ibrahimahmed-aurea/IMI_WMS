using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Settings.BusinessEntities
{
    public class FindBlobParameters
    {
        public string ContainerName { get; set; }
        public string BlobName { get; set; } /* like */

        public override string ToString()
        {
            StringBuilder result = new StringBuilder(base.ToString());
            result.Append(string.Format("\r\n{0}={1}", "ContainerName", ContainerName));
            result.Append(string.Format("\r\n{0}={1}", "BlobName", BlobName));
            return result.ToString();
        }	

    }
}
