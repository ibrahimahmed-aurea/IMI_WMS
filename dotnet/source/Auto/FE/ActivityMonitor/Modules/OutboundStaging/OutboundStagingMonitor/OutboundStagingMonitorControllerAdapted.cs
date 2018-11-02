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
using Imi.SupplyChain.ActivityMonitor.UX.Modules.OutboundStaging.OutboundStagingMonitor.Translators;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.OutboundStaging.OutboundStagingMonitor
{
    public class OutboundStagingMonitorController : OutboundStagingMonitorControllerBase
    {
        private string _favoriteId = null;

        public override void OutboundStagingMonitorSearchPanelViewAddToFavoritesEventHandler(object sender, DataEventArgs<OutboundStagingMonitorSearchPanelViewResult> eventArgs)
        {
            Imi.SupplyChain.UX.Infrastructure.ShellMenuItem menuItem = HyperlinkHelper.CreateFavoritesItem(
                Imi.SupplyChain.ActivityMonitor.UX.Modules.OutboundStaging.ResourceManager.str_d1f71448_4043_40d7_b223_c7660c6a8e20_Title,
                "d1f71448-4043-40d7-b223-c7660c6a8e20",
                Imi.SupplyChain.ActivityMonitor.UX.Modules.OutboundStaging.Constants.EventTopicNames.ShowOutboundStagingMonitorDialog,
                Assembly.GetExecutingAssembly().ManifestModule.Name,
                eventArgs.Data);

            ShellInteractionService.AddToFavorites(menuItem);

            //Get overview view
            if (outboundStagingMonitorOverviewView != null)
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(menuItem.Id);
                favoriteSettingsService.ContainerName = menuItem.Id;
                favoriteSettingsService.AddProvider(outboundStagingMonitorOverviewView, new OutboundStagingMonitorControllerSettingsProvider());
                favoriteSettingsService.SaveSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }
        }


        public void Run(OutboundStagingMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;
            Run(parameters, false);
        }

        public void Activate(OutboundStagingMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;

            if (outboundStagingMonitorOverviewView != null && !string.IsNullOrEmpty(_favoriteId))
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                favoriteSettingsService.ContainerName = _favoriteId;
                favoriteSettingsService.AddProvider(outboundStagingMonitorOverviewView, new OutboundStagingMonitorControllerSettingsProvider());
                favoriteSettingsService.LoadSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }

            Activate(parameters);
        }


        protected override void BuildViews()
        {
            searchPanelView = WorkItem.SmartParts.AddNew<OutboundStagingMonitorSearchPanelView>();
            searchPanelView.RefreshDataOnShow = false;
            searchPanelView.IsVisible = false;

            outboundStagingMonitorOverviewViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.IOutboundStagingMonitorOverviewView");
            outboundStagingMonitorOverviewViewLoader.ViewType = typeof(OutboundStagingMonitorOverviewView);
            outboundStagingMonitorOverviewViewLoader.RefreshDataOnShow = false;
            if (parameters != null)
                outboundStagingMonitorOverviewViewLoader.Update(parameters);

            outboundStagingMonitorOverviewViewLoader.IsDetailView = false;
            outboundStagingMonitorOverviewViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Modules.OutboundStaging.ResourceManager.str_d1f71448_4043_40d7_b223_c7660c6a8e20_Title;
            outboundStagingMonitorOverviewViewLoader.ViewLoaded += (s, e) =>
            {
                outboundStagingMonitorOverviewView = e.Data as IOutboundStagingMonitorOverviewView;

                if (!string.IsNullOrEmpty(_favoriteId))
                {
                    UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                    favoriteSettingsService.ContainerName = _favoriteId;
                    favoriteSettingsService.AddProvider(outboundStagingMonitorOverviewView, new OutboundStagingMonitorControllerSettingsProvider());
                    favoriteSettingsService.LoadSettings();
                    WorkItem.Items.Remove(favoriteSettingsService);
                }
            };

            outboundStagingMonitorDataDetailViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.IOutboundStagingMonitorDataDetailView");
            outboundStagingMonitorDataDetailViewLoader.ViewType = typeof(OutboundStagingMonitorDataDetailView);
            outboundStagingMonitorDataDetailViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.ResourceManager.str_897cffcd_7660_4f85_975f_d9317cabeef1_Title;
            outboundStagingMonitorDataDetailViewLoader.ViewLoaded += (s, e) =>
            {
                outboundStagingMonitorDataDetailView = e.Data as IOutboundStagingMonitorDataDetailView;
            };
        }

    }
}

