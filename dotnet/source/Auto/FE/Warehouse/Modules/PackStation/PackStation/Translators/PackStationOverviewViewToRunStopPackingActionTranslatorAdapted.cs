// Generated from template: .\UX\Module\ActionTranslatorTemplate.cst
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier;
using Imi.SupplyChain.Warehouse.UX.Modules.PackStation.Actions;
using Imi.SupplyChain.UX.Modules.Warehouse.Infrastructure.Services;

namespace Imi.SupplyChain.Warehouse.UX.Modules.PackStation.PackStation.Translators
{
    public class PackStationOverviewViewToRunStopPackingActionTranslator : PackStationOverviewViewToRunStopPackingActionTranslatorBase
    {

        public RunStopPackingActionParameters Translate(PackStationOverviewViewParameters packStationOverviewParameters)
        {
            RunStopPackingActionParameters runStopPackingActionParameters = new RunStopPackingActionParameters();
            runStopPackingActionParameters.WarehouseId = UserSessionService.WarehouseId;
            runStopPackingActionParameters.UserId = packStationOverviewParameters.UserId;
            runStopPackingActionParameters.DepartureId = packStationOverviewParameters.DepartureId;
            runStopPackingActionParameters.ShipToCustomerId = packStationOverviewParameters.ShipToCustomerId;
            runStopPackingActionParameters.ClientId = UserSessionService.ClientId;

            return runStopPackingActionParameters;
        }
		
    }
}	
