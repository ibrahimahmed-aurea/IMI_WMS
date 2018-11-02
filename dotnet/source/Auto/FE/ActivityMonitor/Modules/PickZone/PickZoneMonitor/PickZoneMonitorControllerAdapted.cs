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
using Imi.SupplyChain.ActivityMonitor.UX.Modules.PickZone.Constants;
using Imi.SupplyChain.UX.Modules.ActivityMonitor.Infrastructure.Services;
using Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor;
using Imi.SupplyChain.ActivityMonitor.UX.Modules.PickZone.PickZoneMonitor.Translators;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.PickZone.PickZoneMonitor
{    	
    public class PickZoneMonitorController : PickZoneMonitorControllerBase
    {
        private string _favoriteId = null;

        public override void PickZoneMonitorSearchPanelViewAddToFavoritesEventHandler(object sender, DataEventArgs<PickZoneMonitorSearchPanelViewResult> eventArgs)
        {
            Imi.SupplyChain.UX.Infrastructure.ShellMenuItem menuItem = HyperlinkHelper.CreateFavoritesItem(
                 Imi.SupplyChain.ActivityMonitor.UX.Modules.PickZone.ResourceManager.str_b54f04b1_e8ed_476e_8851_37bffa4b866e_Title,
                "b54f04b1-e8ed-476e-8851-37bffa4b866e",
                 Imi.SupplyChain.ActivityMonitor.UX.Modules.PickZone.Constants.EventTopicNames.ShowPickZoneMonitorDialog,
                 Assembly.GetExecutingAssembly().ManifestModule.Name,
                 eventArgs.Data);

            ShellInteractionService.AddToFavorites(menuItem);

            //Get overview view
            if (pickZoneMonitorOverviewView != null)
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(menuItem.Id);
                favoriteSettingsService.ContainerName = menuItem.Id;
                favoriteSettingsService.AddProvider(pickZoneMonitorOverviewView, new PickZoneMonitorControllerSettingsProvider());
                favoriteSettingsService.SaveSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }
        }

        public void Run(PickZoneMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;
            Run(parameters, false);
        }

        public void Activate(PickZoneMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;

            if (pickZoneMonitorOverviewView != null && !string.IsNullOrEmpty(_favoriteId))
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                favoriteSettingsService.ContainerName = _favoriteId;
                favoriteSettingsService.AddProvider(pickZoneMonitorOverviewView, new PickZoneMonitorControllerSettingsProvider());
                favoriteSettingsService.LoadSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }

            Activate(parameters);
        }


        protected override void BuildViews()
        {
            searchPanelView = WorkItem.SmartParts.AddNew<PickZoneMonitorSearchPanelView>();
            searchPanelView.RefreshDataOnShow = false;
            searchPanelView.IsVisible = false;

            pickZoneMonitorOverviewViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.IPickZoneMonitorOverviewView");
            pickZoneMonitorOverviewViewLoader.ViewType = typeof(PickZoneMonitorOverviewView);
            pickZoneMonitorOverviewViewLoader.RefreshDataOnShow = false;
            if (parameters != null)
                pickZoneMonitorOverviewViewLoader.Update(parameters);

            pickZoneMonitorOverviewViewLoader.IsDetailView = false;
            pickZoneMonitorOverviewViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Modules.PickZone.ResourceManager.str_b54f04b1_e8ed_476e_8851_37bffa4b866e_Title;
            pickZoneMonitorOverviewViewLoader.ViewLoaded += (s, e) =>
            {
                pickZoneMonitorOverviewView = e.Data as IPickZoneMonitorOverviewView;

                if (!string.IsNullOrEmpty(_favoriteId))
                {
                    UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                    favoriteSettingsService.ContainerName = _favoriteId;
                    favoriteSettingsService.AddProvider(pickZoneMonitorOverviewView, new PickZoneMonitorControllerSettingsProvider());
                    favoriteSettingsService.LoadSettings();
                    WorkItem.Items.Remove(favoriteSettingsService);
                }
            };
        }
    }  
}
