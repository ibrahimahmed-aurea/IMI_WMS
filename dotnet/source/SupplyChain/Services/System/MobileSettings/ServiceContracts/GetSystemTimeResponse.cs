using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Cdc.SupplyChain.Services.System.MobileSettings.DataContracts;

namespace Cdc.SupplyChain.Services.System.MobileSettings.ServiceContracts
{
    [MessageContract(WrapperName = "GetSystemTimeResponse", WrapperNamespace = "http://SupplyChain.Services.System.MobileSettings.ServiceContracts/2010/02")]
    public class GetSystemTimeResponse
    {
        [MessageBodyMember(Order = 0)]
        public GetSystemTimeResult GetSystemTimeResult { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\r\n{1}", base.ToString(), GetSystemTimeResult.ToString());
        }

    }
}