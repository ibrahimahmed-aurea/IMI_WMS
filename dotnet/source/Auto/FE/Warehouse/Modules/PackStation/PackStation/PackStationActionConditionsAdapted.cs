// Generated from template: .\UX\Dialog\DialogActionConditionsAdaptedTemplate.cst
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.Warehouse.UX.Modules.PackStation.Actions;
using Imi.SupplyChain.Warehouse.UX.Modules.PackStation.PackStation.Translators;
using Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier;

namespace Imi.SupplyChain.Warehouse.UX.Modules.PackStation.PackStation
{
    public class DialogActionConditionsAdapted : DialogActionConditionsBase
    {
        public override bool PackStationFromLcEditRunPackRowForPackStationCondition_CanExecute(string action, WorkItem context, object caller, object target)
        {
            // Multi select is not supported
            if (context.Items.FindByType<IList<PackStationFromLcEditViewResult>>().Count > 0)
            {
                if (context.Items.FindByType<IList<PackStationFromLcEditViewResult>>().Last().Count > 1)
                    return false;
            }

            IActionCatalogService actionCatalog = context.Services.Get<IActionCatalogService>(true);

            PackStationFromLcEditViewResult viewResult = null;
            RunPackRowForPackStationActionParameters actionParameters = null;
            bool isItemSelected = false;

            if (context.Items.FindByType<PackStationFromLcEditViewResult>().Count > 0)
            {
                viewResult = context.Items.FindByType<PackStationFromLcEditViewResult>().Last();
                isItemSelected = true;
            }
            else
            {
                PackStationFromLcEditViewResult vr = new PackStationFromLcEditViewResult();

                if (context.Items.FindByType<PackStationFromLcEditViewParameters>().Count() > 0)
                {
                    PackStationFromLcEditViewParameters viewParameters = context.Items.FindByType<PackStationFromLcEditViewParameters>().Last();

                }
                viewResult = vr;
            }

            PackStationFromLcEditViewToRunPackRowForPackStationActionTranslator translator = null;

            if (context.Items.FindByType<PackStationFromLcEditViewToRunPackRowForPackStationActionTranslator>().Count > 0)
                translator = context.Items.FindByType<PackStationFromLcEditViewToRunPackRowForPackStationActionTranslator>().Last();
            else
                translator = context.Items.AddNew<PackStationFromLcEditViewToRunPackRowForPackStationActionTranslator>();

            actionParameters = translator.Translate(viewResult);
            if (context.Items.FindByType<PackStationOverviewPresenter>().Count > 0)
            {
                if (!string.IsNullOrEmpty(context.Items.FindByType<PackStationOverviewPresenter>().Last().ToLoadCarrierId))
                {
                    actionParameters.ToLoadCarrierId = context.Items.FindByType<PackStationOverviewPresenter>().Last().ToLoadCarrierId;
                }
                else
                {
                    actionParameters.ToLoadCarrierId = string.Empty;
                }
            }
            actionParameters.IsItemSelected = isItemSelected;
            actionParameters.IsMultipleItemsSelected = false;
            return actionCatalog.CanExecute(ActionNames.RunPackRowForPackStation, context, caller, actionParameters);
        }

        public override bool PackStationFromLCRunPackAllRowsForPackStationCondition_CanExecute(string action, WorkItem context, object caller, object target)
        {
            PackStationOverviewPresenter overviewPresenter = context.Items.FindByType<PackStationOverviewPresenter>().LastOrDefault();

            if (!string.IsNullOrEmpty(overviewPresenter.ToLoadCarrierId))
            {
                // Multi select is not supported
                if (context.Items.FindByType<IList<PackStationFromLCViewResult>>().Count > 0)
                {
                    if (context.Items.FindByType<IList<PackStationFromLCViewResult>>().Last().Count > 1)
                        return false;
                }

                IActionCatalogService actionCatalog = context.Services.Get<IActionCatalogService>(true);

                PackStationFromLCViewResult viewResult = null;
                RunPackAllRowsForPackStationActionParameters actionParameters = null;
                bool isItemSelected = false;

                if (context.Items.FindByType<PackStationFromLCViewResult>().Count > 0)
                {
                    viewResult = context.Items.FindByType<PackStationFromLCViewResult>().Last();
                    isItemSelected = true;
                }
                else
                {
                    PackStationFromLCViewResult vr = new PackStationFromLCViewResult();

                    if (context.Items.FindByType<PackStationFromLCViewParameters>().Count() > 0)
                    {
                        PackStationFromLCViewParameters viewParameters = context.Items.FindByType<PackStationFromLCViewParameters>().Last();

                    }
                    viewResult = vr;
                }

                PackStationFromLCViewToRunPackAllRowsForPackStationActionTranslator translator = null;

                if (context.Items.FindByType<PackStationFromLCViewToRunPackAllRowsForPackStationActionTranslator>().Count > 0)
                    translator = context.Items.FindByType<PackStationFromLCViewToRunPackAllRowsForPackStationActionTranslator>().Last();
                else
                    translator = context.Items.AddNew<PackStationFromLCViewToRunPackAllRowsForPackStationActionTranslator>();

                actionParameters = translator.Translate(new PackStationOverviewViewParameters(), "");
                actionParameters.IsItemSelected = isItemSelected;
                actionParameters.IsMultipleItemsSelected = false;
                return actionCatalog.CanExecute(ActionNames.RunPackAllRowsForPackStation, context, caller, actionParameters);
            }

            return false;
        }

        public override bool PackStationOverviewRunStopPackingCondition_CanExecute(string action, WorkItem context, object caller, object target)
        {
            PackStationOverviewViewParameters viewParameters = null;
            if (context.Items.FindByType<PackStationOverviewViewParameters>().Count() > 0)
            {
                viewParameters = context.Items.FindByType<PackStationOverviewViewParameters>().Last();
            }
            else
            {
                viewParameters = new PackStationOverviewViewParameters();
            }


            if (string.IsNullOrEmpty(viewParameters.PickZoneId) && !string.IsNullOrEmpty(viewParameters.DepartureId) && !string.IsNullOrEmpty(viewParameters.ShipToCustomerId) && !string.IsNullOrEmpty(viewParameters.UserId))
            {
                IActionCatalogService actionCatalog = context.Services.Get<IActionCatalogService>(true);

                RunStopPackingActionParameters actionParameters = null;
                bool isItemSelected = false;

                if (context.Items.FindByType<PackStationOverviewViewResult>().Count > 0)
                {
                    isItemSelected = true;
                }

                PackStationOverviewViewToRunStopPackingActionTranslator translator = null;

                if (context.Items.FindByType<PackStationOverviewViewToRunStopPackingActionTranslator>().Count > 0)
                    translator = context.Items.FindByType<PackStationOverviewViewToRunStopPackingActionTranslator>().Last();
                else
                    translator = context.Items.AddNew<PackStationOverviewViewToRunStopPackingActionTranslator>();

                actionParameters = translator.Translate(new PackStationOverviewViewParameters());
                actionParameters.IsItemSelected = isItemSelected;
                actionParameters.IsMultipleItemsSelected = false;

                return actionCatalog.CanExecute(ActionNames.RunStopPacking, context, caller, actionParameters);
            }

            return false;

        }

        public override bool PackStationOverviewRunUndoPackingCondition_CanExecute(string action, WorkItem context, object caller, object target)
        {
            PackStationOverviewPresenter overviewPresenter = context.Items.FindByType<PackStationOverviewPresenter>().LastOrDefault();

            if (!string.IsNullOrEmpty(overviewPresenter.ToLoadCarrierId))
            {
                IActionCatalogService actionCatalog = context.Services.Get<IActionCatalogService>(true);
                RunUndoPackingActionParameters actionParameters = null;

                PackStationOverviewViewToRunUndoPackingActionTranslator translator = null;

                if (context.Items.FindByType<PackStationOverviewViewToRunUndoPackingActionTranslator>().Count > 0)
                    translator = context.Items.FindByType<PackStationOverviewViewToRunUndoPackingActionTranslator>().Last();
                else
                    translator = context.Items.AddNew<PackStationOverviewViewToRunUndoPackingActionTranslator>();

                actionParameters = translator.Translate(overviewPresenter.ToLoadCarrierId);
                actionParameters.IsItemSelected = true;
                actionParameters.IsMultipleItemsSelected = false;
                return actionCatalog.CanExecute(ActionNames.RunUndoPacking, context, caller, actionParameters);
            }

            return false;
        }

        public override bool PackStationToLCRunFinishPackingWorkflowCondition_CanExecute(string action, WorkItem context, object caller, object target)
        {
            // Multi select is not supported
            if (context.Items.FindByType<IList<PackStationToLCViewResult>>().Count > 0)
            {
                if (context.Items.FindByType<IList<PackStationToLCViewResult>>().Last().Count > 1)
                    return false;
            }

            IActionCatalogService actionCatalog = context.Services.Get<IActionCatalogService>(true);

            PackStationToLCViewResult viewResult = null;
            RunFinishPackingWorkflowActionParameters actionParameters = null;
            bool isItemSelected = false;

            if (context.Items.FindByType<PackStationToLCViewResult>().Count > 0)
            {
                viewResult = context.Items.FindByType<PackStationToLCViewResult>().Last();
                isItemSelected = true;
            }
            
            //Check for Pack & Sort dissable finish pack
            if (context.Items.FindByType<PackStationOverviewViewParameters>().Count() > 0)
            {
                PackStationOverviewViewParameters overviewViewParameters = context.Items.FindByType<PackStationOverviewViewParameters>().Last();

                if (!string.IsNullOrEmpty(overviewViewParameters.PickZoneId))
                {
                    if (context.Items.FindByType<IList<PackStationFromLCViewResult>>().Count > 0)
                    {
                        IList<PackStationFromLCViewResult> fromLCViewResults = context.Items.FindByType<IList<PackStationFromLCViewResult>>().Last();

                        if (fromLCViewResults.Count == 0)
                        {
                            isItemSelected = isItemSelected & true;
                        }
                        else
                        {
                            isItemSelected = isItemSelected & false;
                        }
                    }
                    else
                    {
                        isItemSelected = isItemSelected & true;
                    }
                }
            }

            PackStationToLCViewToRunFinishPackingWorkflowActionTranslator translator = null;

            if (context.Items.FindByType<PackStationToLCViewToRunFinishPackingWorkflowActionTranslator>().Count > 0)
                translator = context.Items.FindByType<PackStationToLCViewToRunFinishPackingWorkflowActionTranslator>().Last();
            else
                translator = context.Items.AddNew<PackStationToLCViewToRunFinishPackingWorkflowActionTranslator>();

            actionParameters = translator.Translate(viewResult);
            actionParameters.IsItemSelected = isItemSelected;
            actionParameters.IsMultipleItemsSelected = false;
            return actionCatalog.CanExecute(ActionNames.RunFinishPackingWorkflow, context, caller, actionParameters);
        }
    }
}
