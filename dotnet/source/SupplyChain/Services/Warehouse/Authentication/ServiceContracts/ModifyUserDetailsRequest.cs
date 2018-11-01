using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts;

namespace Imi.SupplyChain.Warehouse.Services.Authentication.ServiceContracts
{
    [MessageContract(WrapperName = "ModifyUserDetailsRequest", WrapperNamespace = "http://Imi.SupplyChain.Warehouse.Services.Authentication.ServiceContracts/2011/09")]
    public class ModifyUserDetailsRequest : RequestMessageBase
    {
        [MessageBodyMember(Order = 0)]
        public ModifyUserDetailsParameters ModifyUserDetailsParameters { get; set; }

    }
}
