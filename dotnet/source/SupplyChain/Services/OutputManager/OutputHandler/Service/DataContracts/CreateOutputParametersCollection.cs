using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.OM.Services.OutputHandler.DataContracts
{
    [CollectionDataContract(Namespace = "http://Imi.SupplyChain.OM.Services.OutputHandler.DataContracts/2016/11")]
    public class CreateOutputParametersCollection : List<CreateOutputParameters>
    {
        public  CreateOutputParametersCollection()
        {
        }

        public CreateOutputParametersCollection(IEnumerable<CreateOutputParameters> collection)
            : base(collection)
        {
        }
    }
}
