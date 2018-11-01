using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Imi.SupplyChain.Transportation.Services.Tracing.DataContracts;

namespace Imi.SupplyChain.Transportation.Services.Tracing.ServiceContracts
{
    [MessageContract(WrapperName = "GetServerInformationResponse", WrapperNamespace = "http://Imi.SupplyChain.Transportation.Services.Tracing.ServiceContracts/2011/09")]
    public class GetServerInformationResponse
    {
        [MessageBodyMember(Order = 0)]
        public GetServerInformationResult GetServerInformationResult{ get; set; }
    }
 
}
