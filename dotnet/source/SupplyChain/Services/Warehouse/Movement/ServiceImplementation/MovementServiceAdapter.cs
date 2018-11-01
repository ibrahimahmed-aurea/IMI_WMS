using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.SupplyChain.Warehouse.Services.Movement.ServiceContracts;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using MovementActions = Cdc.SupplyChain.Warehouse.BusinessLogic.Movement.BusinessLogic;
using MovementEntities = Cdc.SupplyChain.Warehouse.BusinessLogic.Movement.Entities;
using DataContracts = Cdc.SupplyChain.Warehouse.Services.Movement.DataContracts;
using Cdc.Framework.Services;

namespace Cdc.SupplyChain.Warehouse.Services.Movement.ServiceImplementation
{
    public class MovementServiceAdapter : MarshalByRefObject, IMovementService
    {
        public FindLiftTruckResponse FindLiftTruck(FindLiftTruckRequest request)
        {
            MovementActions.FindLiftTruckAction action = PolicyInjection.Create<MovementActions.FindLiftTruckAction>();

            MovementEntities.FindLiftTruckResult r = action.Execute(Translators.FindLiftTruckTranslator.TranslateFromServiceToBusiness(request.FindLiftTruckParameters));

            FindLiftTruckResponse response = new FindLiftTruckResponse();

            response.FindLiftTruckResult = Translators.FindLiftTruckTranslator.TranslateFromBusinessToService(r);

            return response;
        }

    }
}
