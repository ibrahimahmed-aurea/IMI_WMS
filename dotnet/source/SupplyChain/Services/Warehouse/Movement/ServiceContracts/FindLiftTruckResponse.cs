using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Cdc.SupplyChain.Warehouse.Services.Movement.DataContracts;

namespace Cdc.SupplyChain.Warehouse.Services.Movement.ServiceContracts
{
    [MessageContract(WrapperName = "FindLiftTruckResponse", WrapperNamespace = "http://Cdc.SupplyChain.Warehouse.Services.Movement.ServiceContracts/2007/05")]
    public class FindLiftTruckResponse 
    {
        [MessageBodyMember(Order = 0)]
        public FindLiftTruckResult FindLiftTruckResult { get; set; }

    }
}
