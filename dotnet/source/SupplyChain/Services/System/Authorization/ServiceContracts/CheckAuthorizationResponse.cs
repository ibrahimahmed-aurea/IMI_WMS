using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Imi.SupplyChain.Services.Authorization.DataContracts;

namespace Imi.SupplyChain.Services.Authorization.ServiceContracts
{
    [MessageContract(WrapperName = "CheckAuthorizationResponse", WrapperNamespace = "http://Imi.SupplyChain.Services.Authorization.ServiceContracts/2011/09")]
    public class CheckAuthorizationResponse
    {
        [MessageBodyMember(Order = 0)]
        public CheckAuthorizationResult CheckAuthorizationResult { get; set; }
    }
}
