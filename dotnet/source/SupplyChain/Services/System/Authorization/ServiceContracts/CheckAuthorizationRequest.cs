using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Imi.SupplyChain.Services.Authorization.DataContracts;

namespace Imi.SupplyChain.Services.Authorization.ServiceContracts
{
    [MessageContract(WrapperName = "CheckAuthorizationRequest", WrapperNamespace = "http://Imi.SupplyChain.Services.Authorization.ServiceContracts/2011/09")]
    public class CheckAuthorizationRequest : RequestMessageBase
    {
        [MessageBodyMember(Order = 0)]
        public CheckAuthorizationParameters CheckAuthorizationParameters { get; set; }
    }
}
