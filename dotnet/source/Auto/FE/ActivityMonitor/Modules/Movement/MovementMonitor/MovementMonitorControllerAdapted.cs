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
using Imi.SupplyChain.ActivityMonitor.UX.Modules.Movement.MovementMonitor.Translators;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.Movement.MovementMonitor
{
	public class MovementMonitorController : MovementMonitorControllerBase
    {
        private string _favoriteId = null;

        public virtual void MovementMonitorSearchPanelViewAddToFavoritesEventHandler(object sender, DataEventArgs<MovementMonitorSearchPanelViewResult> eventArgs)
        {
            Imi.SupplyChain.UX.Infrastructure.ShellMenuItem menuItem = HyperlinkHelper.CreateFavoritesItem(
                Imi.SupplyChain.ActivityMonitor.UX.Modules.Movement.ResourceManager.str_c177bbdc_91be_40a0_8eb2_4b7c4537108e_Title,
                "c177bbdc-91be-40a0-8eb2-4b7c4537108e",
                Imi.SupplyChain.ActivityMonitor.UX.Modules.Movement.Constants.EventTopicNames.ShowMovementMonitorDialog,
                Assembly.GetExecutingAssembly().ManifestModule.Name,
                eventArgs.Data);

            ShellInteractionService.AddToFavorites(menuItem);

            //Get overview view
            if (movementMonitorOverviewView != null)
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(menuItem.Id);
                favoriteSettingsService.ContainerName = menuItem.Id;
                favoriteSettingsService.AddProvider(movementMonitorOverviewView, new MovementMonitorControllerSettingsProvider());
                favoriteSettingsService.SaveSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }
        }

        public void Run(MovementMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;
            Run(parameters, false);
        }

        public void Activate(MovementMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;

            if (movementMonitorOverviewView != null && !string.IsNullOrEmpty(_favoriteId))
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                favoriteSettingsService.ContainerName = _favoriteId;
                favoriteSettingsService.AddProvider(movementMonitorOverviewView, new MovementMonitorControllerSettingsProvider());
                favoriteSettingsService.LoadSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }

            Activate(parameters);
        }

        protected override void BuildViews()
        {
            searchPanelView = WorkItem.SmartParts.AddNew<MovementMonitorSearchPanelView>();
            searchPanelView.RefreshDataOnShow = false;
            searchPanelView.IsVisible = false;

            movementMonitorOverviewViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.IMovementMonitorOverviewView");
            movementMonitorOverviewViewLoader.ViewType = typeof(MovementMonitorOverviewView);
            movementMonitorOverviewViewLoader.RefreshDataOnShow = false;
            if (parameters != null)
                movementMonitorOverviewViewLoader.Update(parameters);

            movementMonitorOverviewViewLoader.IsDetailView = false;
            movementMonitorOverviewViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Modules.Movement.ResourceManager.str_c177bbdc_91be_40a0_8eb2_4b7c4537108e_Title;
            movementMonitorOverviewViewLoader.ViewLoaded += (s, e) =>
            {
                movementMonitorOverviewView = e.Data as IMovementMonitorOverviewView;

                if (!string.IsNullOrEmpty(_favoriteId))
                {
                    UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                    favoriteSettingsService.ContainerName = _favoriteId;
                    favoriteSettingsService.AddProvider(movementMonitorOverviewView, new MovementMonitorControllerSettingsProvider());
                    favoriteSettingsService.LoadSettings();
                    WorkItem.Items.Remove(favoriteSettingsService);
                }
            };

            movementMonitorDataDetailViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.IMovementMonitorDataDetailView");
            movementMonitorDataDetailViewLoader.ViewType = typeof(MovementMonitorDataDetailView);
            movementMonitorDataDetailViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.ResourceManager.str_bff16481_1567_4d13_aece_77caa709314e_Title;
            movementMonitorDataDetailViewLoader.ViewLoaded += (s, e) =>
            {
                movementMonitorDataDetailView = e.Data as IMovementMonitorDataDetailView;
            };

        }
	}       
}

