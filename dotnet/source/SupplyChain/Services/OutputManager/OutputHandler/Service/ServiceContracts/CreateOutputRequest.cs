using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Imi.SupplyChain.OM.Services.OutputHandler.DataContracts;

namespace Imi.SupplyChain.OM.Services.OutputHandler.ServiceContracts
{
    [MessageContract(WrapperName = "CreateOutputRequest", WrapperNamespace = "http://Imi.SupplyChain.OM.Services.OutputHandler.ServiceContracts/2016/11")]
    public class CreateOutputRequest
    {
        [MessageBodyMember(Order = 1)]
        public CreateOutputParametersCollection CreateOutputParametersCollection { get; set; }
    }
}
