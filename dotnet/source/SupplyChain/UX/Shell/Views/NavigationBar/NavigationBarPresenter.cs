using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using Imi.Framework.UX;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Shell.Services;
using Imi.Framework.Wpf.Controls;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public class NavigationBarPresenter : Presenter<INavigationBarView>
    {
        [EventPublication(EventTopicNames.StartMenuItemExecuted, PublicationScope.WorkItem)]
        public event EventHandler<StartMenuItemExecutedEventArgs> MenuItemExecuted;

        [EventPublication(EventTopicNames.ActionExecuted, PublicationScope.WorkItem)]
        public event EventHandler<DataEventArgs<ShellDrillDownMenuItem>> ActionExecuted;

        private IShellModuleService _shellModuleService;

        [InjectionConstructor]
        public NavigationBarPresenter([ServiceDependency] IShellModuleService shellModuleService)
        {
            _shellModuleService = shellModuleService;

            _shellModuleService.ModuleLoaded += (s, e) =>
                {
                    View.AddModel(_shellModuleService.GetPresentationModel(e.Data));
                };
        }
        
        public void ExecuteMenuItem(StartMenuItemExecutedEventArgs e)
        {
            EventHandler<StartMenuItemExecutedEventArgs> temp = MenuItemExecuted;

            e.Module = _shellModuleService.ActiveModule;

            if (e.StartOption == StartOption.Dashboard)
            {
                
                //When adding dialogs to the Dashboard we need to be sure that the Dashboard module activation as finished.
                //To be abel to know this we temporarly listen to the ModuleActivated event and then activate the module.
                //When the activation thread is done we get the event and can then fire the MenuItemExecuted event.
                //This procedure is a must when we add a dialog without first activating the Dashboard in the GUI.
                EventHandler<DataEventArgs<IShellModule>> moduleActivatedHandler = null;

                moduleActivatedHandler = (s, e1) =>
                {
                    if (e1.Data.Id == DashboardModule.ModuleId && temp != null)
                    {
                        temp(this, e);
                        temp = null;
                        _shellModuleService.ModuleActivated -= moduleActivatedHandler;
                    }
                };

                _shellModuleService.ModuleActivated += moduleActivatedHandler;

                _shellModuleService.ActiveModule = _shellModuleService.Modules.Where((m) => { return m.Id == DashboardModule.ModuleId; }).FirstOrDefault();
            }
            else
            {
                if (temp != null)
                    temp(this, e);
            }
        }

        public void ActivateModule(IShellModule module)
        {
            IShellView shellView = WorkItem.SmartParts.FindByType<IShellView>().Last();

            shellView.ShowProgress();

            try
            {
                _shellModuleService.ActiveModule = module;
            }
            finally
            {
                shellView.HideProgress();
            }
        }

        public void ExecuteAction(ShellDrillDownMenuItem action)
        {
            EventHandler<DataEventArgs<ShellDrillDownMenuItem>> temp = ActionExecuted;

            if (temp != null)
                temp(this, new DataEventArgs<ShellDrillDownMenuItem>(action));
        }

        [EventSubscription(EventTopicNames.FavoriteAdded)]
        public void FavoriteAddedEventHandler(object sender, DataEventArgs<ShellDrillDownMenuItem> e)
        {
            View.AddToFavorites(sender as IShellModule, e.Data);
        }

        [EventSubscription(EventTopicNames.ModuleActivating)]
        public void ModuleActivatingEventHandler(object sender, DataEventArgs<IShellModule> e)
        {
            if (e.Data != View.SelectedModule)
            {
                View.SelectedModule = e.Data;
            }
        }
    }
}
