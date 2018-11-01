using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts;

namespace Imi.SupplyChain.Warehouse.Services.Authentication.ServiceContracts
{
    [MessageContract(WrapperName = "LogonResponse", WrapperNamespace = "http://Imi.SupplyChain.Warehouse.Services.Authentication.ServiceContracts/2011/09")]
    public class LogonResponse 
    {
        [MessageBodyMember(Order = 0)]
        public LogonResult LogonResult { get; set; }

    }
}
