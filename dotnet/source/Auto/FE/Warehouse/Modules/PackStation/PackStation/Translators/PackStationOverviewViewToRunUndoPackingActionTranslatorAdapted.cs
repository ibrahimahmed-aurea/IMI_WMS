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
    public class PackStationOverviewViewToRunUndoPackingActionTranslator : PackStationOverviewViewToRunUndoPackingActionTranslatorBase
    {
        public RunUndoPackingActionParameters Translate(string toLoadCarrierId)
        {
            RunUndoPackingActionParameters runUndoPackingActionParameters = new RunUndoPackingActionParameters();
            runUndoPackingActionParameters.LoadCarrierId = toLoadCarrierId;

            return runUndoPackingActionParameters;
        }
    }
}
