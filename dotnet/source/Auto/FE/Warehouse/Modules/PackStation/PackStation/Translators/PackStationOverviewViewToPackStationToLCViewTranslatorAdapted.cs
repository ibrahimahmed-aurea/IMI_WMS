// Generated from template: .\UX\Dialog\ViewEventTranslatorTemplate.cst
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Modules.Warehouse.Infrastructure.Services;
using Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier;

namespace Imi.SupplyChain.Warehouse.UX.Modules.PackStation.PackStation.Translators
{
    public class PackStationOverviewViewToPackStationToLCViewTranslator : PackStationOverviewViewToPackStationToLCViewTranslatorBase
    {
        public PackStationToLCViewParameters TranslateFromResultToParameters(string toLoadCarrierId)
        {
            PackStationToLCViewParameters viewParameters = null;

            if (!string.IsNullOrEmpty(toLoadCarrierId))
            {
                viewParameters = new PackStationToLCViewParameters();

                viewParameters.LoadCarrierId = toLoadCarrierId;
            }

            return viewParameters;
        }
    }
}	
