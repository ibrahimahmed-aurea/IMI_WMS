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
using Imi.SupplyChain.ActivityMonitor.UX.Modules.Departure.Constants;
using Imi.SupplyChain.UX.Modules.ActivityMonitor.Infrastructure.Services;
using Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor;
using Imi.SupplyChain.ActivityMonitor.UX.Modules.Departure.DepartureMonitor.Translators;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.Departure.DepartureMonitor
{
    public class DepartureMonitorController : DepartureMonitorControllerBase
    {
        private string _favoriteId = null;

        public override void DepartureMonitorSearchPanelViewAddToFavoritesEventHandler(object sender, DataEventArgs<DepartureMonitorSearchPanelViewResult> eventArgs)
        {
            Imi.SupplyChain.UX.Infrastructure.ShellMenuItem menuItem = HyperlinkHelper.CreateFavoritesItem(
                 Imi.SupplyChain.ActivityMonitor.UX.Modules.Departure.ResourceManager.str_ae5218a2_136b_4eee_a2a5_07a866505018_Title,
                 "ae5218a2-136b-4eee-a2a5-07a866505018",
                 Imi.SupplyChain.ActivityMonitor.UX.Modules.Departure.Constants.EventTopicNames.ShowDepartureMonitorDialog,
                 Assembly.GetExecutingAssembly().ManifestModule.Name,
                 eventArgs.Data);

            ShellInteractionService.AddToFavorites(menuItem);

            //Get overview view
            if (departureMonitorOverviewView != null)
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(menuItem.Id);
                favoriteSettingsService.ContainerName = menuItem.Id;
                favoriteSettingsService.AddProvider(departureMonitorOverviewView, new DepartureMonitorControllerSettingsProvider());
                favoriteSettingsService.SaveSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }
        }

        public void Run(DepartureMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;
            Run(parameters, false);
        }

        public void Activate(DepartureMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;

            if (departureMonitorOverviewView != null && !string.IsNullOrEmpty(_favoriteId))
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                favoriteSettingsService.ContainerName = _favoriteId;
                favoriteSettingsService.AddProvider(departureMonitorOverviewView, new DepartureMonitorControllerSettingsProvider());
                favoriteSettingsService.LoadSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }

            Activate(parameters);
        }

        protected override void BuildViews()
        {
            searchPanelView = WorkItem.SmartParts.AddNew<DepartureMonitorSearchPanelView>();
            searchPanelView.RefreshDataOnShow = false;
            searchPanelView.IsVisible = false;

            departureMonitorOverviewViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.IDepartureMonitorOverviewView");
            departureMonitorOverviewViewLoader.ViewType = typeof(DepartureMonitorOverviewView);
            departureMonitorOverviewViewLoader.RefreshDataOnShow = false;
            if (parameters != null)
                departureMonitorOverviewViewLoader.Update(parameters);

            departureMonitorOverviewViewLoader.IsDetailView = false;
            departureMonitorOverviewViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Modules.Departure.ResourceManager.str_ae5218a2_136b_4eee_a2a5_07a866505018_Title;
            departureMonitorOverviewViewLoader.ViewLoaded += (s, e) =>
            {
                departureMonitorOverviewView = e.Data as IDepartureMonitorOverviewView;
                actionProviderService.RegisterDrillDownAction(departureMonitorOverviewView, WorkItem.Items.FindByType<DepartureMonitorActions>().First().DepartureMonitorOverviewShowWarehouseDeparture, Imi.SupplyChain.ActivityMonitor.UX.Modules.Departure.ResourceManager.str_bd626d5e_63ca_4f60_b211_23bf73610d22_Caption, DepartureMonitorActions.DepartureMonitorOverviewShowWarehouseDepartureOperation);
                departureMonitorOverviewView.EnableDrillDown(new DrillDownArgs() { FieldName = "DepartureId", Caption = Imi.SupplyChain.ActivityMonitor.UX.Modules.Departure.ResourceManager.str_bd626d5e_63ca_4f60_b211_23bf73610d22_Caption, ActionId = WorkItem.Items.FindByType<DepartureMonitorActions>().First().DepartureMonitorOverviewShowWarehouseDeparture, Type = DrillDownType.JumpTo });

                if (!string.IsNullOrEmpty(_favoriteId))
                {
                    UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                    favoriteSettingsService.ContainerName = _favoriteId;
                    favoriteSettingsService.AddProvider(departureMonitorOverviewView, new DepartureMonitorControllerSettingsProvider());
                    favoriteSettingsService.LoadSettings();
                    WorkItem.Items.Remove(favoriteSettingsService);
                }
            };

            departureMonitorDetailViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.IDepartureMonitorDetailView");
            departureMonitorDetailViewLoader.ViewType = typeof(DepartureMonitorDetailView);
            departureMonitorDetailViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.ResourceManager.str_27c919e6_a54a_4b8f_87f9_d302a0f9da56_Title;
            departureMonitorDetailViewLoader.ViewLoaded += (s, e) =>
            {
                departureMonitorDetailView = e.Data as IDepartureMonitorDetailView;
                actionProviderService.RegisterDrillDownAction(departureMonitorDetailView, WorkItem.Items.FindByType<DepartureMonitorActions>().First().DepartureMonitorDetailShowWarehouseDeparture, Imi.SupplyChain.ActivityMonitor.UX.Modules.Departure.ResourceManager.str_bd626d5e_63ca_4f60_b211_23bf73610d22_Caption, DepartureMonitorActions.DepartureMonitorDetailShowWarehouseDepartureOperation);
                departureMonitorDetailView.EnableDrillDown(new DrillDownArgs() { FieldName = "DepartureId", Caption = Imi.SupplyChain.ActivityMonitor.UX.Modules.Departure.ResourceManager.str_bd626d5e_63ca_4f60_b211_23bf73610d22_Caption, ActionId = WorkItem.Items.FindByType<DepartureMonitorActions>().First().DepartureMonitorDetailShowWarehouseDeparture, Type = DrillDownType.JumpTo });
            };

        }
    } 
}
