using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.Framework.Services;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Cdc.SupplyChain.Warehouse.Services.Movement.ServiceContracts;

namespace Cdc.SupplyChain.Warehouse.Services.Movement.ServiceImplementation
{
    [ServiceApplicationName("Warehouse")]
    [ExceptionShielding("DefaultShieldingPolicy")]
    public class MovementService : IMovementService
    {
        public FindLiftTruckResponse FindLiftTruck(FindLiftTruckRequest request)
        {
            MovementServiceAdapter adapter = PolicyInjection.Create<MovementServiceAdapter>();
            return adapter.FindLiftTruck(request);
        }
    }
}
