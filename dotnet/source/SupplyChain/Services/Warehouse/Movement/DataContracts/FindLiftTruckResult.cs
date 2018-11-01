using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Cdc.SupplyChain.Warehouse.Services.Movement.DataContracts
{
    [DataContract(Namespace = "http://Cdc.SupplyChain.Warehouse.Services.Movement.DataContracts/2007/05")]
    public class LiftTruck
    {
        [DataMember(Order = 1, IsRequired = false)]
        public string LiftTruckIdentity { get; set; }

        [DataMember(Order = 2, IsRequired = false)]
        public string LiftTruckName { get; set; }
    }


    [CollectionDataContract(Namespace = "http://Cdc.SupplyChain.Warehouse.Services.Movement.DataContracts/2007/05")]
    public class FindLiftTruckResult : List<LiftTruck>
    {
        public FindLiftTruckResult()
        {
        }

        public FindLiftTruckResult(IEnumerable<LiftTruck> collection)
            : base(collection)
        {
        }
    }

}
