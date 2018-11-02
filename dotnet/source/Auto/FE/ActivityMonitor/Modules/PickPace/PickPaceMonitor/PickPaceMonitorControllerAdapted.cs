using System;
using System.Linq;
using System.ServiceModel;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.CompositeUI.EventBroker;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Workflow;
using Imi.SupplyChain.UX.Services;
using Imi.SupplyChain.UX.Views;
using Imi.SupplyChain.ActivityMonitor.UX.Modules.PickPace.Constants;
using Imi.SupplyChain.UX.Modules.ActivityMonitor.Infrastructure.Services;
using Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor;
using Imi.SupplyChain.ActivityMonitor.UX.Modules.PickPace.PickPaceMonitor.Translators;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.PickPace.PickPaceMonitor
{    	
    public class PickPaceMonitorController : PickPaceMonitorControllerBase
    {
        private string _favoriteId = null;

        public override void PickPaceMonitorSearchPanelViewAddToFavoritesEventHandler(object sender, DataEventArgs<PickPaceMonitorSearchPanelViewResult> eventArgs)
        {
            Imi.SupplyChain.UX.Infrastructure.ShellMenuItem menuItem = HyperlinkHelper.CreateFavoritesItem(
                 Imi.SupplyChain.ActivityMonitor.UX.Modules.PickPace.ResourceManager.str_121ad164_ddc9_495b_a470_c8b6a9e55001_Title,
                 "121ad164-ddc9-495b-a470-c8b6a9e55001",
                 Imi.SupplyChain.ActivityMonitor.UX.Modules.PickPace.Constants.EventTopicNames.ShowPickPaceMonitorDialog,
                 Assembly.GetExecutingAssembly().ManifestModule.Name,
                 eventArgs.Data);

            ShellInteractionService.AddToFavorites(menuItem);

            //Get overview view
            if (pickPaceMonitorOverviewView != null)
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(menuItem.Id);
                favoriteSettingsService.ContainerName = menuItem.Id;
                favoriteSettingsService.AddProvider(pickPaceMonitorOverviewView, new PickPaceMonitorControllerSettingsProvider());
                favoriteSettingsService.SaveSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }
        }

        public void Run(PickPaceMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;
            Run(parameters, false);
        }

        public void Activate(PickPaceMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;

            if (pickPaceMonitorOverviewView != null && !string.IsNullOrEmpty(_favoriteId))
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                favoriteSettingsService.ContainerName = _favoriteId;
                favoriteSettingsService.AddProvider(pickPaceMonitorOverviewView, new PickPaceMonitorControllerSettingsProvider());
                favoriteSettingsService.LoadSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }

            Activate(parameters);
        }


        protected override void BuildViews()
        {
            searchPanelView = WorkItem.SmartParts.AddNew<PickPaceMonitorSearchPanelView>();
            searchPanelView.RefreshDataOnShow = false;
            searchPanelView.IsVisible = false;

            pickPaceMonitorOverviewViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.IPickPaceMonitorOverviewView");
            pickPaceMonitorOverviewViewLoader.ViewType = typeof(PickPaceMonitorOverviewView);
            pickPaceMonitorOverviewViewLoader.RefreshDataOnShow = false;
            if (parameters != null)
                pickPaceMonitorOverviewViewLoader.Update(parameters);

            pickPaceMonitorOverviewViewLoader.IsDetailView = false;
            pickPaceMonitorOverviewViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Modules.PickPace.ResourceManager.str_121ad164_ddc9_495b_a470_c8b6a9e55001_Title;
            pickPaceMonitorOverviewViewLoader.ViewLoaded += (s, e) =>
            {
                pickPaceMonitorOverviewView = e.Data as IPickPaceMonitorOverviewView;

                if (!string.IsNullOrEmpty(_favoriteId))
                {
                    UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                    favoriteSettingsService.ContainerName = _favoriteId;
                    favoriteSettingsService.AddProvider(pickPaceMonitorOverviewView, new PickPaceMonitorControllerSettingsProvider());
                    favoriteSettingsService.LoadSettings();
                    WorkItem.Items.Remove(favoriteSettingsService);
                }
            };

            pickPaceMonitorDetailViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.IPickPaceMonitorDetailView");
            pickPaceMonitorDetailViewLoader.ViewType = typeof(PickPaceMonitorDetailView);
            pickPaceMonitorDetailViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.ResourceManager.str_486bcfcc_6cd6_4e65_a5c6_5a6b7a7ae67a_Title;
            pickPaceMonitorDetailViewLoader.ViewLoaded += (s, e) =>
            {
                pickPaceMonitorDetailView = e.Data as IPickPaceMonitorDetailView;
            };

        }
    }  
}
