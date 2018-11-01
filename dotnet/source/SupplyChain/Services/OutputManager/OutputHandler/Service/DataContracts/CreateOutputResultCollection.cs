using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.OM.Services.OutputHandler.DataContracts
{
    [CollectionDataContract(Namespace = "http://Imi.SupplyChain.OM.Services.OutputHandler.DataContracts/2016/11")]
    public class CreateOutputResultCollection : List<CreateOutputResult>
    {
        public  CreateOutputResultCollection()
        {
        }

        public CreateOutputResultCollection(IEnumerable<CreateOutputResult> collection)
            : base(collection)
        {
        }
    }
}
