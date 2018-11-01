using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Cdc.SupplyChain.Warehouse.Services.Movement.DataContracts
{
    [DataContract(Namespace = "http://Cdc.SupplyChain.Warehouse.Services.Movement.DataContracts/2007/05")]
    public class FindLiftTruckParameters
    {
        [DataMember(Order = 1, IsRequired = true)]
        public string SearchString { get; set; }
    }
}
