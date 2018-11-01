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
using Imi.SupplyChain.ActivityMonitor.UX.Modules.Receive.ReceiveMonitor.Translators;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.Receive.ReceiveMonitor
{
    public class ReceiveMonitorController : ReceiveMonitorControllerBase
    {
        private string _favoriteId = null;

        public override void ReceiveMonitorSearchPanelViewAddToFavoritesEventHandler(object sender, DataEventArgs<ReceiveMonitorSearchPanelViewResult> eventArgs)
        {
            Imi.SupplyChain.UX.Infrastructure.ShellMenuItem menuItem = HyperlinkHelper.CreateFavoritesItem(
                Imi.SupplyChain.ActivityMonitor.UX.Modules.Receive.ResourceManager.str_0759801e_9ba2_4f20_8383_5549a777f9ab_Title,
                "0759801e-9ba2-4f20-8383-5549a777f9ab",
                Imi.SupplyChain.ActivityMonitor.UX.Modules.Receive.Constants.EventTopicNames.ShowReceiveMonitorDialog,
                Assembly.GetExecutingAssembly().ManifestModule.Name,
                eventArgs.Data);

            ShellInteractionService.AddToFavorites(menuItem);

            if (receiveMonitorOverviewView != null)
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(menuItem.Id);
                favoriteSettingsService.ContainerName = menuItem.Id;
                favoriteSettingsService.AddProvider(receiveMonitorOverviewView, new ReceiveMonitorControllerSettingsProvider());
                favoriteSettingsService.SaveSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }
        }

        public void Run(ReceiveMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;
            Run(parameters, false);
        }

        public void Activate(ReceiveMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;

            if (receiveMonitorOverviewView != null && !string.IsNullOrEmpty(_favoriteId))
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                favoriteSettingsService.ContainerName = _favoriteId;
                favoriteSettingsService.AddProvider(receiveMonitorOverviewView, new ReceiveMonitorControllerSettingsProvider());
                favoriteSettingsService.LoadSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }

            Activate(parameters);
        }


        protected override void BuildViews()
        {
            searchPanelView = WorkItem.SmartParts.AddNew<ReceiveMonitorSearchPanelView>();
            searchPanelView.RefreshDataOnShow = false;
            searchPanelView.IsVisible = false;

            receiveMonitorOverviewViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.IReceiveMonitorOverviewView");
            receiveMonitorOverviewViewLoader.ViewType = typeof(ReceiveMonitorOverviewView);
            receiveMonitorOverviewViewLoader.RefreshDataOnShow = false;
            if (parameters != null)
                receiveMonitorOverviewViewLoader.Update(parameters);

            receiveMonitorOverviewViewLoader.IsDetailView = false;
            receiveMonitorOverviewViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Modules.Receive.ResourceManager.str_0759801e_9ba2_4f20_8383_5549a777f9ab_Title;
            receiveMonitorOverviewViewLoader.ViewLoaded += (s, e) =>
            {
                receiveMonitorOverviewView = e.Data as IReceiveMonitorOverviewView;

                if (!string.IsNullOrEmpty(_favoriteId))
                {
                    UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                    favoriteSettingsService.ContainerName = _favoriteId;
                    favoriteSettingsService.AddProvider(receiveMonitorOverviewView, new ReceiveMonitorControllerSettingsProvider());
                    favoriteSettingsService.LoadSettings();
                    WorkItem.Items.Remove(favoriteSettingsService);
                }
            };

            receiveMonitorDataDetailViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.IReceiveMonitorDataDetailView");
            receiveMonitorDataDetailViewLoader.ViewType = typeof(ReceiveMonitorDataDetailView);
            receiveMonitorDataDetailViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.ResourceManager.str_cb503856_28ed_43a1_a1f3_e5cc4a00bfc8_Title;
            receiveMonitorDataDetailViewLoader.ViewLoaded += (s, e) =>
            {
                receiveMonitorDataDetailView = e.Data as IReceiveMonitorDataDetailView;
            };
        }
    }
}

