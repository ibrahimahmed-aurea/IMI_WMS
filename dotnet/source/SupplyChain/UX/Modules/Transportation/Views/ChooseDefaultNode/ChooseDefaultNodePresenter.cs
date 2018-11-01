using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI.EventBroker;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX.Modules.Transportation.Infrastructure.Services;
using Imi.SupplyChain.UX.Views;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.Transportation.Services.Authentication.DataContracts;
using Imi.SupplyChain.Transportation.Services.Authentication.ServiceContracts;

namespace Imi.SupplyChain.UX.Modules.Transportation.Views.ChooseDefaultNode
{
    
    public class ChooseDefaultNodePresenter : AsyncDataPresenter<IAuthenticationService, IChooseDefaultNodeView>
    {
        private bool _forceUpdate;
        private bool _backendCallIsRunning = false;
        
        private FindUserDetailsParameters viewParameters;

        [ServiceDependency]
        public ITransportationUserSessionService UserSessionService { get; set; }

        [InjectionConstructor]
        public ChooseDefaultNodePresenter([WcfServiceDependency] IAuthenticationService authenticationService)
            : base(authenticationService)
        {
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

                UserNode currentNode = null;

                if (UserSessionService.NodeId != null)
                {
                    string nodeId = UserSessionService.NodeId as string;

                    IEnumerable<UserNode> l = from UserNode n in userDetails.UserNodes where n.NodeIdentity == nodeId select n;

                    if (l.Count() > 0)
                    {
                        currentNode = l.First();
                    }
                }

                if(currentNode == null)
                {
                    string nodeId = userDetails.RecentNodeIdentity;

                    IEnumerable<UserNode> l = from UserNode n in userDetails.UserNodes where n.NodeIdentity == nodeId select n;

                    if (l.Count() > 0)
                    {
                        currentNode = l.First();
                        // deafult to this if the user presses cancel
                        UserSessionService.NodeId = currentNode.NodeIdentity;
                    }
                }

                View.SelectNode(currentNode);
            }

        }

        public void Close()
        {
            // Abort running call to backend
            if (_backendCallIsRunning)
            {
                _backendCallIsRunning = false;
                this.Cancel();
            }

            this.View.Close();
            WorkItem.SmartParts.Remove(this.View);
        }
                
        public void SelectAndClose(UserNode selectedNode)
        {
            if (selectedNode != null)
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
                                        
                    UserSessionService.NodeId = selectedNode.NodeIdentity;

                    // Get the application
                    IShellModule module = WorkItem.Items.FindByType<IShellModule>().First();

                    LogonParameters logonParameters = new LogonParameters();
                    logonParameters.UserIdentity = UserSessionService.UserId;
                    logonParameters.NodeIdentity = selectedNode.NodeIdentity;
                    logonParameters.TerminalIdentity = UserSessionService.TerminalId;
                    logonParameters.ApplicationIdentity = module.Id;

                    LogonRequest logonRequest = new LogonRequest();

                    logonRequest.LogonParameters = logonParameters;

                    Service.Logon(logonRequest);
                                        
                    if (userSettingsTopic != null)
                    {
                        UserSettingsChangedEventArgs userSettingsChangedEventArgs = new UserSettingsChangedEventArgs(true);
                        userSettingsTopic.Fire(this, userSettingsChangedEventArgs, WorkItem, PublicationScope.Descendants);
                    }

                    // Set the selected Node on statusrow in container
                    ShellInteractionService.ContextInfo = string.Format(LocalResources.STATUSBAR_TRANSPORTATION_CLIENT,
                                                                 selectedNode.NodeIdentity,
                                                                 selectedNode.NodeName);

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
