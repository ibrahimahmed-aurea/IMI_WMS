using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Imi.SupplyChain.Transportation.Services.Authentication.DataContracts;

namespace Imi.SupplyChain.Transportation.Services.Authentication.ServiceContracts
{
    [MessageContract(WrapperName = "FindUserDetailsRequest", WrapperNamespace = "http://Imi.SupplyChain.Transportation.Services.Authentication.ServiceContracts/2011/09")]
    public class FindUserDetailsRequest : RequestMessageBase
    {
        [MessageBodyMember(Order = 0)]
        public FindUserDetailsParameters FindUserDetailsParameters { get; set; }

    }

}
