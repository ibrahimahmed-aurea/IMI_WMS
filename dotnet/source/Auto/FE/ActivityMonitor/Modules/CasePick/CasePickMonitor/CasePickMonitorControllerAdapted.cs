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
using Imi.SupplyChain.ActivityMonitor.UX.Modules.CasePick.CasePickMonitor.Translators;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.CasePick.CasePickMonitor
{
    public class CasePickMonitorController : CasePickMonitorControllerBase
    {
        private string _favoriteId = null;

        public override void CasePickMonitorSearchPanelViewAddToFavoritesEventHandler(object sender, DataEventArgs<CasePickMonitorSearchPanelViewResult> eventArgs)
        {
            Imi.SupplyChain.UX.Infrastructure.ShellMenuItem menuItem = HyperlinkHelper.CreateFavoritesItem(
               Imi.SupplyChain.ActivityMonitor.UX.Modules.CasePick.ResourceManager.str_24cd9629_716f_4e53_9969_22f7a444c534_Title,
               "24cd9629-716f-4e53-9969-22f7a444c534",
               Imi.SupplyChain.ActivityMonitor.UX.Modules.CasePick.Constants.EventTopicNames.ShowCasePickMonitorDialog,
               Assembly.GetExecutingAssembly().ManifestModule.Name,
               eventArgs.Data);

            ShellInteractionService.AddToFavorites(menuItem);

            //Get overview view
            if (casePickMonitorOverviewView != null)
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(menuItem.Id);
                favoriteSettingsService.ContainerName = menuItem.Id;
                favoriteSettingsService.AddProvider(casePickMonitorOverviewView, new CasePickMonitorControllerSettingsProvider());
                favoriteSettingsService.SaveSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }
        }


        public void Run(CasePickMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;
            Run(parameters, false);
        }

        public void Activate(CasePickMonitorOverviewViewParameters parameters, string favoriteId)
        {
            _favoriteId = favoriteId;

            if (casePickMonitorOverviewView != null && !string.IsNullOrEmpty(_favoriteId))
            {
                UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                favoriteSettingsService.ContainerName = _favoriteId;
                favoriteSettingsService.AddProvider(casePickMonitorOverviewView, new CasePickMonitorControllerSettingsProvider());
                favoriteSettingsService.LoadSettings();
                WorkItem.Items.Remove(favoriteSettingsService);
            }

            Activate(parameters);
        }


        protected override void BuildViews()
        {
            searchPanelView = WorkItem.SmartParts.AddNew<CasePickMonitorSearchPanelView>();
            searchPanelView.RefreshDataOnShow = false;
            searchPanelView.IsVisible = false;

            casePickMonitorOverviewViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.ICasePickMonitorOverviewView");
            casePickMonitorOverviewViewLoader.ViewType = typeof(CasePickMonitorOverviewView);
            casePickMonitorOverviewViewLoader.RefreshDataOnShow = false;
            if (parameters != null)
                casePickMonitorOverviewViewLoader.Update(parameters);

            casePickMonitorOverviewViewLoader.IsDetailView = false;
            casePickMonitorOverviewViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Modules.CasePick.ResourceManager.str_24cd9629_716f_4e53_9969_22f7a444c534_Title;
            casePickMonitorOverviewViewLoader.ViewLoaded += (s, e) =>
            {
                casePickMonitorOverviewView = e.Data as ICasePickMonitorOverviewView;

                if (!string.IsNullOrEmpty(_favoriteId))
                {
                    UXSettingsService favoriteSettingsService = WorkItem.Items.AddNew<UXSettingsService>(_favoriteId);
                    favoriteSettingsService.ContainerName = _favoriteId;
                    favoriteSettingsService.AddProvider(casePickMonitorOverviewView, new CasePickMonitorControllerSettingsProvider());
                    favoriteSettingsService.LoadSettings();
                    WorkItem.Items.Remove(favoriteSettingsService);
                }
            };

            casePickMonitorDataDetailViewLoader = WorkItem.SmartParts.AddNew<DynamicViewLoader>("Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.ICasePickMonitorDataDetailView");
            casePickMonitorDataDetailViewLoader.ViewType = typeof(CasePickMonitorDataDetailView);
            casePickMonitorDataDetailViewLoader.Title = Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.ResourceManager.str_1e68c425_2a36_4e9d_a074_8927a5b6c881_Title;
            casePickMonitorDataDetailViewLoader.ViewLoaded += (s, e) =>
            {
                casePickMonitorDataDetailView = e.Data as ICasePickMonitorDataDetailView;
            };

        }

    }
}

