using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.UX;
using Microsoft.Practices.CompositeUI.EventBroker;
using Imi.Framework.UX.Services;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using Imi.SupplyChain.UX.Shell.Services;
using System.Collections.ObjectModel;
using Microsoft.Practices.CompositeUI.Services;
using Microsoft.Practices.CompositeUI.Configuration;
using System.Threading;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Infrastructure.Services;
using System.Windows;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public class ShellPresenter : Presenter<IShellView>
    {
        [ServiceDependency]
        public IShellModuleService ShellModuleService { get; set; }

        [ServiceDependency]
        public IUXSettingsService UXSettingsService { get; set; }
                
        [InjectionConstructor]
        public ShellPresenter([ServiceDependency] WorkItem workItem)
        {
            
        }

        public override void OnViewReady()
        {
            if (ShellModuleService.ActiveModule == null && ShellModuleService.Modules.Count > 0)
            {
                ShellModuleService.ActiveModule = ShellModuleService.Modules[0];
            }
        }

        [EventSubscription(EventTopicNames.ModuleActivating)]
        public void ModuleActivatingEventHandler(object sender, DataEventArgs<IShellModule> e)
        {
            PresentModule(e.Data);
        }

        private void PresentModule(IShellModule module)
        {
            View.Model = ShellModuleService.GetPresentationModel(module);
            Application.Current.MainWindow.DataContext = View.Model;
        }
    }
}
