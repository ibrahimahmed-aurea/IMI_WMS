using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Imi.SupplyChain.OutputManager.Services.Initialization.DataContracts;

namespace Imi.SupplyChain.OutputManager.Services.Initialization.ServiceContracts
{
    [MessageContract(WrapperName = "FindOutputManagersRequest", WrapperNamespace = "http://Imi.SupplyChain.OutputManager.Services.Initialization.ServiceContracts/2011/09")]
    public class FindOutputManagersRequest : RequestMessageBase
    {
        [MessageBodyMember(Order = 0)]
        public FindOutputManagersParameters FindOutputManagersParameters { get; set; }

    }

}
