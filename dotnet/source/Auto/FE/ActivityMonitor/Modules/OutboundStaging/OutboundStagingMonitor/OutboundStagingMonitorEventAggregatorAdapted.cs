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
using Imi.SupplyChain.ActivityMonitor.UX.Modules.OutboundStaging.OutboundStagingMonitor.Translators;
using Imi.SupplyChain.UX.Modules.ActivityMonitor.Infrastructure.Services;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.OutboundStaging.OutboundStagingMonitor
{
    #region Compiler Settings
	  // Disable assigned property never used
	  #pragma warning disable 414 
	#endregion

    public class OutboundStagingMonitorEventAggregator : OutboundStagingMonitorEventAggregatorBase
    {
        public override void OutboundStagingMonitorOverviewViewUpdatedEventHandler(object sender, DataEventArgs<OutboundStagingMonitorOverviewViewResult> eventArgs)
        {
            IOutboundStagingMonitorDataDetailView chartDataDetailView = WorkItem.SmartParts.FindByType<IOutboundStagingMonitorDataDetailView>().Last() as IOutboundStagingMonitorDataDetailView;

            IList<OutboundStagingMonitorOverviewViewResult> overviewData = null;

            if (WorkItem.SmartParts.FindByType<IList<OutboundStagingMonitorOverviewViewResult>>().Count > 0)
            {
                overviewData = WorkItem.SmartParts.FindByType<IList<OutboundStagingMonitorOverviewViewResult>>().Last() as IList<OutboundStagingMonitorOverviewViewResult>;
            }


            if (chartDataDetailView != null)
            {
                OutboundStagingMonitorOverviewViewToOutboundStagingMonitorDataDetailViewTranslator outboundStagingOverviewViewToChartDataDetailViewTranslator = null;

                if (WorkItem.Items.FindByType<OutboundStagingMonitorOverviewViewToOutboundStagingMonitorDataDetailViewTranslator>().Count > 0)
                    outboundStagingOverviewViewToChartDataDetailViewTranslator = WorkItem.Items.FindByType<OutboundStagingMonitorOverviewViewToOutboundStagingMonitorDataDetailViewTranslator>().Last();
                else
                    outboundStagingOverviewViewToChartDataDetailViewTranslator = WorkItem.Items.AddNew<OutboundStagingMonitorOverviewViewToOutboundStagingMonitorDataDetailViewTranslator>();

                IList<OutboundStagingMonitorDataDetailViewResult> detailData = new List<OutboundStagingMonitorDataDetailViewResult>();
                if (overviewData != null)
                {
                    foreach (OutboundStagingMonitorOverviewViewResult overviewResult in overviewData)
                    {
                        detailData.Add(outboundStagingOverviewViewToChartDataDetailViewTranslator.TranslateFromResultToResult(overviewResult));
                    }
                }

                if (chartDataDetailView.IsEnabled)
                {
                    chartDataDetailView.RefreshDataOnShow = false;
                    chartDataDetailView.PresentData(detailData);
                    chartDataDetailView.UpdateRowCount(detailData.Count, detailData.Count, false);
                }
                else
                {
                    chartDataDetailView.Update(null);
                    chartDataDetailView.UpdateRowCount(0, 0, false);
                }
            }
        }
    }	
}
