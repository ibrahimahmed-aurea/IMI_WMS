using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.UX;
using Microsoft.Practices.CompositeUI;
using Imi.Framework.UX.Services;
using Microsoft.Practices.ObjectBuilder;
using Imi.SupplyChain.UX.Modules.OutputManager.Infrastructure.Services;
using Imi.SupplyChain.UX.Views;
using Imi.SupplyChain.UX.Services;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.SupplyChain.OutputManager.Services.Initialization.DataContracts;
using Imi.SupplyChain.OutputManager.Services.Initialization.ServiceContracts;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.CompositeUI.EventBroker;


namespace Imi.SupplyChain.UX.Modules.OutputManager.Views.ChooseDefaultOutputManager
{
    public class ChooseDefaultOutputManagerPresenter : AsyncDataPresenter<IInitializationService, IChooseDefaultOutputManagerView>
    {
        private bool _forceUpdate;
        private bool _backendCallIsRunning = false;

        private FindOutputManagersParameters viewParameters;
                
        [InjectionConstructor]
        public ChooseDefaultOutputManagerPresenter([WcfServiceDependency] IInitializationService authenticationService)
            : base(authenticationService)
        {
        }

        [ServiceDependency]
        public IOutputManagerUserSessionService OutputManagerSessionService
        {
            get;
            set;
        }

        public override void OnViewReady()
        {
            viewParameters = null; //new FindUserDetailsParameters();
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

        public void UpdateView(FindOutputManagersParameters viewParameters)
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

            
                viewParameters = parameters as FindOutputManagersParameters;


                FindOutputManagersRequest serviceRequest = new FindOutputManagersRequest();

                serviceRequest.FindOutputManagersParameters = new FindOutputManagersParameters();

                _backendCallIsRunning = true;

                FindOutputManagersResponse serviceResponse = Service.FindOutputManagers(serviceRequest);

                if ((serviceResponse != null) && (serviceResponse.FindOutputManagerResult != null))
                {
                    data = serviceResponse.FindOutputManagerResult;
                    
                }
            
                       

            return (data);
        }

        protected override void PresentData(object data)
        {
            _backendCallIsRunning = false;
                        
            View.PresentData(data);

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
                
        public void SelectAndClose(Imi.SupplyChain.OutputManager.Services.Initialization.DataContracts.OutputManager selectedOutputManager)
        {
            if (selectedOutputManager != null) 
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
                    OutputManagerSessionService.OutputManagerId = selectedOutputManager.OutputManagerIdentity;

                    // Set the selected Output Manager
                    ShellInteractionService.ContextInfo = string.Format(LocalResources.STATUSBAR_OM_CLIENT,
                                                                 selectedOutputManager.OutputManagerIdentity,
                                                                 selectedOutputManager.OutputManagerName);
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
