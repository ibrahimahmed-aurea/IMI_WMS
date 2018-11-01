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
using Imi.SupplyChain.ActivityMonitor.UX.Modules.Loading.LoadingMonitor.Translators;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.Loading.LoadingMonitor
{
	public class LoadingMonitorController : LoadingMonitorControllerBase
    {
        private string _favoriteId = null;

        public override void LoadingMonitorSearchPanelViewAddToFavoritesEventHandler(object sender, DataEventArgs<LoadingMonitorSearchPanelViewResult> eventArgs)
        {
            Imi.SupplyChain.UX.Infrastructure.ShellMenuItem menuItem = HyperlinkHelper.CreateFavoritesItem(
                Imi.SupplyChain.ActivityMonitor.UX.Modules.Loading.ResourceManager.str_2b50c1d8_19fe_41dc_ba6b_05e86c87a9d8_Title,
                "2b50c1d8-19fe-41dc-ba6b-05e86c87a9d8",
                Imi.SupplyChain.ActivityMonitor.UX.Modules.Loading.Constants.EventTopicNames.ShowLoadingMonitorDialog,
                Assembly.GetExecutingAssembly().ManifestModule.Name,
                eventArgs.Data);

            ShellInteractionService.AddToFavorites(menuItem);

            //Get overview view
            if (loadingMonitorOverviewView != null)
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(menuItem.Id);
                favoriteSettingsService.ContainerName = menuItem.Id;
                favoriteSettingsService.AddProvider(loadingMonitorOverviewView, new LoadingMonitorControllerSettingsProvider());
                favoriteSettingsService.SaveSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }
        }

        public void Run(LoadingMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;
            Run(parameters, false);
        }

        public void Activate(LoadingMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;

            if (loadingMonitorOverviewView != null && !string.IsNullOrEmpty(_favoriteId))
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                favoriteSettingsService.ContainerName = _favoriteId;
                favoriteSettingsService.AddProvider(loadingMonitorOverviewView, new LoadingMonitorControllerSettingsProvider());
                favoriteSettingsService.LoadSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }

            Activate(parameters);
        }

        protected override void BuildViews()
        {
            searchPanelView = WorkItem.SmartParts.AddNew<LoadingMonitorSearchPanelView>();
            searchPanelView.RefreshDataOnShow = false;
            searchPanelView.IsVisible = false;

            loadingMonitorOverviewViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.ILoadingMonitorOverviewView");
            loadingMonitorOverviewViewLoader.ViewType = typeof(LoadingMonitorOverviewView);
            loadingMonitorOverviewViewLoader.RefreshDataOnShow = false;
            if (parameters != null)
                loadingMonitorOverviewViewLoader.Update(parameters);

            loadingMonitorOverviewViewLoader.IsDetailView = false;
            loadingMonitorOverviewViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Modules.Loading.ResourceManager.str_2b50c1d8_19fe_41dc_ba6b_05e86c87a9d8_Title;
            loadingMonitorOverviewViewLoader.ViewLoaded += (s, e) =>
            {
                loadingMonitorOverviewView = e.Data as ILoadingMonitorOverviewView;

                if (!string.IsNullOrEmpty(_favoriteId))
                {
                    UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                    favoriteSettingsService.ContainerName = _favoriteId;
                    favoriteSettingsService.AddProvider(loadingMonitorOverviewView, new LoadingMonitorControllerSettingsProvider());
                    favoriteSettingsService.LoadSettings();
                    WorkItem.Items.Remove(favoriteSettingsService);
                }
            };

            loadingMonitorDataDetailViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.ILoadingMonitorDataDetailView");
            loadingMonitorDataDetailViewLoader.ViewType = typeof(LoadingMonitorDataDetailView);
            loadingMonitorDataDetailViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.ResourceManager.str_5bb4b3f1_c5f6_4ea0_8690_1be5827c7e4b_Title;
            loadingMonitorDataDetailViewLoader.ViewLoaded += (s, e) =>
            {
                loadingMonitorDataDetailView = e.Data as ILoadingMonitorDataDetailView;
            };

        }
	}       
}

