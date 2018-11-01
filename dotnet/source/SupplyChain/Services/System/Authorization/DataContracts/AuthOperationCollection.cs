using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.Services.Authorization.DataContracts
{
    [CollectionDataContract(Namespace = "http://Imi.SupplyChain.Services.Authorization.DataContracts/2011/09")]
    public class AuthOperationCollection : List<AuthOperation>
    {
        public AuthOperationCollection()
        {
        }

        public AuthOperationCollection(IEnumerable<AuthOperation> collection)
            : base(collection)
        {
        }
    }
}
