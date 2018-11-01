// Generated from template: .\UX\View\PresenterTemplate.cst

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Services;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using Imi.Framework.Wpf.Controls;
using Imi.Framework.Services;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Services;
using Imi.SupplyChain.UX.Modules.Warehouse.Infrastructure.Services;
using Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier.Constants;
using System.ServiceModel;
using Imi.SupplyChain.Warehouse.UX.Contracts.PickLoadCarrier.ServiceContracts;
using Imi.SupplyChain.Warehouse.UX.Contracts.PickLoadCarrier.DataContracts;


namespace Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier
{
    public class PackStationOverviewPackRowEventArgs : EventArgs
    {
        public string ToLoadCarrier;
        public string FromLoadCarrier;
        public string PickOrderLineNumber;
        public decimal QuantityToPack;
    }

    public class PackStationOverviewPresenter : PackStationOverviewPresenterBase
    {
        private string toLoadCarrierId;
        public string ToLoadCarrierId
        {
            get
            {
                return toLoadCarrierId;
            }
            set
            {
                toLoadCarrierId = value;
                View.ToLoadCarrierId = toLoadCarrierId;
                if (!string.IsNullOrEmpty(value))
                {
                    if (viewParameters != null)
                    {
                        ExecuteSearch(viewParameters);
                    }
                }
                else
                {
                    if (WorkItem.Items.FindByType<IPackStationToLCView>().Count > 0)
                    {
                        PackStationToLCViewParameters parameters = new PackStationToLCViewParameters();
                        parameters.LoadCarrierId = value;
                        WorkItem.Items.FindByType<IPackStationToLCView>().Last().Update(parameters);
                    }

                    ScanEachPickPackage = false;
                    MaxNoOfScans = 0;
                }
            }
        }


        public bool ScanEachPickPackage { get; private set; }
        public int MaxNoOfScans { get; private set; }

        [EventPublication(EventTopicNames.PackStationOverviewStopPackingTopic, PublicationScope.WorkItem)]
        public event EventHandler StopPacking;

        [EventPublication(EventTopicNames.PackStationOverviewPackRowTopic, PublicationScope.WorkItem)]
        public event EventHandler PackRow;

        [InjectionConstructor]
        public PackStationOverviewPresenter([WcfServiceDependency] IPickLoadCarrierService pickLoadCarrierService)
            : base(pickLoadCarrierService)
        {
        }

        public override void OnViewUpdated()
        {
            if (View.GetData != null)
            {
                OnViewUpdated(View.GetData);
            }
            else
            {
                OnViewUpdated(new List<PackStationOverviewViewResult>());
            }
        }

        public void ShowComboDialog(IWorkspace workspace, bool fromLoadCarrier)
        {
            string workItemName;
            if (fromLoadCarrier)
            {
                workItemName = "FromLoadCarrierId";
            }
            else
            {
                workItemName = "ToLoadCarrierId";
            }

            if (WorkItem.WorkItems[workItemName] != null)
                WorkItem.WorkItems[workItemName].Terminate();
            FindLoadCarrierForPackStationFindViewParameters parameters = new FindLoadCarrierForPackStationFindViewParameters();
            if (viewParameters != null)
            {
                if (string.IsNullOrEmpty(viewParameters.DepartureId))
                {
                    parameters.DepartureId = "%";
                }
                else
                {
                    parameters.DepartureId = viewParameters.DepartureId;
                }
                if (string.IsNullOrEmpty(viewParameters.ShipToCustomerId))
                {
                    parameters.ShipToCustomerId = "%";
                }
                else
                {
                    parameters.ShipToCustomerId = viewParameters.ShipToCustomerId;
                }
                parameters.PackedBy = viewParameters.UserId;
                if (fromLoadCarrier)
                {
                    parameters.PickRowEmptyStatus = "NOTEMPTY";
                }
                else
                {
                    parameters.PickRowEmptyStatus = "EMPTY";
                }
            }
            else
            {
                parameters.DepartureId = "%";
                parameters.ShipToCustomerId = "%";
                parameters.PackedBy = WorkItem.Items.FindByType<PackStationSearchPanelView>().Last().CurrentItem.UserId;
                if (fromLoadCarrier)
                {
                    parameters.PickRowEmptyStatus = "NOTEMPTY";
                }
                else
                {
                    parameters.PickRowEmptyStatus = "EMPTY";
                }
            }

            parameters.WarehouseId = UserSessionService.WarehouseId;
            parameters.ClientId = UserSessionService.ClientId;

            ShellInteractionService.ShowProgress();
            try
            {
                Assembly assembly = Assembly.Load("Imi.SupplyChain.Warehouse.UX.Modules.PackStation");
                IModuleLoaderService moduleLoaderService = WorkItem.Services.Get<IModuleLoaderService>();
                moduleLoaderService.Load(WorkItem.RootWorkItem.WorkItems["Warehouse"], assembly);
                Type workItemType = typeof(ControlledWorkItem<>).MakeGenericType(assembly.GetType("Imi.SupplyChain.Warehouse.UX.Modules.PackStation.FindLoadCarrierForPackStationFind.FindLoadCarrierForPackStationFindController"));
                object workItem = WorkItem.WorkItems.AddNew(workItemType, workItemName);
                object controller = workItem.GetType().GetProperty("Controller").GetValue(workItem, null);
                controller.GetType().InvokeMember("Run", BindingFlags.InvokeMethod, null, controller, new object[] { workspace, parameters });
            }
            finally
            {
                ShellInteractionService.HideProgress();
            }
        }

        public override void OnViewUpdated(IList<PackStationOverviewViewResult> viewResults)
        {
            foreach (IList<PackStationOverviewViewResult> item in WorkItem.Items.FindByType<IList<PackStationOverviewViewResult>>())
                WorkItem.Items.Remove(item);

            foreach (PackStationOverviewViewResult item in WorkItem.Items.FindByType<PackStationOverviewViewResult>())
                WorkItem.Items.Remove(item);

            if (currentItem != null)
                currentItem.PropertyChanged -= PropertyChangedEventHandler;

            currentItem = null;

            if ((viewResults != null) && (viewResults.Count > 0))
            {
                currentItem = viewResults.Last();
                WorkItem.Items.Add(currentItem);
                WorkItem.Items.Add(viewResults);
                if (viewParameters != null)
                {
                    if (!string.IsNullOrEmpty(viewParameters.PickZoneId))
                    {
                        View.EnableComponent("NewLoadCarrierLink", false);
                    }
                    else if (!string.IsNullOrEmpty(viewParameters.UserId) && !string.IsNullOrEmpty(viewParameters.DepartureId) && !string.IsNullOrEmpty(viewParameters.ShipToCustomerId))
                    {
                        View.EnableComponent("NewLoadCarrierLink", true);
                    }
                    else
                    {
                        View.EnableComponent("NewLoadCarrierLink", false);
                    }
                }
                else
                {
                    View.EnableComponent("NewLoadCarrierLink", false);
                }
            }
            else
            {
                View.EnableComponent("NewLoadCarrierLink", false);
            }

            InitializeRules(currentItem);

            if (currentItem != null)
                currentItem.PropertyChanged += PropertyChangedEventHandler;

            //UpdateDataSources(currentItem);

            ActionProviderService.UpdateActions(View);

            RaiseViewUpdated(currentItem);
        }

        protected override void PresentData(object data)
        {
            IPackStationSearchPanelView searchPanel = null;

            if (WorkItem.Items.FindByType<IPackStationSearchPanelView>().Count > 0)
            {
                searchPanel = WorkItem.Items.FindByType<IPackStationSearchPanelView>().Last();
            }

            if (searchPanel != null)
            {
                if ((data != null && ((IEnumerable<PackStationOverviewViewResult>)data).Count() > 0))
                {
                    //Start Set To Load Carrier if Pack & Sort
                    if (!string.IsNullOrEmpty(viewParameters.PickZoneId) && !string.IsNullOrEmpty(viewParameters.UserId) && string.IsNullOrEmpty(toLoadCarrierId))
                    {
                        if (((IEnumerable<PackStationOverviewViewResult>)data).Where(d => !d.Selected.Value).Count() == 0)
                        {
                            FindAndSetToLoadCarrierToPackAndSort(viewParameters.PickZoneId, viewParameters.UserId);
                        }
                    }


                    if (!string.IsNullOrEmpty(viewParameters.UserId))
                    {
                        searchPanel.EnableComponent("UserId", false);
                    }
                    else
                    {
                        searchPanel.EnableComponent("UserId", true);
                    }

                    if (!string.IsNullOrEmpty(viewParameters.DepartureId))
                    {
                        searchPanel.EnableComponent("DepartureId", false);
                    }
                    else
                    {
                        searchPanel.EnableComponent("DepartureId", true);
                    }

                    if (!string.IsNullOrEmpty(viewParameters.ShipToCustomerId))
                    {
                        searchPanel.EnableComponent("ShipToCustomerId", false);
                    }
                    else
                    {
                        searchPanel.EnableComponent("ShipToCustomerId", true);
                    }

                    if (!string.IsNullOrEmpty(viewParameters.PickZoneId))
                    {
                        searchPanel.EnableComponent("PickZoneId", false);
                        View.EnableComponent("ToLoadCarrier", false);
                    }
                    else
                    {
                        searchPanel.EnableComponent("PickZoneId", true);
                    }

                    
                }
                else
                {
                    if (string.IsNullOrEmpty(searchPanel.CurrentItem.PickZoneId) && string.IsNullOrEmpty(searchPanel.CurrentItem.DepartureId) && string.IsNullOrEmpty(searchPanel.CurrentItem.ShipToCustomerId))
                    {
                        searchPanel.EnableComponent("DepartureId", true);
                        searchPanel.EnableComponent("ShipToCustomerId", true);
                        searchPanel.EnableComponent("UserId", true);
                        searchPanel.EnableComponent("PickZoneId", true);
                        View.EnableComponent("ToLoadCarrier", true);
                        View.EnableComponent("FromLoadCarrier", true);
                        ToLoadCarrierId = string.Empty;
                    }
                    else if (string.IsNullOrEmpty(viewParameters.PickZoneId) && !string.IsNullOrEmpty(viewParameters.DepartureId) && !string.IsNullOrEmpty(viewParameters.ShipToCustomerId) && !string.IsNullOrEmpty(viewParameters.UserId) && !string.IsNullOrEmpty(toLoadCarrierId))
                    {
                        searchPanel.EnableComponent("DepartureId", false);
                        searchPanel.EnableComponent("ShipToCustomerId", false);
                        searchPanel.EnableComponent("PickZoneId", false);
                    }
                }
            }

            View.PresentData(data);

            if (drillDownItem != null)
            {
                View.CurrentItem = drillDownItem;
                drillDownItem = null;
            }
        }

        private void FindAndSetToLoadCarrierToPackAndSort(string pickZoneId, string empId)
        {
            GetToLoadCarrierPackAndSortResponse serviceResponse = null;
            GetToLoadCarrierPackAndSortRequest serviceRequest = new GetToLoadCarrierPackAndSortRequest();
            serviceRequest.GetToLoadCarrierPackAndSortParameters = new GetToLoadCarrierPackAndSortParameters();
            serviceRequest.GetToLoadCarrierPackAndSortParameters.PickZoneId = pickZoneId;
            serviceRequest.GetToLoadCarrierPackAndSortParameters.UserId = empId;
            serviceRequest.GetToLoadCarrierPackAndSortParameters.WarehouseId = UserSessionService.WarehouseId;
            serviceResponse = Service.GetToLoadCarrierPackAndSort(serviceRequest);
            if ((serviceResponse != null) && (serviceResponse.GetToLoadCarrierPackAndSortResult != null))
            {
                ToLoadCarrierId = serviceResponse.GetToLoadCarrierPackAndSortResult.LoadCarrierId;

                View.EnableComponent("ToLoadCarrier", false);
                View.EnableComponent("FromLoadCarrier", false);

                PackStationSearchPanelView searchPanel = null;
                if (WorkItem.Items.FindByType<PackStationSearchPanelView>().Count > 0)
                {
                    searchPanel = WorkItem.Items.FindByType<PackStationSearchPanelView>().Last();
                }

                if (searchPanel != null)
                {
                    searchPanel.DepartureId.Text = serviceResponse.GetToLoadCarrierPackAndSortResult.DepartureId;
                    searchPanel.ShipToCustomerId.Text = serviceResponse.GetToLoadCarrierPackAndSortResult.ShipToCustomerId;
                    searchPanel.UserId.Text = empId;
                    PackStationSearchPanelViewResult searchPanelResult = new PackStationSearchPanelViewResult();
                    searchPanelResult.DepartureId = serviceResponse.GetToLoadCarrierPackAndSortResult.DepartureId;
                    searchPanelResult.ShipToCustomerId = serviceResponse.GetToLoadCarrierPackAndSortResult.ShipToCustomerId;
                    searchPanelResult.UserId = empId;
                    searchPanelResult.PickZoneId = pickZoneId;
                    searchPanel.PresentData(searchPanelResult);
                    viewParameters.DepartureId = serviceResponse.GetToLoadCarrierPackAndSortResult.DepartureId;
                    viewParameters.ClientId = UserSessionService.ClientId;
                    viewParameters.UserId = empId;
                    viewParameters.ShipToCustomerId = serviceResponse.GetToLoadCarrierPackAndSortResult.ShipToCustomerId;
                    viewParameters.PickZoneId = pickZoneId;
                }

                View.SetFocus();
            }
        }

        public bool SelectFromLoadCarrier(string loadCarrierId)
        {
            PackStationSearchPanelView searchPanel = null;
            if (WorkItem.Items.FindByType<PackStationSearchPanelView>().Count > 0)
            {
                searchPanel = WorkItem.Items.FindByType<PackStationSearchPanelView>().Last();
            }
            string customerId = string.Empty;
            string departureId = string.Empty;
            string empId = string.Empty;
            bool firstCarId = false;

            string shipToCustomerId = string.Empty;
            string pickZoneId = string.Empty;

            if (viewParameters != null)
            {
                if (!string.IsNullOrEmpty(viewParameters.DepartureId))
                {
                    departureId = viewParameters.DepartureId;
                }
                if (!string.IsNullOrEmpty(viewParameters.ShipToCustomerId))
                {
                    customerId = viewParameters.ShipToCustomerId;
                }
                if (!string.IsNullOrEmpty(viewParameters.UserId))
                {
                    empId = viewParameters.UserId;
                }
                if (!string.IsNullOrEmpty(viewParameters.PickZoneId))
                {
                    pickZoneId = viewParameters.PickZoneId;
                }
            }

            if (searchPanel != null)
            {
                if (string.IsNullOrEmpty(empId))
                {
                    empId = searchPanel.CurrentItem.UserId;
                    if (viewParameters != null)
                    {
                        viewParameters.UserId = empId;
                    }
                }

                if (string.IsNullOrEmpty(pickZoneId))
                {
                    pickZoneId = searchPanel.CurrentItem.PickZoneId;
                }
            }

            if (string.IsNullOrEmpty(departureId) || string.IsNullOrEmpty(customerId))
            {
                firstCarId = true;
            }


            if (firstCarId && !string.IsNullOrEmpty(pickZoneId) && string.IsNullOrEmpty(toLoadCarrierId)) //Start Pack & Sort
            {
                StartPackAndSortCarResponse serviceResponse = null;
                StartPackAndSortCarRequest serviceRequest = new StartPackAndSortCarRequest();
                serviceRequest.StartPackAndSortCarParameters = new StartPackAndSortCarParameters();
                serviceRequest.StartPackAndSortCarParameters.UserId = empId;
                serviceRequest.StartPackAndSortCarParameters.LoadCarrierId = loadCarrierId;
                serviceResponse = Service.StartPackAndSortCar(serviceRequest);
                if ((serviceResponse == null) || (serviceResponse.StartPackAndSortCarResult == null))
                {
                    return false;
                }
                else
                {
                    departureId = serviceResponse.StartPackAndSortCarResult.DepartureId;
                    shipToCustomerId = serviceResponse.StartPackAndSortCarResult.ShipToCustomerId;

                    ToLoadCarrierId = loadCarrierId;
                    View.EnableComponent("ToLoadCarrier", false);
                    View.EnableComponent("FromLoadCarrier", false);
                }
            }
            else
            {
                SelectFromLoadCarrierForPackStationResponse serviceResponse = null;
                try
                {
                    SelectFromLoadCarrierForPackStationRequest serviceRequest = new SelectFromLoadCarrierForPackStationRequest();
                    serviceRequest.SelectFromLoadCarrierForPackStationParameters = new SelectFromLoadCarrierForPackStationParameters();
                    serviceRequest.SelectFromLoadCarrierForPackStationParameters.ClientId = UserSessionService.ClientId;
                    serviceRequest.SelectFromLoadCarrierForPackStationParameters.DepartureId = departureId;
                    serviceRequest.SelectFromLoadCarrierForPackStationParameters.LoadCarrierId = loadCarrierId;
                    serviceRequest.SelectFromLoadCarrierForPackStationParameters.ShipToCustomerId = customerId;
                    serviceRequest.SelectFromLoadCarrierForPackStationParameters.UserId = empId;
                    serviceRequest.SelectFromLoadCarrierForPackStationParameters.WarehouseId = UserSessionService.WarehouseId;
                    serviceResponse = Service.SelectFromLoadCarrierForPackStation(serviceRequest);
                    if ((serviceResponse == null) || (serviceResponse.SelectFromLoadCarrierForPackStationResult == null))
                    {
                        return false;
                    }
                    else
                    {
                        departureId = serviceResponse.SelectFromLoadCarrierForPackStationResult.DepartureId;
                        shipToCustomerId = serviceResponse.SelectFromLoadCarrierForPackStationResult.ShipToCustomerId;
                    }
                }
                catch (FaultException<ApplicationFault> ex)
                {
                    if (ex.Detail.ErrorCode == "PBCAR078" || ex.Detail.ErrorCode == "PBCAR048")
                    {

                        if (ShellInteractionService.ShowMessageBox(ResourceManager.str_ResetPackStatio_Title, GetAlarmText("PACK001"), null, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            if (StopPacking != null)
                            {
                                StopPacking(this, null);
                            }
                            viewParameters.DepartureId = string.Empty;
                            viewParameters.ShipToCustomerId = string.Empty;
                            ToLoadCarrierId = string.Empty;
                            SelectFromLoadCarrier(loadCarrierId);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (ex.Detail.ErrorCode == "PBCAR083")
                    {
                        if (ShellInteractionService.ShowMessageBox(StringResources.ActionException_Text, ex.Message, null, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            ForceSelectFromLoadCarrierForPackStationRequest forceSelectServiceRequest = new ForceSelectFromLoadCarrierForPackStationRequest();
                            forceSelectServiceRequest.ForceSelectFromLoadCarrierForPackStationParameters = new ForceSelectFromLoadCarrierForPackStationParameters();
                            forceSelectServiceRequest.ForceSelectFromLoadCarrierForPackStationParameters.LoadCarrierId = loadCarrierId;
                            forceSelectServiceRequest.ForceSelectFromLoadCarrierForPackStationParameters.UserId = empId;
                            ForceSelectFromLoadCarrierForPackStationResponse forceResponce = Service.ForceSelectFromLoadCarrierForPackStation(forceSelectServiceRequest);
                            if ((forceResponce == null) || (forceResponce.ForceSelectFromLoadCarrierForPackStationResult == null))
                            {
                                return false;
                            }
                            else
                            {
                                departureId = forceResponce.ForceSelectFromLoadCarrierForPackStationResult.DepartureId;
                                shipToCustomerId = forceResponce.ForceSelectFromLoadCarrierForPackStationResult.ShipToCustomerId;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (firstCarId)
                        {
                            if (viewParameters != null)
                            {
                                viewParameters.UserId = string.Empty;
                            }
                        }
                        ShellInteractionService.ShowMessageBox(StringResources.ActionException_Text, ex.Message, null, MessageBoxButton.Ok, MessageBoxImage.Error);
                        return false;
                    }
                }
            }

            if (firstCarId)
            {
                if (searchPanel != null)
                {
                    searchPanel.DepartureId.Text = departureId;
                    searchPanel.ShipToCustomerId.Text = shipToCustomerId;
                    searchPanel.UserId.Text = empId;
                    PackStationSearchPanelViewResult searchPanelResult = new PackStationSearchPanelViewResult();
                    searchPanelResult.DepartureId = departureId;
                    searchPanelResult.ShipToCustomerId = shipToCustomerId;
                    searchPanelResult.UserId = empId;
                    searchPanelResult.PickZoneId = pickZoneId;
                    searchPanel.PresentData(searchPanelResult);
                    PackStationOverviewViewParameters parameters = new PackStationOverviewViewParameters();
                    parameters.DepartureId = departureId;
                    parameters.ClientId = UserSessionService.ClientId;
                    parameters.UserId = empId;
                    parameters.ShipToCustomerId = shipToCustomerId;
                    parameters.PickZoneId = pickZoneId;
                    View.Update(parameters);
                }
            }
            else
            {
                View.Update(viewParameters); // View.OnViewUpdated();
            }
            return true;
        }



        public bool SelectToLoadCarrier(string loadCarrierId)
        {
            PackStationSearchPanelView searchPanel = null;
            if (WorkItem.Items.FindByType<PackStationSearchPanelView>().Count > 0)
            {
                searchPanel = WorkItem.Items.FindByType<PackStationSearchPanelView>().Last();
            }
            string customerId = string.Empty;
            string departureId = string.Empty;
            string empId = string.Empty;
            bool firstCarId = false;
            if (viewParameters != null)
            {
                if (!string.IsNullOrEmpty(viewParameters.DepartureId))
                {
                    departureId = viewParameters.DepartureId;
                }
                if (!string.IsNullOrEmpty(viewParameters.ShipToCustomerId))
                {
                    customerId = viewParameters.ShipToCustomerId;
                }
                if (!string.IsNullOrEmpty(viewParameters.UserId))
                {
                    empId = viewParameters.UserId;
                }
            }
            if (string.IsNullOrEmpty(empId))
            {
                if (searchPanel != null)
                {
                    empId = searchPanel.CurrentItem.UserId;
                }
            }
            if (string.IsNullOrEmpty(departureId) || string.IsNullOrEmpty(customerId))
            {
                firstCarId = true;
            }
            SelectToLoadCarrierForPackStationResponse serviceResponse = null;
            try
            {
                SelectToLoadCarrierForPackStationRequest serviceRequest = new SelectToLoadCarrierForPackStationRequest();
                serviceRequest.SelectToLoadCarrierForPackStationParameters = new SelectToLoadCarrierForPackStationParameters();
                serviceRequest.SelectToLoadCarrierForPackStationParameters.ClientId = UserSessionService.ClientId;
                serviceRequest.SelectToLoadCarrierForPackStationParameters.DepartureId = departureId;
                serviceRequest.SelectToLoadCarrierForPackStationParameters.LoadCarrierId = loadCarrierId;
                serviceRequest.SelectToLoadCarrierForPackStationParameters.ShipToCustomerId = customerId;
                serviceRequest.SelectToLoadCarrierForPackStationParameters.UserId = empId;
                serviceRequest.SelectToLoadCarrierForPackStationParameters.WarehouseId = UserSessionService.WarehouseId;
                serviceResponse = Service.SelectToLoadCarrierForPackStation(serviceRequest);
                if ((serviceResponse == null) || (serviceResponse.SelectToLoadCarrierForPackStationResult == null))
                {
                    return false;
                }
            }
            catch (FaultException<ApplicationFault> ex)
            {
                if (ex.Detail.ErrorCode == "PBCAR078" || ex.Detail.ErrorCode == "PBCAR048")
                {
                    if (ShellInteractionService.ShowMessageBox(ResourceManager.str_ResetPackStatio_Title, GetAlarmText("PACK001"), null, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if (StopPacking != null)
                        {
                            StopPacking(this, null);
                        }
                        viewParameters.DepartureId = string.Empty;
                        viewParameters.ShipToCustomerId = string.Empty;
                        SelectToLoadCarrier(loadCarrierId);
                        return true;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(ToLoadCarrierId))
                        {
                            ToLoadCarrierId = ToLoadCarrierId;
                        }
                        else
                        {
                            ToLoadCarrierId = string.Empty;
                        }
                        return false;
                    }
                }
                else
                {
                    if (firstCarId)
                    {
                        if (viewParameters != null)
                        {
                            viewParameters.UserId = string.Empty;
                        }
                    }
                    ToLoadCarrierId = string.Empty;
                    throw;
                }
            }
            if (firstCarId)
            {
                if (searchPanel != null)
                {
                    searchPanel.DepartureId.Text = serviceResponse.SelectToLoadCarrierForPackStationResult.DepartureId;
                    searchPanel.ShipToCustomerId.Text = serviceResponse.SelectToLoadCarrierForPackStationResult.ShipToCustomerId;
                    searchPanel.UserId.Text = empId;
                    PackStationSearchPanelViewResult searchPanelResult = new PackStationSearchPanelViewResult();
                    searchPanelResult.DepartureId = serviceResponse.SelectToLoadCarrierForPackStationResult.DepartureId;
                    searchPanelResult.ShipToCustomerId = serviceResponse.SelectToLoadCarrierForPackStationResult.ShipToCustomerId;
                    searchPanelResult.UserId = empId;
                    searchPanel.PresentData(searchPanelResult);
                    PackStationOverviewViewParameters parameters = new PackStationOverviewViewParameters();
                    parameters.DepartureId = serviceResponse.SelectToLoadCarrierForPackStationResult.DepartureId;
                    parameters.ClientId = UserSessionService.ClientId;
                    parameters.UserId = empId;
                    parameters.ShipToCustomerId = serviceResponse.SelectToLoadCarrierForPackStationResult.ShipToCustomerId;
                    viewParameters = parameters;
                }
            }

            ScanEachPickPackage = serviceResponse.SelectToLoadCarrierForPackStationResult.ScanEachPickPackage.Value;
            MaxNoOfScans = serviceResponse.SelectToLoadCarrierForPackStationResult.MaxNoOfScans.GetValueOrDefault(0);

            ToLoadCarrierId = loadCarrierId;
            return true;
        }
        public void ShowCreateLoadCarrierDialog()
        {
            if (viewParameters != null && currentItem != null)
            {
                CreateLoadCarrierForPackStationViewParameters loadCarrierParameters = new CreateLoadCarrierForPackStationViewParameters();
                loadCarrierParameters.ClientId = UserSessionService.ClientId;
                loadCarrierParameters.DepartureId = viewParameters.DepartureId;
                loadCarrierParameters.UserId = viewParameters.UserId;
                loadCarrierParameters.ShipToCustomerId = viewParameters.ShipToCustomerId;
                loadCarrierParameters.ShipToCustomerType = currentItem.ShipToCustomerType;
                ShellInteractionService.ShowProgress();
                try
                {
                    Assembly assembly = Assembly.Load("Imi.SupplyChain.Warehouse.UX.Modules.PackStation");
                    IModuleLoaderService moduleLoaderService = WorkItem.Services.Get<IModuleLoaderService>();
                    moduleLoaderService.Load(WorkItem.RootWorkItem.WorkItems["Warehouse"], assembly);
                    Type workItemType = typeof(ControlledWorkItem<>).MakeGenericType(assembly.GetType("Imi.SupplyChain.Warehouse.UX.Modules.PackStation.CreateLoadCarrierForPackStation.CreateLoadCarrierForPackStationController"));
                    WorkItem workItem = WorkItem.WorkItems.AddNew(workItemType, "CreateLoadCarrier");
                    workItem.Terminated += (s, e) =>
                    {
                        DialogResult dialogResult = DialogResult.None;
                        if (workItem.Items.Get("DialogResult") != null)
                            dialogResult = (DialogResult)workItem.Items.Get("DialogResult");
                        if ((dialogResult == DialogResult.Ok) &&
                            (workItem.Items.FindByType<CreateLoadCarrierForPackStationViewResult>().Count > 0))
                        {
                            CreateLoadCarrierForPackStationViewResult result = workItem.Items.FindByType<CreateLoadCarrierForPackStationViewResult>().Last();
                            try
                            {
                                SelectToLoadCarrier(result.LoadCarrierId);
                            }
                            catch (Exception ex)
                            {
                                ShellInteractionService.ShowMessageBox(ex);
                            }
                        }
                    };
                    object controller = workItem.GetType().GetProperty("Controller").GetValue(workItem, null);
                    controller.GetType().InvokeMember("Run", BindingFlags.InvokeMethod, null, controller, new object[] { loadCarrierParameters });
                }
                finally
                {
                    ShellInteractionService.HideProgress();
                }
            }
        }

        public void ClearAllButPackedBy()
        {
            PackStationSearchPanelView searchPanelView = null;
            if (WorkItem.Items.FindByType<PackStationSearchPanelView>().Count > 0)
            {
                searchPanelView = WorkItem.Items.FindByType<PackStationSearchPanelView>().Last();
            }
            if (searchPanelView != null)
            {
                ToLoadCarrierId = string.Empty;
                searchPanelView.CurrentItem.DepartureId = null;
                searchPanelView.CurrentItem.ShipToCustomerId = null;
                searchPanelView.DepartureId.Text = string.Empty;
                searchPanelView.ShipToCustomerId.Text = string.Empty;
                PackStationSearchPanelViewResult searchPanelResult = new PackStationSearchPanelViewResult();
                searchPanelResult.DepartureId = string.Empty;
                searchPanelResult.ShipToCustomerId = string.Empty;
                searchPanelResult.UserId = searchPanelView.UserId.Text;
                searchPanelResult.PickZoneId = searchPanelView.CurrentItem.PickZoneId;
                searchPanelView.Update(searchPanelResult);
                PackStationOverviewViewParameters parameters = new PackStationOverviewViewParameters();
                parameters.DepartureId = string.Empty;
                parameters.ClientId = UserSessionService.ClientId;
                parameters.UserId = searchPanelView.UserId.Text;
                parameters.ShipToCustomerId = string.Empty;
                parameters.PickZoneId = searchPanelView.CurrentItem.PickZoneId;

                View.EnableComponent("ToLoadCarrier", true);
                View.EnableComponent("FromLoadCarrier", true);

                View.Update(parameters);
            }
        }

        protected List<string> UpdateDataSourceFindArtIdFromEAN(string eanCode)
        {
            bool isUpdating = updatingList.Contains("FindArtIdFromEAN");
            List<string> foundProductNumbers = new List<string>();

            if (!isUpdating)
            {
                updatingList.Add("FindArtIdFromEAN");

                ShellInteractionService.ShowProgress();

                try
                {
                    Imi.SupplyChain.Warehouse.UX.Contracts.Ean.ServiceContracts.FindArtIdFromEANRequest serviceRequest = new Imi.SupplyChain.Warehouse.UX.Contracts.Ean.ServiceContracts.FindArtIdFromEANRequest();
                    serviceRequest.FindArtIdFromEANParameters = new Imi.SupplyChain.Warehouse.UX.Contracts.Ean.DataContracts.FindArtIdFromEANParameters();

                    serviceRequest.FindArtIdFromEANParameters.BarCodeNumber = eanCode;
                    serviceRequest.FindArtIdFromEANParameters.ClientId = UserSessionService.ClientId;

                    Imi.SupplyChain.Warehouse.UX.Contracts.Ean.ServiceContracts.FindArtIdFromEANResponse serviceResponse = EanService.FindArtIdFromEAN(serviceRequest);

                    if (serviceResponse != null &&
                        serviceResponse.FindArtIdFromEANResultCollection != null &&
                        serviceResponse.FindArtIdFromEANResultCollection.Count > 0)
                    {

                        foreach (Imi.SupplyChain.Warehouse.UX.Contracts.Ean.DataContracts.FindArtIdFromEANResult result in serviceResponse.FindArtIdFromEANResultCollection)
                        {
                            foundProductNumbers.Add(result.ProductNumber);
                        }

                    }

                }
                finally
                {
                    ShellInteractionService.HideProgress();
                    updatingList.Remove("FindArtIdFromEAN");
                }
            }

            return foundProductNumbers;

        }

        public void PackWithEAN(string eanCode)
        {
            if (!string.IsNullOrEmpty(ToLoadCarrierId))
            {
                if (WorkItem.Items.FindByType<IPackStationFromLCView>().Count > 0)
                {
                    IPackStationFromLCView detailView = WorkItem.Items.FindByType<IPackStationFromLCView>().LastOrDefault();
                    List<string> productNumbersFromEAN = UpdateDataSourceFindArtIdFromEAN(eanCode);

                    string productNumber = string.Empty;

                    if (productNumbersFromEAN.Count == 1)
                    {
                        productNumber = productNumbersFromEAN[0];
                    }
                    else
                    {
                        productNumber = eanCode;
                    }

                    //else
                    //{
                    //    Console.Beep(350, 1000);
                    //    if (productNumbersFromEAN.Count == 0)
                    //    {
                    //        ShellInteractionService.ShowMessageBox(ResourceManager.str_EANPack_Error_Title, GetAlarmText("PACK006"), string.Empty, MessageBoxButton.Ok, MessageBoxImage.Warning);
                    //    }
                    //    else
                    //    {
                    //        ShellInteractionService.ShowMessageBox(ResourceManager.str_EANPack_Error_Title, GetAlarmText("PACK005"), string.Empty, MessageBoxButton.Ok, MessageBoxImage.Warning);
                    //    }
                    //}

                    if (productNumber != string.Empty)
                    {
                        List<PackStationFromLCViewResult> pickOrderLines = detailView.GetData;

                        List<PackStationFromLCViewResult> foundProductLines = FindPickOrderLineWithProductNumber(pickOrderLines, productNumber);

                        if (foundProductLines.Count > 0)
                        {
                            if (foundProductLines.Count == 1)
                            {
                                PackStationFromLCViewResult foundProductLine = foundProductLines[0];

                                if (ScanEachPickPackage)
                                {
                                    if (foundProductLine.PickedQuantity <= MaxNoOfScans || !string.IsNullOrEmpty(viewParameters.PickZoneId)) //Override MaxNoOfScans if Pack & Sort
                                    {
                                        detailView.CurrentItem = foundProductLine;
                                        PackPickOrderLine(foundProductLine, 1);
                                    }
                                    else
                                    {

                                        Console.Beep(350, 1000);
                                        ShellInteractionService.ShowMessageBox(ResourceManager.str_EANPack_Error_Title, GetAlarmText("PACK002"), string.Empty, MessageBoxButton.Ok, MessageBoxImage.Warning);
                                        //Error message "The pick quantity is greater the max number of scans. Please pack the line manualy"

                                        detailView.CurrentItem = foundProductLine;
                                        detailView.Refresh();
                                        detailView.SetFocus();
                                    }
                                }
                                else
                                {
                                    PackPickOrderLine(foundProductLine, foundProductLine.PickedQuantity.Value);
                                }
                            }
                            else
                            {
                                Console.Beep(350, 1000);
                                ShellInteractionService.ShowMessageBox(ResourceManager.str_EANPack_Error_Title, GetAlarmText("PACK003"), string.Empty, MessageBoxButton.Ok, MessageBoxImage.Warning);
                                //Error message "The product can´t be packed with EAN since there are several pick order lines for this product"
                            }
                        }
                        else
                        {
                            Console.Beep(350, 1000);
                            ShellInteractionService.ShowMessageBox(ResourceManager.str_EANPack_Error_Title, GetAlarmText("PACK004"), string.Empty, MessageBoxButton.Ok, MessageBoxImage.Warning);
                            //Error messaga "No product corresponds to this EAN code"
                        }
                    }
                }
            }
            else //Start Pack & Sort
            {
                PackStationSearchPanelView searchPanel = null;
                if (WorkItem.Items.FindByType<PackStationSearchPanelView>().Count > 0)
                {
                    searchPanel = WorkItem.Items.FindByType<PackStationSearchPanelView>().Last();
                }

                string empId = string.Empty;
                string pickZoneId = string.Empty;

                if (viewParameters != null)
                {
                    if (!string.IsNullOrEmpty(viewParameters.PickZoneId))
                    {
                        pickZoneId = viewParameters.PickZoneId;
                    }

                    if (!string.IsNullOrEmpty(viewParameters.UserId))
                    {
                        empId = viewParameters.UserId;
                    }
                }

                if (searchPanel != null)
                {
                    if (string.IsNullOrEmpty(empId))
                    {
                        empId = searchPanel.CurrentItem.UserId;
                    }

                    if (string.IsNullOrEmpty(pickZoneId))
                    {
                        pickZoneId = searchPanel.CurrentItem.PickZoneId;
                    }
                }

                if (!string.IsNullOrEmpty(pickZoneId))
                {
                    if (!string.IsNullOrEmpty(empId))
                    {
                        ScanEachPickPackage = true;

                        StartPackAndSortArtResponse serviceResponse = null;
                        //try
                        //{
                        StartPackAndSortArtRequest serviceRequest = new StartPackAndSortArtRequest();
                        serviceRequest.StartPackAndSortArtParameters = new StartPackAndSortArtParameters();
                        serviceRequest.StartPackAndSortArtParameters.UserId = empId;
                        serviceRequest.StartPackAndSortArtParameters.PickZoneId = pickZoneId;
                        serviceRequest.StartPackAndSortArtParameters.BarCodeNumber = eanCode;
                        serviceRequest.StartPackAndSortArtParameters.WarehouseId = UserSessionService.WarehouseId;
                        serviceRequest.StartPackAndSortArtParameters.ClientId = UserSessionService.ClientId;
                        serviceResponse = Service.StartPackAndSortArt(serviceRequest);
                        if ((serviceResponse != null) && (serviceResponse.StartPackAndSortArtResult != null))
                        {
                            ToLoadCarrierId = serviceResponse.StartPackAndSortArtResult.LoadCarrierId;
                            View.EnableComponent("ToLoadCarrier", false);
                            View.EnableComponent("FromLoadCarrier", false);

                            if (searchPanel != null)
                            {
                                searchPanel.DepartureId.Text = serviceResponse.StartPackAndSortArtResult.DepartureId;
                                searchPanel.ShipToCustomerId.Text = serviceResponse.StartPackAndSortArtResult.ShipToCustomerId;
                                searchPanel.UserId.Text = empId;
                                PackStationSearchPanelViewResult searchPanelResult = new PackStationSearchPanelViewResult();
                                searchPanelResult.DepartureId = serviceResponse.StartPackAndSortArtResult.DepartureId;
                                searchPanelResult.ShipToCustomerId = serviceResponse.StartPackAndSortArtResult.ShipToCustomerId;
                                searchPanelResult.PickZoneId = pickZoneId;
                                searchPanelResult.UserId = empId;
                                searchPanel.PresentData(searchPanelResult);
                                PackStationOverviewViewParameters parameters = new PackStationOverviewViewParameters();
                                parameters.DepartureId = serviceResponse.StartPackAndSortArtResult.DepartureId;
                                parameters.ClientId = UserSessionService.ClientId;
                                parameters.PickZoneId = pickZoneId;
                                parameters.UserId = empId;
                                parameters.ShipToCustomerId = serviceResponse.StartPackAndSortArtResult.ShipToCustomerId;
                                View.Update(parameters);
                            }
                        }
                        //}
                        //catch (FaultException<ApplicationFault> ex)
                        //{
                        //    if (ShellInteractionService.ShowMessageBox(ResourceManager.str_ResetPackStatio_Title, GetAlarmText(ex.Detail.ErrorCode), null, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Ok)
                        //    {
                        //    }
                        //}
                    }
                }
                else
                {
                    Console.Beep(350, 1000);
                    ShellInteractionService.ShowMessageBox(ResourceManager.str_EANPack_Error_Title, GetAlarmText("PACK007"), string.Empty, MessageBoxButton.Ok, MessageBoxImage.Warning);
                }
            }
        }

        private void PackPickOrderLine(PackStationFromLCViewResult lineToPackFrom, decimal quantityToPack)
        {
            PackStationOverviewPackRowEventArgs args = new PackStationOverviewPackRowEventArgs();
            args.FromLoadCarrier = lineToPackFrom.LoadCarrierId;
            args.ToLoadCarrier = ToLoadCarrierId;
            args.PickOrderLineNumber = lineToPackFrom.PickOrderLineNumber;
            args.QuantityToPack = quantityToPack;

            if (PackRow != null)
            {
                PackRow(this, args);
            }
        }

        private List<PackStationFromLCViewResult> FindPickOrderLineWithProductNumber(IList<PackStationFromLCViewResult> viewResult, string productNumber)
        {
            List<PackStationFromLCViewResult> result = new List<PackStationFromLCViewResult>();

            if (viewResult != null)
            {
                result = viewResult.Where(i => i.ProductNumber == productNumber).ToList();
            }

            return result;
        }


        private string GetAlarmText(string alarmId)
        {
            Services.Alarm.ServiceContracts.FindAlarmTextResponse result = AlarmService.FindAlarmText(new Services.Alarm.ServiceContracts.FindAlarmTextRequest() { FindAlarmTextParams = new Services.Alarm.DataContracts.FindAlarmTextParams() { AlarmId = alarmId, LanguageCode = UserSessionService.LanguageCode } });
            if (result != null && result.FindAlarmTextResult != null)
            {
                return result.FindAlarmTextResult.AlarmText;
            }

            return string.Empty;
        }

    }
}
