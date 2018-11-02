// Generated from template: .\UX\Dialog\ViewEventAggregatorTemplate.cst
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI;
using Imi.Framework.UX;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Rules;
using Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor;
using Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor.Constants;
using Imi.SupplyChain.ActivityMonitor.UX.Modules.Loading.LoadingMonitor.Translators;
using Imi.SupplyChain.UX.Modules.ActivityMonitor.Infrastructure.Services;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.Loading.LoadingMonitor
{
    #region Compiler Settings
	  // Disable assigned property never used
	  #pragma warning disable 414 
	#endregion

    public class LoadingMonitorEventAggregator : LoadingMonitorEventAggregatorBase
    {
        public override void LoadingMonitorOverviewViewUpdatedEventHandler(object sender, DataEventArgs<LoadingMonitorOverviewViewResult> eventArgs)
        {
            ILoadingMonitorDataDetailView loadingMonitorDataDetailView = WorkItem.SmartParts.FindByType<ILoadingMonitorDataDetailView>().Last() as ILoadingMonitorDataDetailView;

            IList<LoadingMonitorOverviewViewResult> overviewData = null;

            if (WorkItem.SmartParts.FindByType<IList<LoadingMonitorOverviewViewResult>>().Count > 0)
            {
                overviewData = WorkItem.SmartParts.FindByType<IList<LoadingMonitorOverviewViewResult>>().Last() as IList<LoadingMonitorOverviewViewResult>;
            }

            if (loadingMonitorDataDetailView != null)
            {
                LoadingMonitorOverviewViewToLoadingMonitorDataDetailViewTranslator loadingMonitorOverviewViewToLoadingMonitorDataDetailViewTranslator = null;

                if (WorkItem.Items.FindByType<LoadingMonitorOverviewViewToLoadingMonitorDataDetailViewTranslator>().Count > 0)
                    loadingMonitorOverviewViewToLoadingMonitorDataDetailViewTranslator = WorkItem.Items.FindByType<LoadingMonitorOverviewViewToLoadingMonitorDataDetailViewTranslator>().Last();
                else
                    loadingMonitorOverviewViewToLoadingMonitorDataDetailViewTranslator = WorkItem.Items.AddNew<LoadingMonitorOverviewViewToLoadingMonitorDataDetailViewTranslator>();

                IList<LoadingMonitorDataDetailViewResult> detailData = new List<LoadingMonitorDataDetailViewResult>();
                if (overviewData != null)
                {
                    foreach (LoadingMonitorOverviewViewResult overviewResult in overviewData)
                    {
                        detailData.Add(loadingMonitorOverviewViewToLoadingMonitorDataDetailViewTranslator.TranslateFromResultToResult(overviewResult));
                    }
                }

                if (loadingMonitorDataDetailView.IsEnabled)
                {
                    loadingMonitorDataDetailView.RefreshDataOnShow = false;
                    loadingMonitorDataDetailView.PresentData(detailData);
                    loadingMonitorDataDetailView.UpdateRowCount(detailData.Count, detailData.Count, false);
                }
                else
                {
                    loadingMonitorDataDetailView.Update(null);
                    loadingMonitorDataDetailView.UpdateRowCount(0, 0, false);
                }
            }
        }		
    }	
}
