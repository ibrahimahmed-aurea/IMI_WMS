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
    public class PackStationFromLCViewToRunPackAllRowsForPackStationActionTranslator : PackStationFromLCViewToRunPackAllRowsForPackStationActionTranslatorBase
    {
        public RunPackAllRowsForPackStationActionParameters Translate(PackStationOverviewViewParameters packStationOverviewParameters, string toLoadCarrierId)
        {
            RunPackAllRowsForPackStationActionParameters runPackAllRowsForPackStationActionParameters = new RunPackAllRowsForPackStationActionParameters();
            runPackAllRowsForPackStationActionParameters.ClientId = UserSessionService.ClientId;
            runPackAllRowsForPackStationActionParameters.DepartureId = packStationOverviewParameters.DepartureId;
            runPackAllRowsForPackStationActionParameters.ShipToCustomerId = packStationOverviewParameters.ShipToCustomerId;
            runPackAllRowsForPackStationActionParameters.LoadCarrierId = toLoadCarrierId;
            runPackAllRowsForPackStationActionParameters.UserId = packStationOverviewParameters.UserId;

            return runPackAllRowsForPackStationActionParameters;
        }
		
    }
}	
