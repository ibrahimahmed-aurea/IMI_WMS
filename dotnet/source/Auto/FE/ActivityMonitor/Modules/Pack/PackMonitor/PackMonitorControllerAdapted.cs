// Generated from template: .\UX\Dialog\WorkItemControllerTemplate.cst
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
using Imi.SupplyChain.ActivityMonitor.UX.Modules.Pack.PackMonitor.Translators;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.Pack.PackMonitor
{
	public class PackMonitorController : PackMonitorControllerBase
    {
        private string _favoriteId = null;

        public override void PackMonitorSearchPanelViewAddToFavoritesEventHandler(object sender, DataEventArgs<PackMonitorSearchPanelViewResult> eventArgs)
        {
            Imi.SupplyChain.UX.Infrastructure.ShellMenuItem menuItem = HyperlinkHelper.CreateFavoritesItem(
                Imi.SupplyChain.ActivityMonitor.UX.Modules.Pack.ResourceManager.str_37b749b8_18c9_498b_abc8_3d0ee722bd93_Title,
                "37b749b8-18c9-498b-abc8-3d0ee722bd93",
                Imi.SupplyChain.ActivityMonitor.UX.Modules.Pack.Constants.EventTopicNames.ShowPackMonitorDialog,
                Assembly.GetExecutingAssembly().ManifestModule.Name,
                eventArgs.Data);

            ShellInteractionService.AddToFavorites(menuItem);

            if (packMonitorOverviewView != null)
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(menuItem.Id);
                favoriteSettingsService.ContainerName = menuItem.Id;
                favoriteSettingsService.AddProvider(packMonitorOverviewView, new PackMonitorControllerSettingsProvider());
                favoriteSettingsService.SaveSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }
        }

        public void Run(PackMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;
            Run(parameters, false);
        }

        public void Activate(PackMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;

            if (packMonitorOverviewView != null && !string.IsNullOrEmpty(_favoriteId))
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                favoriteSettingsService.ContainerName = _favoriteId;
                favoriteSettingsService.AddProvider(packMonitorOverviewView, new PackMonitorControllerSettingsProvider());
                favoriteSettingsService.LoadSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }

            Activate(parameters);
        }

        protected override void BuildViews()
        {
            searchPanelView = WorkItem.SmartParts.AddNew<PackMonitorSearchPanelView>();
            searchPanelView.RefreshDataOnShow = false;
            searchPanelView.IsVisible = false;

            packMonitorOverviewViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.IPackMonitorOverviewView");
            packMonitorOverviewViewLoader.ViewType = typeof(PackMonitorOverviewView);
            packMonitorOverviewViewLoader.RefreshDataOnShow = false;
            if (parameters != null)
                packMonitorOverviewViewLoader.Update(parameters);

            packMonitorOverviewViewLoader.IsDetailView = false;
            packMonitorOverviewViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Modules.Pack.ResourceManager.str_37b749b8_18c9_498b_abc8_3d0ee722bd93_Title;
            packMonitorOverviewViewLoader.ViewLoaded += (s, e) =>
            {
                packMonitorOverviewView = e.Data as IPackMonitorOverviewView;

                if (!string.IsNullOrEmpty(_favoriteId))
                {
                    UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                    favoriteSettingsService.ContainerName = _favoriteId;
                    favoriteSettingsService.AddProvider(packMonitorOverviewView, new PackMonitorControllerSettingsProvider());
                    favoriteSettingsService.LoadSettings();
                    WorkItem.Items.Remove(favoriteSettingsService);
                }
            };

            packMonitorDataDetailViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.IPackMonitorDataDetailView");
            packMonitorDataDetailViewLoader.ViewType = typeof(PackMonitorDataDetailView);
            packMonitorDataDetailViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.ResourceManager.str_08aaca54_f7a9_4897_8134_67427bea5459_Title;
            packMonitorDataDetailViewLoader.ViewLoaded += (s, e) =>
            {
                packMonitorDataDetailView = e.Data as IPackMonitorDataDetailView;
            };

        }
    
	}       
}

