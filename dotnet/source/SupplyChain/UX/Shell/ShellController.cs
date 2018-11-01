using System;
using System.ServiceModel;
using System.Windows;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Services;
using Imi.Framework.UX;
using Imi.SupplyChain.UX.Shell.Views;
using Imi.SupplyChain.UX.Shell.Services;
using Imi.Framework.UX.Services;
using Imi.Framework.Wpf.Controls;
using System.Threading;
using System.Globalization;
using System.Deployment.Application;
using System.Collections.Specialized;
using System.Web;
using System.Linq;
using System.Configuration;
using System.Reflection;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Microsoft.Practices.CompositeUI.Configuration;
using Microsoft.Practices.ObjectBuilder;
using Imi.SupplyChain.UX.Shell.Configuration;
using Imi.Framework.UX.Wpf.Visualizer;
using System.Windows.Threading;
using System.ComponentModel;

namespace Imi.SupplyChain.UX.Shell
{
	public class ShellController : WorkItemController
	{
        private IWorkspace _shellWorkspace;
        private ILoginView _loginView;
        private IShellView _shellView;
                
        [ServiceDependency]
        public IShellModuleService ShellModuleService { get; set; }

        [ServiceDependency]
        public IUXSettingsService SettingsService { get; set; }
        
        [ServiceDependency]
        public IModuleLoaderService ModuleLoaderService { get; set; }
                               
        public override void Run()
        {
            Application.Current.DispatcherUnhandledException += DispatcherUnhandledExceptionEventHandler;
                        
            _shellWorkspace = WorkItem.RootWorkItem.Workspaces["mainWorkspace"] as IWorkspace;
            _shellView = WorkItem.SmartParts.AddNew<ShellView>();
            _shellWorkspace.Show(_shellView);
        }

        protected override void OnRunStarted()
        {
            base.OnRunStarted();
        }
                                                                                
        private void DispatcherUnhandledExceptionEventHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            Exception ex = e.Exception;

            if (ex is TargetInvocationException)
                if (ex.InnerException != null)
                    ex = ex.InnerException;

            IMessageBoxView messageBoxView = WorkItem.SmartParts.AddNew<MessageBoxView>();
            messageBoxView.Show(StringResources.Unhandled_Exception_Text, ex.Message, ex.ToString(), Infrastructure.MessageBoxButton.Ok, Infrastructure.MessageBoxImage.Error);
        }

        public void Login()
        {
            _loginView = this.WorkItem.SmartParts.AddNew<LoginView>();
            _shellWorkspace.Show(_loginView);
        }

        [EventSubscription(EventTopicNames.ShowSettingsDialog)]
        public void ShowSettingsDialogEventHandler(object sender, EventArgs e)
        {
            var shellSettingsView = WorkItem.SmartParts.AddNew<ShellSettingsView>();

            try
            {
                shellSettingsView.Owner = Application.Current.MainWindow;
                shellSettingsView.ShowDialog();
            }
            finally
            {
                WorkItem.SmartParts.Remove(shellSettingsView);
            }
        }

        [EventSubscription(EventTopicNames.Close)]
        public void CloseEventHandler(object sender, EventArgs e)
        {
            _shellView.CloseActiveSmartPart();
        }

        [EventSubscription(EventTopicNames.CloseAll)]
        public void CloseAllEventHandler(object sender, EventArgs e)
        {
            _shellView.ShowProgress();

            try
            {
                _shellView.CloseActiveWorkspace();
            }
            finally
            {
                _shellView.HideProgress();
            }
        }

        [EventSubscription(EventTopicNames.CloseAllWorkspaces)]
        public void CloseAllWorkspacesEventHandler(object sender, EventArgs e)
        {
            _shellView.ShowProgress();

            try
            {
                _shellView.CloseAllWorkspaces();
            }
            finally
            {
                _shellView.HideProgress();
            }
        }

        [EventSubscription(EventTopicNames.Help)]
        public void HelpEventHandler(object sender, EventArgs e)
        {
            ShellInteractionService shellInteractionService = ShellModuleService.GetWorkItem(ShellModuleService.ActiveModule).Services.Get<IShellInteractionService>() as ShellInteractionService;
            shellInteractionService.OnHelpRequested(e);
        }

        [EventSubscription(EventTopicNames.PrepareShutdown)]
        public void PrepareShutdownEventHandler(object sender, PrepareShutdownEventArgs e)
        {
            var services = from m in ShellModuleService.Modules
                           select ShellModuleService.GetWorkItem(m).Services.Get<IShellInteractionService>() as ShellInteractionService;

            foreach (ShellInteractionService service in services)
            {
                service.OnShellTerminating(e);

                if (e.Cancel)
                {
                    return;
                }
            }
            
            if (e.Logout)
            {
                AppDomain.CurrentDomain.SetData("Logout", new object());
            }

            _shellView.ShowProgress();

            try
            {
                SettingsService.SaveSettings();
                
                IFavoritesService favoritesServices = WorkItem.RootWorkItem.Services.Get<IFavoritesService>();
                favoritesServices.SaveFavorites();
            }
            catch
            {
            }
            finally
            {
                _shellView.HideProgress();
                
                try
                {
                    foreach (ShellInteractionService service in services)
                    {
                        service.OnShellTerminated(new EventArgs());
                    }
                }
                finally
                {
                    //WorkItem.Services.Get<IChannelFactoryService>().Dispose();
                }
            }
        }
                
        [EventSubscription(EventTopicNames.ShowZoomDialog)]
        public void ShowZoomDialogEventHandler(object sender, EventArgs e)
        {
            var zoomView = WorkItem.SmartParts.AddNew<ZoomView>();

            try
            {
                zoomView.Owner = Application.Current.MainWindow;
                zoomView.ZoomLevel = (Application.Current.MainWindow as MainWindow).statusBar.ZoomLevel;

                if (zoomView.ShowDialog() ?? false)
                {
                    (Application.Current.MainWindow as MainWindow).statusBar.ZoomLevel = zoomView.GetPercentage().Value;
                }
            }
            finally 
            {
                WorkItem.SmartParts.Remove(zoomView);
            }
        }

        [EventSubscription(EventTopicNames.ActionExecuted)]
        public void ActionExecutedEventHandler(object sender, DataEventArgs<ShellDrillDownMenuItem> e)
        {
            ShellDrillDownMenuItem action = e.Data;

            if (!action.IsAuthorized)
            {
                IMessageBoxView messageBoxView = WorkItem.SmartParts.AddNew<MessageBoxView>();
                messageBoxView.Show(StringResources.Authorization_NotAuhtorized
                    , StringResources.Authorization_Message
                    , null
                    , Infrastructure.MessageBoxButton.Ok
                    , Infrastructure.MessageBoxImage.Warning);

                return;
            }
            else if (!action.IsEnabled)
                return;

            try
            {
                _shellView.ShowProgress();

                try
                {
                    IActionCatalogService actionCatalog = action.WorkItem.Services.Get<IActionCatalogService>();
                    actionCatalog.Execute(action.Id, action.WorkItem, this, action.Parameters);
                }
                finally
                {
                    _shellView.HideProgress();
                }
            }
            catch (Exception ex)
            {
                IMessageBoxView messageBoxView = WorkItem.SmartParts.AddNew<MessageBoxView>();
                messageBoxView.Show(StringResources.ActionException_Text, ex.Message, ex.ToString(), Infrastructure.MessageBoxButton.Ok, Infrastructure.MessageBoxImage.Error);
            }
        }
                
        [EventSubscription(EventTopicNames.StartMenuItemExecuted)]
        public void StartMenuItemExecutedEventHandler(object sender, StartMenuItemExecutedEventArgs e)
        {
            ShellMenuItem shellMenuItem = new ShellMenuItem();
            shellMenuItem.Id = e.MenuItem.Id;
            shellMenuItem.EventTopic = e.MenuItem.EventTopic;
            shellMenuItem.Caption = e.MenuItem.Caption;
            shellMenuItem.IsAuthorized = e.MenuItem.IsAuthorized;
            shellMenuItem.IsEnabled = e.MenuItem.IsEnabled;
            shellMenuItem.Parameters = e.MenuItem.Parameters;
            shellMenuItem.Operation = e.MenuItem.Operation;
            shellMenuItem.AssemblyFile = e.MenuItem.AssemblyFile;
            
            if (!shellMenuItem.IsAuthorized)
            {
                IMessageBoxView messageBoxView = WorkItem.SmartParts.AddNew<MessageBoxView>();
                messageBoxView.Show(StringResources.Authorization_NotAuhtorized
                    , StringResources.Authorization_Message
                    , null
                    , Infrastructure.MessageBoxButton.Ok
                    , Infrastructure.MessageBoxImage.Warning);

                return;
            }
            
            try
            {
                _shellView.ShowProgress();

                try
                {
                    if (!string.IsNullOrEmpty(shellMenuItem.AssemblyFile))
                    {
                        ModuleInfo info = new ModuleInfo(shellMenuItem.AssemblyFile);
                        ModuleLoaderService.Load(ShellModuleService.GetWorkItem(e.Module), info);
                    }
                                        
                    MenuItemExecutedEventArgs mappedEventArgs = new MenuItemExecutedEventArgs();
                    mappedEventArgs.MenuItem = shellMenuItem;
                    mappedEventArgs.OpenInNewWindow = (e.StartOption == StartOption.NewWindow || e.StartOption == StartOption.Dashboard);
                    
                    ShellInteractionService interactionService = ShellModuleService.GetWorkItem(e.Module).Services.Get<IShellInteractionService>() as ShellInteractionService;
                    interactionService.OnMenuItemExecuted(mappedEventArgs);

                    if (!string.IsNullOrEmpty(shellMenuItem.EventTopic))
                    {
                        EventTopic itemTopic = WorkItem.RootWorkItem.EventTopics.Get(shellMenuItem.EventTopic);

                        if (itemTopic != null)
                        {
                            itemTopic.Fire(this, mappedEventArgs, WorkItem.RootWorkItem, PublicationScope.Global);
                        }
                    }
                }
                finally
                {
                    _shellView.HideProgress();
                }
            }
            catch (Exception ex)
            {
                IMessageBoxView messageBoxView = WorkItem.SmartParts.AddNew<MessageBoxView>();
                messageBoxView.Show(StringResources.ActionException_Text, ex.Message, ex.ToString(), Infrastructure.MessageBoxButton.Ok, Infrastructure.MessageBoxImage.Error);
            }
        }
    }
}
