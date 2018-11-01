using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.Services.Authorization.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.Services.Authorization.DataContracts/2011/09")]
    public class CheckAuthorizationResult
    {
        [DataMember(Order = 0)]
        public AuthOperationCollection Operations { get; set; }

        [DataMember(Order = 1)]
        public bool IsAuthorizationEnabled { get; set; }
    }
}
