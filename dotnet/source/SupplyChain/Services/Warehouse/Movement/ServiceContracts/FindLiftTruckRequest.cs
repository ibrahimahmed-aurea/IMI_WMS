using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Cdc.Framework.Services;
using Cdc.SupplyChain.Warehouse.Services.Movement.DataContracts;

namespace Cdc.SupplyChain.Warehouse.Services.Movement.ServiceContracts
{
    [MessageContract(WrapperName = "FindLiftTruckRequest", WrapperNamespace = "http://Cdc.SupplyChain.Warehouse.Services.Movement.ServiceContracts/2007/05")]
    public class FindLiftTruckRequest
    {
        [MessageBodyMember(Order = 0)]
        public FindLiftTruckParameters FindLiftTruckParameters { get; set; }

    }

}
