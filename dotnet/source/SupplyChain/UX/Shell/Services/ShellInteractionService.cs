using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;
using Imi.SupplyChain.UX.Shell.Views;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.Framework.UX;
using System.Collections.Specialized;
using Microsoft.Practices.CompositeUI.EventBroker;
using System.ComponentModel;
using Imi.SupplyChain.UX.Shell.Configuration;
using System.Configuration;
using Imi.SupplyChain.UX.Shell.Settings;

namespace Imi.SupplyChain.UX.Shell.Services
{
    public class ShellInteractionService : IShellInteractionService
    {
        private WorkItem _workItem;
        private IShellModule _module;
        private IShellPresentationModel _model;
        private IShellView _shellView;
        private ObservableCollection<ShellAction> _actions;
        private IDictionary<ShellAction, ShellDrillDownMenuItem> _actionDictionary;
        private IList<ShellAction> _actionOrderList;
        private IShellModuleService _shellModuleService;
        
        public event EventHandler<WorkspaceCancelEventArgs> SmartPartClosing;
        public event EventHandler<WorkspaceEventArgs> SmartPartActivated;
        public event EventHandler<HelpRequestedEventArgs> HelpRequested;
        public event EventHandler ModuleActivated;
        public event EventHandler ModuleDeactivated;
        public event EventHandler<CancelEventArgs> ShellTerminating;
        public event EventHandler ShellTerminated;
        public event EventHandler<MenuItemExecutedEventArgs> MenuItemExecuted;
        public event EventHandler<HyperlinkExecutedEventArgs> HyperlinkExecuted;

        [EventPublication(EventTopicNames.FavoriteAdded, PublicationScope.Global)]
        public event EventHandler<DataEventArgs<ShellDrillDownMenuItem>> FavoriteAdded;

        public ShellInteractionService([ServiceDependency] WorkItem workItem, IShellModule module, [ServiceDependency] IShellModuleService shellModuleService)
        {
            _workItem = workItem;
            _module = module;
            _shellModuleService = shellModuleService;

            _actions = new ObservableCollection<ShellAction>();
            _actions.CollectionChanged += ActionsCollectionChangedEventHandler;
            _actionDictionary = new Dictionary<ShellAction, ShellDrillDownMenuItem>();
            _actionOrderList = new List<ShellAction>();

            ControlledWorkItem<ShellController> shellWorkItem = workItem.RootWorkItem.WorkItems.FindByType<ControlledWorkItem<ShellController>>().Last();

            shellModuleService.RegisterModule(module, workItem);
            _model = shellModuleService.GetPresentationModel(module);
            
            _shellView = shellWorkItem.SmartParts.FindByType<IShellView>().Last();
        }

        private void ActionsCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (ShellAction action in e.OldItems)
                {
                    if (_actionDictionary.ContainsKey(action))
                    {
                        action.PropertyChanged -= ActionPropertyChanged;
                        ShellDrillDownMenuItem actionMenuItem = _actionDictionary[action];
                        _model.Actions.Remove(actionMenuItem);
                        _actionDictionary.Remove(action);
                    }

                    if (_actionOrderList.Contains(action))
                    {
                        _actionOrderList.Remove(action);
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (ShellAction action in e.NewItems)
                {
                    int insertIndex = 0;
                    
                    if (!action.IsDetailAction)
                    {
                        foreach (ShellAction listAction in _actionOrderList)
                        {
                            if (listAction.IsDetailAction)
                            {
                                break;
                            }

                            insertIndex++;
                        }
                    }

                    ShellDrillDownMenuItem actionMenuItem = new ShellDrillDownMenuItem();
                    CopyValues(action, actionMenuItem);

                    if (action.IsDetailAction)
                    {
                        _actionOrderList.Add(action);
                        _actionDictionary.Add(action, actionMenuItem);
                        _model.Actions.Add(actionMenuItem);
                    }
                    else //Get order of actions belonging to master view and detail view correct
                    {
                        _actionOrderList.Insert(insertIndex, action);
                        _actionDictionary.Add(action, actionMenuItem);
                        _model.Actions.Insert(insertIndex, actionMenuItem);
                    }

                    action.PropertyChanged += ActionPropertyChanged;
                }
            }
        }

        private IWorkspace GetWorkspace(IShellModule module)
        {
            IWorkspace workspace = _shellView.GetWorkspace(module);

            workspace.SmartPartActivated -= WorkspaceSmartPartActivatedEventHandler;
            workspace.SmartPartClosing -= WorkspaceSmartPartClosingEventHandler;
            workspace.SmartPartActivated += WorkspaceSmartPartActivatedEventHandler;
            workspace.SmartPartClosing += WorkspaceSmartPartClosingEventHandler;

            return workspace;
        }

        private void WorkspaceSmartPartClosingEventHandler(object sender, WorkspaceCancelEventArgs e)
        {
            EventHandler<WorkspaceCancelEventArgs> temp = SmartPartClosing;

            if (temp != null)
                temp(sender, e);
        }

        private void WorkspaceSmartPartActivatedEventHandler(object sender, WorkspaceEventArgs e)
        {
            if (_shellModuleService.ActiveModule.Id != DashboardModule.ModuleId)
            {
                foreach (WorkItem wi in _workItem.GetDescendants())
                {
                    if (wi.SmartParts.FindByType<object>().Contains(e.SmartPart))
                    {
                        _shellModuleService.ActiveModule = _module;
                        break;
                    }
                }
            }

            EventHandler<WorkspaceEventArgs> temp = SmartPartActivated;

            if (temp != null)
                temp(sender, e);
        }

        private void ActionPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ShellAction action = sender as ShellAction;
            
            if (_actionDictionary.ContainsKey(action))
            {
                ShellDrillDownMenuItem actionMenuItem = _actionDictionary[action];
                CopyValues(action, actionMenuItem);
            }
        }

        private static void CopyValues(ShellAction source, ShellDrillDownMenuItem target)
        {
            target.Caption = source.Caption;
            target.Id = source.Id;
            target.IsAuthorized = source.IsAuthorized;
            target.IsEnabled = source.IsEnabled;
            target.WorkItem = source.WorkItem;
            target.Operation = source.Operation;
            target.Parameters = source.Parameters;
        }
                
        public IShellModule ActiveModule
        {
            get
            {
                return _shellView.Model.Module;
            }
        }

        public void Show(object smartPart)
        {
            GetWorkspace(_shellModuleService.ActiveModule).Show(smartPart);
        }

        public void Show(object smartPart, ISmartPartInfo smartPartInfo)
        {
            GetWorkspace(_shellModuleService.ActiveModule).Show(smartPart, smartPartInfo);
        }
              
        public void Close(object smartPart)
        {
            GetWorkspace(_module).Close(smartPart);
        }
               
        public void ShowProgress()
        {
            _shellView.ShowProgress();
        }

        public void HideProgress()
        {
            _shellView.HideProgress();
        }
       
        public object ActiveSmartPart
        {
            get
            {
                return GetWorkspace(_shellModuleService.ActiveModule).ActiveSmartPart;
            }
        }
               
        public MessageBoxResult ShowMessageBox(string caption, string message, string details, MessageBoxButton button, MessageBoxImage image)
        {
            IMessageBoxView view = _workItem.SmartParts.AddNew<MessageBoxView>();
            return view.Show(caption, message, details, button, image);
        }

        public void ShowMessageBox(Exception ex)
        {
            ShowMessageBox(StringResources.ActionException_Text, ex.Message, ex.ToString(), Infrastructure.MessageBoxButton.Ok, Infrastructure.MessageBoxImage.Error);
        }

        public MessageBoxResult ShowNotAuhtorizedMessageBox()
        {
            IMessageBoxView messageBoxView = _workItem.SmartParts.AddNew<MessageBoxView>();
            return messageBoxView.Show(StringResources.Authorization_NotAuhtorized
                , StringResources.Authorization_Message
                , null
                , Infrastructure.MessageBoxButton.Ok
                , Infrastructure.MessageBoxImage.Warning);
        }
                       
        public ICollection<ShellAction> Actions
        {
            get
            {
                return _actions;
            }
        }
                                
        public void Activate(object smartPart)
        {
            GetWorkspace(_module).Activate(smartPart);
        }

        public void ApplySmartPartInfo(object smartPart, ISmartPartInfo smartPartInfo)
        {
            GetWorkspace(_module).ApplySmartPartInfo(smartPart, smartPartInfo);
        }

        public void Hide(object smartPart)
        {
            GetWorkspace(_module).Hide(smartPart);
        }

        public ReadOnlyCollection<object> SmartParts
        {
            get 
            {
                return GetWorkspace(_module).SmartParts;
            }
        }

        public string ContextInfo
        {
            get
            {
                return _model.ContextInfo;
            }
            set
            {
                _model.ContextInfo = value;
            }
        }

        public void ShowNotification(ShellNotification notification)
        {
            _shellView.ShowNotification(_module.Title, notification);
        }
        
        public void OnHelpRequested(EventArgs e)
        {
            if (_shellModuleService.ActiveModule == _module)
            {
                EventHandler<HelpRequestedEventArgs> temp = HelpRequested;

                ShellConfigurationSection config = ConfigurationManager.GetSection(ShellConfigurationSection.SectionKey) as ShellConfigurationSection;
 
                HelpRequestedEventArgs args = new HelpRequestedEventArgs();
                args.HelpBaseUri = config.HelpBaseUri;

                if (temp != null)
                {
                    temp(this, args);
                }
                else
                {
                    args.Cancel = true;
                }

                if (args.Cancel)
                {
                    IMessageBoxView messageBoxView = _workItem.SmartParts.AddNew<MessageBoxView>();
                    messageBoxView.Show(_shellView.Model.Module.Title, StringResources.No_Help_Message, null, Infrastructure.MessageBoxButton.Ok, Infrastructure.MessageBoxImage.Information);
                }
            }
        }

        public void OnShellTerminating(CancelEventArgs e)
        {
            EventHandler<CancelEventArgs> temp = ShellTerminating;

            if (temp != null)
            {
                temp(this, e);
            }
        }

        public void OnShellTerminated(EventArgs e)
        {
            EventHandler temp = ShellTerminated;

            if (temp != null)
            {
                temp(this, e);
            }
        }

        public void OnModuleActivated(EventArgs e)
        {
            EventHandler temp = ModuleActivated;

            if (temp != null)
            {
                temp(this, e);
            }
        }

        public void OnModuleDeactivated(EventArgs e)
        {
            EventHandler temp = ModuleDeactivated;

            if (temp != null)
            {
                temp(this, e);
            }
        }

        public void OnMenuItemExecuted(MenuItemExecutedEventArgs e)
        {
            EventHandler<MenuItemExecutedEventArgs> temp = MenuItemExecuted;

            if (temp != null)
            {
                temp(this, e);
            }
        }

        public void AddToFavorites(ShellMenuItem item)
        {
            ShellDrillDownMenuItem drillDownItem = new ShellDrillDownMenuItem();
            drillDownItem.EventTopic = item.EventTopic;
            drillDownItem.Id = item.Id;
            drillDownItem.Caption = item.Caption;
            drillDownItem.Operation = item.Operation;
            drillDownItem.IsAuthorized = item.IsAuthorized;
            drillDownItem.IsEnabled = item.IsEnabled;
            drillDownItem.Parameters = item.Parameters;
            drillDownItem.AssemblyFile = item.AssemblyFile;
            
            EventHandler<DataEventArgs<ShellDrillDownMenuItem>> temp = FavoriteAdded;

            if (temp != null)
            {
                temp(_module, new DataEventArgs<ShellDrillDownMenuItem>(drillDownItem));
            }
        }

        public void OnHyperlinkExecuted(HyperlinkExecutedEventArgs e)
        {
            EventHandler<HyperlinkExecutedEventArgs> temp = HyperlinkExecuted;

            if (temp != null)
            {
                temp(_module, e);
            }
        }
    }
}
