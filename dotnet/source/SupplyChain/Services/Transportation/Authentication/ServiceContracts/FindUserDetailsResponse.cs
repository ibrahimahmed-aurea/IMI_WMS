using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Imi.SupplyChain.Transportation.Services.Authentication.DataContracts;

namespace Imi.SupplyChain.Transportation.Services.Authentication.ServiceContracts
{
    [MessageContract(WrapperName = "FindUserDetailsResponse", WrapperNamespace = "http://Imi.SupplyChain.Transportation.Services.Authentication.ServiceContracts/2011/09")]
    public class FindUserDetailsResponse 
    {
        [MessageBodyMember(Order = 0)]
        public FindUserDetailsResult FindUserDetailsResult { get; set; }

    }
}
