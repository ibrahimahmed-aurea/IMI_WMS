using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX.Modules.GatewayMHS.Infrastructure.Services;
using Imi.SupplyChain.UX.Views;
using Imi.SupplyChain.UX.Services;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts;
using Imi.SupplyChain.Warehouse.Services.Authentication.ServiceContracts;
using Microsoft.Practices.CompositeUI.EventBroker;

namespace Imi.SupplyChain.UX.Modules.GatewayMHS.Views.ChooseDefaultWarehouse
{
    public class ChooseDefaultWarehousePresenter : AsyncDataPresenter<IAuthenticationService, IChooseDefaultWarehouseView>
    {
        private bool forceUpdate;
        private bool backendCallIsRunning = false;

        private FindUserDetailsParameters viewParameters;
                
        [InjectionConstructor]
        public ChooseDefaultWarehousePresenter([WcfServiceDependency] IAuthenticationService authenticationService)
            : base(authenticationService)
        {
        }

        [ServiceDependency]
        public IGatewayMHSUserSessionService UserSessionService
        {
            get;
            set;
        }

        public override void OnViewReady()
        {
            viewParameters = new FindUserDetailsParameters();
            forceUpdate = true;
        }

        public override void OnViewShow()
        {
            if ((View.RefreshDataOnShow) || (forceUpdate))
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
                forceUpdate = false;
                ExecuteSearch(viewParameters);
            }
            else
            {
                forceUpdate = true;
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

                backendCallIsRunning = true;

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
            backendCallIsRunning = false;
                        
            View.PresentData(data);

            if (data != null)
            {
                FindUserDetailsResult userDetails = data as FindUserDetailsResult;

                if (!string.IsNullOrEmpty(userDetails.RecentWarehouseIdentity))
                    View.SelectedWarehouseId = userDetails.RecentWarehouseIdentity;
            }

        }

        public void Close()
        {
            // Abort running call to backend
            if (backendCallIsRunning)
            {
                backendCallIsRunning = false;
                this.Cancel();
            }

            this.View.Close();
            WorkItem.SmartParts.Remove(this.View);
        }

        public void SelectAndClose(UserWarehouse selectedWarehouse)
        {
            if (selectedWarehouse != null)
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
                                Close();
                                return;
                            }
                        }
                    }

                    ShellInteractionService.ShowProgress();
                    
                    // Get the application
                    ModuleController module = WorkItem.Items.FindByType<ModuleController>().First();

                    LogonParameters logonParameters = new LogonParameters();
                    logonParameters.UserIdentity = UserSessionService.UserId;
                    logonParameters.WarehouseIdentity = selectedWarehouse.WarehouseIdentity;
                    logonParameters.TerminalIdentity = UserSessionService.TerminalId;
                    logonParameters.ApplicationIdentity = module.Id;

                    LogonRequest logonRequest = new LogonRequest();

                    logonRequest.LogonParameters = logonParameters;

                    Service.Logon(logonRequest);

                    UserSessionService.WarehouseId = selectedWarehouse.WarehouseIdentity;
                                        
                    if (userSettingsTopic != null)
                    {
                        UserSettingsChangedEventArgs userSettingsChangedEventArgs = new UserSettingsChangedEventArgs(true);
                        userSettingsTopic.Fire(this, userSettingsChangedEventArgs, WorkItem, PublicationScope.Descendants);
                    }

                    // Set the selected Warehouse and ClientId on statusrow in container
                    ShellInteractionService.ContextInfo = string.Format(LocalResources.STATUSBAR_WH,
                                                             selectedWarehouse.WarehouseIdentity,
                                                             selectedWarehouse.WarehouseName);

                    Close();
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
