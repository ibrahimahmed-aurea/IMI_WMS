using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Cdc.Framework.Services;
using Cdc.SupplyChain.Warehouse.Services.Movement.ServiceContracts;

namespace Cdc.SupplyChain.Warehouse.Services.Movement.ServiceContracts
{
    [ServiceContract(Namespace = "http://Cdc.SupplyChain.Warehouse.Services.Movement.ServiceContracts/2007/05")]
    [ServiceApplicationName("Warehouse")]
    public interface IMovementService
    {
        [OperationContract(Action = "FindLiftTruck")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        FindLiftTruckResponse FindLiftTruck(
            FindLiftTruckRequest request);
    }

}
