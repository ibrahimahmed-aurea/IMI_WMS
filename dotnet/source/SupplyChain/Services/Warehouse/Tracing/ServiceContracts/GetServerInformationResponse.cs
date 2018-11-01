using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Imi.SupplyChain.Warehouse.Services.Tracing.DataContracts;

namespace Imi.SupplyChain.Warehouse.Services.Tracing.ServiceContracts
{
    [MessageContract(WrapperName = "GetServerInformationResponse", WrapperNamespace = "http://Imi.SupplyChain.Warehouse.Services.Tracing.ServiceContracts/2011/09")]
    public class GetServerInformationResponse
    {
        [MessageBodyMember(Order = 0)]
        public GetServerInformationResult GetServerInformationResult{ get; set; }
    }
 
}
