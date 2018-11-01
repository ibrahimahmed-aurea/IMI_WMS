// Generated from template: .\DataContracts\DataContractTemplate.cst
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.Services.Settings.DataContracts
{
    [CollectionDataContract(Namespace = "http://Imi.SupplyChain.Services.Settings.DataContracts/2011/09")]
    public class FindBlobResult : List<Blob>
    {
        public FindBlobResult()
        {
        }

        public FindBlobResult(IEnumerable<Blob> collection)
            : base(collection)
        {
        }
		
        public override string ToString()
        {
            StringBuilder result = new StringBuilder(base.ToString());
            result.Append(string.Format("\r\nBlob count={0}", this.Count));
            return result.ToString();
        }
    }
}
