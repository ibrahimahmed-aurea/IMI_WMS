using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX.Modules.ActivityMonitor.Infrastructure.Services;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts;
using Imi.SupplyChain.Warehouse.Services.Authentication.ServiceContracts;
using Imi.SupplyChain.UX.Infrastructure;
using Microsoft.Practices.CompositeUI.EventBroker;
using Imi.SupplyChain.UX.Modules.ActivityMonitor;

namespace Imi.SupplyChain.UX.Modules.ActivityMonitor.Views.ChooseDefaultWarehouse
{
    public class ChooseDefaultWarehousePresenter : AsyncDataPresenter<IAuthenticationService, IChooseDefaultWarehouseView>
    {
        private bool _forceUpdate;
        private bool _backendCallIsRunning = false;

        private FindUserDetailsParameters viewParameters;
                
        [InjectionConstructor]
        public ChooseDefaultWarehousePresenter([WcfServiceDependency] IAuthenticationService authenticationService)
            : base(authenticationService)
        {
        }

        [ServiceDependency]
        public IActivityMonitorUserSessionService UserSessionService
        {
            get;
            set;
        }

        public override void OnViewReady()
        {
            viewParameters = new FindUserDetailsParameters();
            _forceUpdate = true;
        }

        public override void OnViewShow()
        {
            if ((View.RefreshDataOnShow) || (_forceUpdate))
            {
                RefreshView();
            }
        }

        public void RefreshView()
        {
            UpdateView(viewParameters);
        }

        public void UpdateView(FindUserDetailsParameters viewParameters)
        {
            this.viewParameters = viewParameters;

            if (View.IsVisible)
            {
                _forceUpdate = false;
                ExecuteSearch(viewParameters);
            }
            else
            {
                _forceUpdate = true;
            }
        }


        protected override object ExecuteSearchAsync(object parameters)
        {
            object data = null;

            if ((parameters != null) && (parameters is FindUserDetailsParameters))
            {
                viewParameters = parameters as FindUserDetailsParameters;

                FindUserDetailsRequest serviceRequest = new FindUserDetailsRequest();

                serviceRequest.FindUserDetailsParameters = viewParameters;

                _backendCallIsRunning = true;

                FindUserDetailsResponse serviceResponse = Service.FindUserDetails(serviceRequest);

                if ((serviceResponse != null) && (serviceResponse.FindUserDetailsResult != null))
                {
                    data = serviceResponse.FindUserDetailsResult;
                    UserSessionService.UserId = serviceResponse.FindUserDetailsResult.UserIdentity;
                }
            }
                       

            return (data);
        }

        protected override void PresentData(object data)
        {
            _backendCallIsRunning = false;
                        
            View.PresentData(data);

            if (data != null)
            {
                FindUserDetailsResult userDetails = data as FindUserDetailsResult;

                if (!string.IsNullOrEmpty(userDetails.RecentWarehouseIdentity))
                    View.SelectedWarehouseId = userDetails.RecentWarehouseIdentity;
                if (!string.IsNullOrEmpty(userDetails.RecentCompanyIdentity))
                    View.SelectedClientId = userDetails.RecentCompanyIdentity;
            }

        }

        public void Close(bool result)
        {
            // Abort running call to backend
            if (_backendCallIsRunning)
            {
                _backendCallIsRunning = false;
                this.Cancel();
            }

            this.View.Close(result);
            WorkItem.SmartParts.Remove(this.View);
        }
                
        public void SelectAndClose(UserWarehouse selectedWarehouse, UserCompany selectedCompany)
        {
            if ((selectedWarehouse != null) && (selectedCompany != null))
            {
                try
                {
                    UserSettingsChangedEventArgs args = new UserSettingsChangedEventArgs();

                    EventTopic userSettingsTopic = WorkItem.EventTopics.Get(Imi.SupplyChain.UX.UXEventTopicNames.UserSettingsChangedTopic);
                    
                    if (userSettingsTopic != null)
                    {
                        userSettingsTopic.Fire(this, args, WorkItem, PublicationScope.Descendants);

                        if (args.OpenDialogs.Count > 0)
                        {
                            if (ShellInteractionService.ShowMessageBox(this.View.Title, string.Format(LocalResources.ChangeUserSettings_CloseAll, string.Join("\n", args.OpenDialogs)), null, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                            {
                                Close(false);
                                return;    
                            }
                        }
                    }

                    ShellInteractionService.ShowProgress();

                    // Get the application
                    IShellModule module = WorkItem.Items.FindByType<IShellModule>().First();

                    LogonParameters logonParameters = new LogonParameters();
                    logonParameters.UserIdentity = UserSessionService.UserId;
                    logonParameters.CompanyIdentity = selectedCompany.CompanyIdentity;
                    logonParameters.WarehouseIdentity = selectedCompany.WarehouseIdentity;
                    logonParameters.TerminalIdentity = UserSessionService.TerminalId;
                    logonParameters.ApplicationIdentity = module.Id;

                    LogonRequest logonRequest = new LogonRequest();

                    logonRequest.LogonParameters = logonParameters;

                    LogonResponse response = Service.Logon(logonRequest);

                    // Set the selected Warehouse and ClientId on statusrow in container
                    ShellInteractionService.ContextInfo = string.Format(LocalResources.STATUSBAR_WH_CLIENT,
                                                                 selectedWarehouse.WarehouseIdentity,
                                                                 selectedWarehouse.WarehouseName,
                                                                 selectedCompany.CompanyIdentity,
                                                                 selectedCompany.CompanyName);
                    Close(true);
                }
                catch (Exception ex)
                {
                    ShellInteractionService.HideProgress();
                    ShellInteractionService.ShowMessageBox(StringResources.ActionException_Text, ex.Message, ex.ToString(), MessageBoxButton.Ok, MessageBoxImage.Error);
                }
                finally
                {
                    ShellInteractionService.HideProgress();
                }
            }
        }
    }
}
