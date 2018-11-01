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
using Imi.SupplyChain.ActivityMonitor.UX.Modules.CasePick.CasePickMonitor.Translators;
using Imi.SupplyChain.UX.Modules.ActivityMonitor.Infrastructure.Services;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.CasePick.CasePickMonitor
{
    #region Compiler Settings
	  // Disable assigned property never used
	  #pragma warning disable 414 
	#endregion

    public class CasePickMonitorEventAggregator : CasePickMonitorEventAggregatorBase
    {
        public override void CasePickMonitorOverviewViewUpdatedEventHandler(object sender, DataEventArgs<CasePickMonitorOverviewViewResult> eventArgs)
        {
            ICasePickMonitorDataDetailView chartDataDetailView = WorkItem.SmartParts.FindByType<ICasePickMonitorDataDetailView>().Last() as ICasePickMonitorDataDetailView;

            IList<CasePickMonitorOverviewViewResult> overviewData = null;

            if (WorkItem.SmartParts.FindByType<IList<CasePickMonitorOverviewViewResult>>().Count > 0)
            {
                overviewData = WorkItem.SmartParts.FindByType<IList<CasePickMonitorOverviewViewResult>>().Last() as IList<CasePickMonitorOverviewViewResult>;
            }


            if (chartDataDetailView != null)
            {
                CasePickMonitorOverviewViewToCasePickMonitorDataDetailViewTranslator casePickWorkloadOverviewViewToChartDataDetailViewTranslator = null;

                if (WorkItem.Items.FindByType<CasePickMonitorOverviewViewToCasePickMonitorDataDetailViewTranslator>().Count > 0)
                    casePickWorkloadOverviewViewToChartDataDetailViewTranslator = WorkItem.Items.FindByType<CasePickMonitorOverviewViewToCasePickMonitorDataDetailViewTranslator>().Last();
                else
                    casePickWorkloadOverviewViewToChartDataDetailViewTranslator = WorkItem.Items.AddNew<CasePickMonitorOverviewViewToCasePickMonitorDataDetailViewTranslator>();

                IList<CasePickMonitorDataDetailViewResult> detailData = new List<CasePickMonitorDataDetailViewResult>();
                if (overviewData != null)
                {
                    foreach (CasePickMonitorOverviewViewResult overviewResult in overviewData)
                    {
                        detailData.Add(casePickWorkloadOverviewViewToChartDataDetailViewTranslator.TranslateFromResultToResult(overviewResult));
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
