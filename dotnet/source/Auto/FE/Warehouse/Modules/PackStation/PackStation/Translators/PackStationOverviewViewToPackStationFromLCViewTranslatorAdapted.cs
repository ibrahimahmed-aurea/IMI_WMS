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
    public class PackStationOverviewViewToPackStationFromLCViewTranslator : PackStationOverviewViewToPackStationFromLCViewTranslatorBase
    {
        public PackStationFromLCViewParameters TranslateFromResultToParameters(IList<PackStationOverviewViewResult> viewResult)
        {
            PackStationFromLCViewParameters viewParameters = null;

            if (viewResult != null)
            {
                viewParameters = new PackStationFromLCViewParameters();

                foreach (PackStationOverviewViewResult result in viewResult)
                {
                    if (result.Selected.Value)
                    {
                        viewParameters.LoadCarrierIds += "#" + result.LoadCarrierId.Trim() + "#";
                    }
                }
            }

            return viewParameters;
        }


    }
}	
