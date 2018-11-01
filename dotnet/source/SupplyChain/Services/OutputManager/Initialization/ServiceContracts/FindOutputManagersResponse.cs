using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Imi.SupplyChain.OutputManager.Services.Initialization.DataContracts;

namespace Imi.SupplyChain.OutputManager.Services.Initialization.ServiceContracts
{
    [MessageContract(WrapperName = "FindOutputManagersResponse", WrapperNamespace = "http://Imi.SupplyChain.OutputManager.Services.Initialization.ServiceContracts/2011/09")]
    public class FindOutputManagersResponse 
    {
        [MessageBodyMember(Order = 0)]
        public FindOutputManagerResult FindOutputManagerResult { get; set; }

    }
}
