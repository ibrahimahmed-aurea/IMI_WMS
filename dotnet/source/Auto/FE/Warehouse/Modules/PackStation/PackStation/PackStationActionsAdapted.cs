// Generated from template: .\UX\Dialog\DialogActionsTemplate.cst
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.Warehouse.UX.Modules.PackStation;
using Imi.SupplyChain.Warehouse.UX.Modules.PackStation.Actions;
using Imi.SupplyChain.Warehouse.UX.Modules.PackStation.PackStation.Translators;
using Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier;

namespace Imi.SupplyChain.Warehouse.UX.Modules.PackStation.PackStation
{
    public class PackStationActions : PackStationActionsBase
    {
        public override void OnPackStationFromLcEditRunPackRowForPackStation(WorkItem context, object caller, object target)
        {
            WorkItem wi = GetModuleWorkItem(context);

            if (wi.Items.FindByType<RunPackRowForPackStationAction>().Count == 0)
                wi.Items.AddNew<RunPackRowForPackStationAction>();

            PackStationFromLcEditViewResult viewResult = null;
            RunPackRowForPackStationActionParameters actionParameters = null;
            PackStationFromLcEditViewToRunPackRowForPackStationActionTranslator translator = null;
            bool isItemSelected = false;

            if (context.Items.FindByType<PackStationFromLcEditViewToRunPackRowForPackStationActionTranslator>().Count > 0)
                translator = context.Items.FindByType<PackStationFromLcEditViewToRunPackRowForPackStationActionTranslator>().Last();
            else
                translator = context.Items.AddNew<PackStationFromLcEditViewToRunPackRowForPackStationActionTranslator>();

            if (context.Items.FindByType<PackStationFromLcEditViewResult>().Count > 0)
            {
                viewResult = context.Items.FindByType<PackStationFromLcEditViewResult>().Last();
                isItemSelected = true;
            }
            else
            {
                viewResult = new PackStationFromLcEditViewResult();

                if (context.Items.FindByType<PackStationFromLcEditViewParameters>().Count() > 0)
                {
                    PackStationFromLcEditViewParameters viewParameters = context.Items.FindByType<PackStationFromLcEditViewParameters>().Last();

                }
            }

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

            IPackStationFromLcEditView view = context.SmartParts.FindByType<PackStationFromLcEditView>().Last();

            IPackStationOverviewView packStationOverviewView = context.SmartParts.FindByType<IPackStationOverviewView>().LastOrDefault();
            IPackStationFromLCView packStationFromLcView = context.SmartParts.FindByType<IPackStationFromLCView>().LastOrDefault();
            IPackStationToLCView packStationToLcView = context.SmartParts.FindByType<IPackStationToLCView>().LastOrDefault();
            try
            {
                if (!view.Validate())
                    return;

                ActionCatalog.Execute(ActionNames.RunPackRowForPackStation, context, caller, actionParameters);

                // Check if view should be closed.
                bool ignoreClosing = ((context.Items.Get("IgnoreClose") != null) &&
                                    ((bool)(context.Items.Get("IgnoreClose"))));

                if (context.Items.Contains("RefreshPackStationOverview"))
                {
                    if (((bool)context.Items["RefreshPackStationOverview"]))
                    {
                        packStationOverviewView.Refresh();
                    }
                    else
                    {
                        if (packStationFromLcView != null)
                        {
                            packStationFromLcView.Refresh();
                        }

                        if (packStationToLcView != null)
                        {
                            packStationToLcView.Refresh();
                        }
                    }
                }
                else
                {
                    if (packStationFromLcView != null)
                    {
                        packStationFromLcView.Refresh();
                    }

                    if (packStationToLcView != null)
                    {
                        packStationToLcView.Refresh();
                    }
                }
            }
            catch (Imi.SupplyChain.UX.ValidationException ex)
            {
                view.HandleValidationResult(ValidationHelper.ConvertToResult((Imi.SupplyChain.UX.ValidationException)ex));
            }
        }

        public override void OnPackStationLcToEditRunUnPackRowForPackStation(WorkItem context, object caller, object target)
        {
            WorkItem wi = GetModuleWorkItem(context);

            if (wi.Items.FindByType<RunUnPackRowForPackStationAction>().Count == 0)
                wi.Items.AddNew<RunUnPackRowForPackStationAction>();

            PackStationLcToEditViewResult viewResult = null;
            RunUnPackRowForPackStationActionParameters actionParameters = null;
            PackStationLcToEditViewToRunUnPackRowForPackStationActionTranslator translator = null;
            bool isItemSelected = false;

            if (context.Items.FindByType<PackStationLcToEditViewToRunUnPackRowForPackStationActionTranslator>().Count > 0)
                translator = context.Items.FindByType<PackStationLcToEditViewToRunUnPackRowForPackStationActionTranslator>().Last();
            else
                translator = context.Items.AddNew<PackStationLcToEditViewToRunUnPackRowForPackStationActionTranslator>();

            if (context.Items.FindByType<PackStationLcToEditViewResult>().Count > 0)
            {
                viewResult = context.Items.FindByType<PackStationLcToEditViewResult>().Last();
                isItemSelected = true;
            }
            else
            {
                viewResult = new PackStationLcToEditViewResult();

                if (context.Items.FindByType<PackStationLcToEditViewParameters>().Count() > 0)
                {
                    PackStationLcToEditViewParameters viewParameters = context.Items.FindByType<PackStationLcToEditViewParameters>().Last();

                }
            }

            actionParameters = translator.Translate(viewResult);
            actionParameters.IsItemSelected = isItemSelected;
            actionParameters.IsMultipleItemsSelected = false;

            IPackStationLcToEditView view = context.SmartParts.FindByType<PackStationLcToEditView>().Last();
            IPackStationOverviewView packStationOverviewView = context.SmartParts.FindByType<IPackStationOverviewView>().LastOrDefault();
            IPackStationToLCView packStationToLcView = context.SmartParts.FindByType<IPackStationToLCView>().LastOrDefault();
            IPackStationFromLCView packStationFromLcView = context.SmartParts.FindByType<IPackStationFromLCView>().LastOrDefault();

            try
            {
                if (!view.Validate())
                    return;

                ActionCatalog.Execute(ActionNames.RunUnPackRowForPackStation, context, caller, actionParameters);

                // Check if view should be closed.
                bool ignoreClosing = ((context.Items.Get("IgnoreClose") != null) &&
                                    ((bool)(context.Items.Get("IgnoreClose"))));

                if (context.Items.Contains("RefreshPackStationOverview"))
                {
                    if (((bool)context.Items["RefreshPackStationOverview"]))
                    {
                        packStationOverviewView.Refresh();
                    }
                    else
                    {
                        if (packStationFromLcView != null)
                        {
                            packStationFromLcView.Refresh();
                        }

                        if (packStationToLcView != null)
                        {
                            packStationToLcView.Refresh();
                        }
                    }
                }
                else
                {
                    if (packStationFromLcView != null)
                    {
                        packStationFromLcView.Refresh();
                    }

                    if (packStationToLcView != null)
                    {
                        packStationToLcView.Refresh();
                    }
                }
            }
            catch (Imi.SupplyChain.UX.ValidationException ex)
            {
                view.HandleValidationResult(ValidationHelper.ConvertToResult((Imi.SupplyChain.UX.ValidationException)ex));
            }
        }

        public override void OnPackStationFromLCRunPackAllRowsForPackStation(WorkItem context, object caller, object target)
        {
            WorkItem wi = GetModuleWorkItem(context);

            if (wi.Items.FindByType<RunPackAllRowsForPackStationAction>().Count == 0)
                wi.Items.AddNew<RunPackAllRowsForPackStationAction>();

            RunPackAllRowsForPackStationActionParameters actionParameters = null;
            PackStationFromLCViewToRunPackAllRowsForPackStationActionTranslator translator = null;

            if (context.Items.FindByType<PackStationFromLCViewToRunPackAllRowsForPackStationActionTranslator>().Count > 0)
                translator = context.Items.FindByType<PackStationFromLCViewToRunPackAllRowsForPackStationActionTranslator>().Last();
            else
                translator = context.Items.AddNew<PackStationFromLCViewToRunPackAllRowsForPackStationActionTranslator>();

            PackStationOverviewViewParameters viewParameters = null;
            if (context.Items.FindByType<PackStationOverviewViewParameters>().Count() > 0)
            {
                viewParameters = context.Items.FindByType<PackStationOverviewViewParameters>().Last();

            }

            PackStationOverviewPresenter overviewPresenter = null;

            if (context.Items.FindByType<PackStationOverviewPresenter>().Count > 0)
            {
                overviewPresenter = context.Items.FindByType<PackStationOverviewPresenter>().Last();

            }
            if (viewParameters == null || overviewPresenter == null)
            {
                return;
            }

            actionParameters = translator.Translate(viewParameters, overviewPresenter.ToLoadCarrierId);
            actionParameters.IsItemSelected = true;
            actionParameters.IsMultipleItemsSelected = false;

            IPackStationFromLCView view = context.SmartParts.FindByType<PackStationFromLCView>().Last();
            IPackStationOverviewView packStationOverviewView = context.SmartParts.FindByType<IPackStationOverviewView>().LastOrDefault();

            try
            {
                if (!view.Validate())
                    return;

                ActionCatalog.Execute(ActionNames.RunPackAllRowsForPackStation, context, caller, actionParameters);

                // Check if view should be closed.
                bool ignoreClosing = ((context.Items.Get("IgnoreClose") != null) &&
                                    ((bool)(context.Items.Get("IgnoreClose"))));

                packStationOverviewView.Refresh();
            }
            catch (Imi.SupplyChain.UX.ValidationException ex)
            {
                view.HandleValidationResult(ValidationHelper.ConvertToResult((Imi.SupplyChain.UX.ValidationException)ex));
            }
        }

        public override void OnPackStationToLCRunUnPackAllRowsForPackStation(WorkItem context, object caller, object target)
        {
            WorkItem wi = GetModuleWorkItem(context);

            if (wi.Items.FindByType<RunUnPackAllRowsForPackStationAction>().Count == 0)
                wi.Items.AddNew<RunUnPackAllRowsForPackStationAction>();

            PackStationToLCViewResult viewResult = null;
            RunUnPackAllRowsForPackStationActionParameters actionParameters = null;
            PackStationToLCViewToRunUnPackAllRowsForPackStationActionTranslator translator = null;
            bool isItemSelected = false;

            if (context.Items.FindByType<PackStationToLCViewToRunUnPackAllRowsForPackStationActionTranslator>().Count > 0)
                translator = context.Items.FindByType<PackStationToLCViewToRunUnPackAllRowsForPackStationActionTranslator>().Last();
            else
                translator = context.Items.AddNew<PackStationToLCViewToRunUnPackAllRowsForPackStationActionTranslator>();

            if (context.Items.FindByType<PackStationToLCViewResult>().Count > 0)
            {
                viewResult = context.Items.FindByType<PackStationToLCViewResult>().Last();
                isItemSelected = true;
            }
            else
            {
                viewResult = new PackStationToLCViewResult();

                if (context.Items.FindByType<PackStationToLCViewParameters>().Count() > 0)
                {
                    PackStationToLCViewParameters viewParameters = context.Items.FindByType<PackStationToLCViewParameters>().Last();

                }
            }

            actionParameters = translator.Translate(viewResult);
            actionParameters.IsItemSelected = isItemSelected;
            actionParameters.IsMultipleItemsSelected = false;

            IPackStationToLCView view = context.SmartParts.FindByType<PackStationToLCView>().Last();
            IPackStationOverviewView packStationOverviewView = context.SmartParts.FindByType<IPackStationOverviewView>().LastOrDefault();

            try
            {
                if (!view.Validate())
                    return;

                ActionCatalog.Execute(ActionNames.RunUnPackAllRowsForPackStation, context, caller, actionParameters);

                // Check if view should be closed.
                bool ignoreClosing = ((context.Items.Get("IgnoreClose") != null) &&
                                    ((bool)(context.Items.Get("IgnoreClose"))));

                packStationOverviewView.Refresh();
            }
            catch (Imi.SupplyChain.UX.ValidationException ex)
            {
                view.HandleValidationResult(ValidationHelper.ConvertToResult((Imi.SupplyChain.UX.ValidationException)ex));
            }
        }

        public override void OnPackStationToLCRunFinishPackingWorkflow(WorkItem context, object caller, object target)
        {
            WorkItem wi = GetModuleWorkItem(context);

            if (wi.Items.FindByType<RunFinishPackingWorkflowAction>().Count == 0)
                wi.Items.AddNew<RunFinishPackingWorkflowAction>();

            PackStationToLCViewResult viewResult = null;
            RunFinishPackingWorkflowActionParameters actionParameters = null;
            PackStationToLCViewToRunFinishPackingWorkflowActionTranslator translator = null;
            bool isItemSelected = false;

            if (context.Items.FindByType<PackStationToLCViewToRunFinishPackingWorkflowActionTranslator>().Count > 0)
                translator = context.Items.FindByType<PackStationToLCViewToRunFinishPackingWorkflowActionTranslator>().Last();
            else
                translator = context.Items.AddNew<PackStationToLCViewToRunFinishPackingWorkflowActionTranslator>();

            if (context.Items.FindByType<PackStationToLCViewResult>().Count > 0)
            {
                viewResult = context.Items.FindByType<PackStationToLCViewResult>().Last();
                isItemSelected = true;
            }
            else
            {
                viewResult = new PackStationToLCViewResult();

                if (context.Items.FindByType<PackStationToLCViewParameters>().Count() > 0)
                {
                    PackStationToLCViewParameters viewParameters = context.Items.FindByType<PackStationToLCViewParameters>().Last();
                }
            }
            actionParameters = translator.Translate(viewResult);
            actionParameters.IsItemSelected = isItemSelected;
            actionParameters.IsMultipleItemsSelected = false;

            if (context.Items.FindByType<PackStationOverviewViewParameters>().Count > 0)
            {
                PackStationOverviewViewParameters overviewParameters = context.Items.FindByType<PackStationOverviewViewParameters>().Last();
                IPackStationToLCView view = context.SmartParts.FindByType<PackStationToLCView>().Last();
                PackStationOverviewPresenter overviewPresenter = context.Items.FindByType<PackStationOverviewPresenter>().Last();

                actionParameters.UserId = overviewParameters.UserId;
                try
                {
                    if (!view.Validate())
                        return;

                    ActionCatalog.Execute(ActionNames.RunFinishPackingWorkflow, context, caller, actionParameters);

                    // Check if view should be closed.
                    bool ignoreClosing = ((context.Items.Get("IgnoreClose") != null) &&
                                        ((bool)(context.Items.Get("IgnoreClose"))));

                    overviewPresenter.ToLoadCarrierId = string.Empty;
                    overviewPresenter.ClearAllButPackedBy();
                }
                catch (Imi.SupplyChain.UX.ValidationException ex)
                {
                    view.HandleValidationResult(ValidationHelper.ConvertToResult((Imi.SupplyChain.UX.ValidationException)ex));
                }
            }
        }

        public override void OnPackStationOverviewRunStopPacking(WorkItem context, object caller, object target)
        {
            WorkItem wi = GetModuleWorkItem(context);

            if (wi.Items.FindByType<RunStopPackingAction>().Count == 0)
                wi.Items.AddNew<RunStopPackingAction>();

            PackStationOverviewViewParameters viewParameters = null;
            RunStopPackingActionParameters actionParameters = null;
            PackStationOverviewViewToRunStopPackingActionTranslator translator = null;
            bool isItemSelected = false;

            if (context.Items.FindByType<PackStationOverviewViewToRunStopPackingActionTranslator>().Count > 0)
                translator = context.Items.FindByType<PackStationOverviewViewToRunStopPackingActionTranslator>().Last();
            else
                translator = context.Items.AddNew<PackStationOverviewViewToRunStopPackingActionTranslator>();

            if (context.Items.FindByType<PackStationOverviewViewParameters>().Count() > 0)
            {
                viewParameters = context.Items.FindByType<PackStationOverviewViewParameters>().Last();
                isItemSelected = true;
            }
            else
            {
                viewParameters = new PackStationOverviewViewParameters();



            }


            actionParameters = translator.Translate(viewParameters);
            actionParameters.IsItemSelected = isItemSelected;
            actionParameters.IsMultipleItemsSelected = false;

            IPackStationOverviewView view = context.SmartParts.FindByType<PackStationOverviewView>().Last();
            PackStationOverviewPresenter overviewPresenter = context.Items.FindByType<PackStationOverviewPresenter>().Last();

            try
            {
                if (!view.Validate())
                    return;

                ActionCatalog.Execute(ActionNames.RunStopPacking, context, caller, actionParameters);

                // Check if view should be closed.
                bool ignoreClosing = ((context.Items.Get("IgnoreClose") != null) &&
                                    ((bool)(context.Items.Get("IgnoreClose"))));

                overviewPresenter.ClearAllButPackedBy();
            }
            catch (Imi.SupplyChain.UX.ValidationException ex)
            {
                view.HandleValidationResult(ValidationHelper.ConvertToResult((Imi.SupplyChain.UX.ValidationException)ex));
            }
        }


        /* Proxy action for PackStationFromLcEditRunPackRowForPackStation */

        public string PackStationOverviewRunPackRowForPackStation
        {
            get
            {
                return packStationOverviewRunPackRowForPackStation;
            }
        }

        protected string packStationOverviewRunPackRowForPackStation = "action://Imi.SupplyChain.Warehouse.UX.Modules.PackStation.PackStation/PackStationOverviewRunPackRowForPackStation";

        public virtual void OnPackStationOverviewRunPackRowForPackStation(WorkItem context, object caller, object target)
        {

            if (target is Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier.PackStationOverviewPackRowEventArgs)
            {
                WorkItem wi = GetModuleWorkItem(context);

                if (wi.Items.FindByType<RunPackRowForPackStationAction>().Count == 0)
                    wi.Items.AddNew<RunPackRowForPackStationAction>();

                Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier.PackStationOverviewPackRowEventArgs args = target as Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier.PackStationOverviewPackRowEventArgs;

                RunPackRowForPackStationActionParameters actionParameters = new RunPackRowForPackStationActionParameters();
                actionParameters.FromLoadCarrierId = args.FromLoadCarrier;
                actionParameters.ToLoadCarrierId = args.ToLoadCarrier;
                actionParameters.PickOrderLineNumber = args.PickOrderLineNumber;
                actionParameters.PickedQuantity = args.QuantityToPack;
                actionParameters.IsItemSelected = true;

                IPackStationOverviewView packStationOverviewView = context.SmartParts.FindByType<IPackStationOverviewView>().LastOrDefault();
                IPackStationFromLCView packStationFromLcView = context.SmartParts.FindByType<IPackStationFromLCView>().LastOrDefault();
                IPackStationToLCView packStationToLcView = context.SmartParts.FindByType<IPackStationToLCView>().LastOrDefault();

                try
                {
                    ActionCatalog.Execute(ActionNames.RunPackRowForPackStation, context, caller, actionParameters);

                    if (context.Items.Contains("RefreshPackStationOverview"))
                    {
                        if (((bool)context.Items["RefreshPackStationOverview"]))
                        {
                            packStationOverviewView.Refresh();
                        }
                        else
                        {
                            if (packStationFromLcView != null)
                            {
                                packStationFromLcView.Refresh();
                            }

                            if (packStationToLcView != null)
                            {
                                packStationToLcView.Refresh();
                            }
                        }
                    }
                    else
                    {
                        if (packStationFromLcView != null)
                        {
                            packStationFromLcView.Refresh();
                        }

                        if (packStationToLcView != null)
                        {
                            packStationToLcView.Refresh();
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }
        }


        public override void OnPackStationOverviewRunUndoPacking(WorkItem context, object caller, object target)
        {
            WorkItem wi = GetModuleWorkItem(context);

            if (wi.Items.FindByType<RunUndoPackingAction>().Count == 0)
                wi.Items.AddNew<RunUndoPackingAction>();

            RunUndoPackingActionParameters actionParameters = null;
            PackStationOverviewViewToRunUndoPackingActionTranslator translator = null;

            if (context.Items.FindByType<PackStationOverviewViewToRunUndoPackingActionTranslator>().Count > 0)
                translator = context.Items.FindByType<PackStationOverviewViewToRunUndoPackingActionTranslator>().Last();
            else
                translator = context.Items.AddNew<PackStationOverviewViewToRunUndoPackingActionTranslator>();

            PackStationOverviewPresenter overviewPresenter = null;

            if (context.Items.FindByType<PackStationOverviewPresenter>().Count > 0)
            {
                overviewPresenter = context.Items.FindByType<PackStationOverviewPresenter>().Last();

            }
            if (overviewPresenter == null)
            {
                return;
            }

            actionParameters = translator.Translate(overviewPresenter.ToLoadCarrierId);
            actionParameters.IsItemSelected = true;
            actionParameters.IsMultipleItemsSelected = false;

            IPackStationOverviewView view = context.SmartParts.FindByType<PackStationOverviewView>().Last();

            try
            {
                ActionCatalog.Execute(ActionNames.RunUndoPacking, context, caller, actionParameters);

                // Check if view should be closed.
                bool ignoreClosing = ((context.Items.Get("IgnoreClose") != null) &&
                                    ((bool)(context.Items.Get("IgnoreClose"))));

                overviewPresenter.ClearAllButPackedBy();
            }
            catch (Imi.SupplyChain.UX.ValidationException ex)
            {
                view.HandleValidationResult(ValidationHelper.ConvertToResult((Imi.SupplyChain.UX.ValidationException)ex));
            }
        }

        public override void OnBuiltUp(string id)
        {
            base.OnBuiltUp(id);

            packStationOverviewRunPackRowForPackStation += "/" + id;
            ActionCatalog.RegisterActionImplementation(packStationOverviewRunPackRowForPackStation, OnPackStationOverviewRunPackRowForPackStation);
            //ActionCatalog.RegisterSpecificCondition(packStationFromLcEditRunPackRowForPackStation, new PackStationFromLcEditRunPackRowForPackStationCondition());
        }
    }
}
