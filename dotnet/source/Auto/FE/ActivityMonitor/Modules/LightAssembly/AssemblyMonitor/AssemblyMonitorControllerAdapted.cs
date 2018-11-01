using System;
using System.Linq;
using System.ServiceModel;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.CompositeUI.EventBroker;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Workflow;
using Imi.SupplyChain.UX.Services;
using Imi.SupplyChain.UX.Views;
using Imi.SupplyChain.UX.Modules.ActivityMonitor.Infrastructure.Services;
using Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor;
using Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.Constants;
using Imi.SupplyChain.ActivityMonitor.UX.Modules.LightAssembly.AssemblyMonitor.Translators;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.LightAssembly.AssemblyMonitor
{
    public class AssemblyMonitorController : AssemblyMonitorControllerBase
    {
        private string _favoriteId = null;

        public override void AssemblyMonitorSearchPanelViewAddToFavoritesEventHandler(object sender, DataEventArgs<AssemblyMonitorSearchPanelViewResult> eventArgs)
        {
            Imi.SupplyChain.UX.Infrastructure.ShellMenuItem menuItem = HyperlinkHelper.CreateFavoritesItem(
                Imi.SupplyChain.ActivityMonitor.UX.Modules.LightAssembly.ResourceManager.str_607b9c40_8433_47ff_87f0_6b7dfac9739a_Title,
                "607b9c40-8433-47ff-87f0-6b7dfac9739a",
                Imi.SupplyChain.ActivityMonitor.UX.Modules.LightAssembly.Constants.EventTopicNames.ShowAssemblyMonitorDialog,
                Assembly.GetExecutingAssembly().ManifestModule.Name,
                eventArgs.Data);

            ShellInteractionService.AddToFavorites(menuItem);

            //Get overview view
            if (assemblyMonitorOverviewView != null)
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(menuItem.Id);
                favoriteSettingsService.ContainerName = menuItem.Id;
                favoriteSettingsService.AddProvider(assemblyMonitorOverviewView, new AssemblyMonitorControllerSettingsProvider());
                favoriteSettingsService.SaveSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }
        }


        public void Run(AssemblyMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;
            Run(parameters, false);
        }

        public void Activate(AssemblyMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;

            if (assemblyMonitorOverviewView != null && !string.IsNullOrEmpty(_favoriteId))
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                favoriteSettingsService.ContainerName = _favoriteId;
                favoriteSettingsService.AddProvider(assemblyMonitorOverviewView, new AssemblyMonitorControllerSettingsProvider());
                favoriteSettingsService.LoadSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }

            Activate(parameters);
        }


        protected override void BuildViews()
        {
            searchPanelView = WorkItem.SmartParts.AddNew<AssemblyMonitorSearchPanelView>();
            searchPanelView.RefreshDataOnShow = false;
            searchPanelView.IsVisible = false;

            assemblyMonitorOverviewViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.IAssemblyMonitorOverviewView");
            assemblyMonitorOverviewViewLoader.ViewType = typeof(AssemblyMonitorOverviewView);
            assemblyMonitorOverviewViewLoader.RefreshDataOnShow = false;
            if (parameters != null)
                assemblyMonitorOverviewViewLoader.Update(parameters);

            assemblyMonitorOverviewViewLoader.IsDetailView = false;
            assemblyMonitorOverviewViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Modules.LightAssembly.ResourceManager.str_607b9c40_8433_47ff_87f0_6b7dfac9739a_Title;
            assemblyMonitorOverviewViewLoader.ViewLoaded += (s, e) =>
                {
                    assemblyMonitorOverviewView = e.Data as IAssemblyMonitorOverviewView;

                    if (!string.IsNullOrEmpty(_favoriteId))
                    {
                        UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                        favoriteSettingsService.ContainerName = _favoriteId;
                        favoriteSettingsService.AddProvider(assemblyMonitorOverviewView, new AssemblyMonitorControllerSettingsProvider());
                        favoriteSettingsService.LoadSettings();
                        WorkItem.Items.Remove(favoriteSettingsService);
                    }
                };

            assemblyMonitorDataDetailViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.IAssemblyMonitorDataDetailView");
            assemblyMonitorDataDetailViewLoader.ViewType = typeof(AssemblyMonitorDataDetailView);
            assemblyMonitorDataDetailViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.ResourceManager.str_9d57f38f_d712_4e56_a4bf_6b6bc5be301d_Title;
            assemblyMonitorDataDetailViewLoader.ViewLoaded += (s, e) =>
                {
                    assemblyMonitorDataDetailView = e.Data as IAssemblyMonitorDataDetailView;
                };
        }
    }
}
