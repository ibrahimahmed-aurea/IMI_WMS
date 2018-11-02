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
using Imi.SupplyChain.ActivityMonitor.UX.Modules.LightAssembly.AssemblyMonitor.Translators;
using Imi.SupplyChain.UX.Modules.ActivityMonitor.Infrastructure.Services;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.LightAssembly.AssemblyMonitor
{
    #region Compiler Settings
	  // Disable assigned property never used
	  #pragma warning disable 414 
	#endregion

    public class AssemblyMonitorEventAggregator : AssemblyMonitorEventAggregatorBase
    {
        public override void AssemblyMonitorOverviewViewUpdatedEventHandler(object sender, DataEventArgs<AssemblyMonitorOverviewViewResult> eventArgs)
        {
            IAssemblyMonitorDataDetailView chartDataDetailView = WorkItem.SmartParts.FindByType<IAssemblyMonitorDataDetailView>().Last() as IAssemblyMonitorDataDetailView;

            IList<AssemblyMonitorOverviewViewResult> overviewData = null;

            if (WorkItem.SmartParts.FindByType<IList<AssemblyMonitorOverviewViewResult>>().Count > 0)
            {
                overviewData = WorkItem.SmartParts.FindByType<IList<AssemblyMonitorOverviewViewResult>>().Last() as IList<AssemblyMonitorOverviewViewResult>;
            }


            if (chartDataDetailView != null)
            {
                AssemblyMonitorOverviewViewToAssemblyMonitorDataDetailViewTranslator assemblyOverviewViewToChartDataDetailViewTranslator = null;

                if (WorkItem.Items.FindByType<AssemblyMonitorOverviewViewToAssemblyMonitorDataDetailViewTranslator>().Count > 0)
                    assemblyOverviewViewToChartDataDetailViewTranslator = WorkItem.Items.FindByType<AssemblyMonitorOverviewViewToAssemblyMonitorDataDetailViewTranslator>().Last();
                else
                    assemblyOverviewViewToChartDataDetailViewTranslator = WorkItem.Items.AddNew<AssemblyMonitorOverviewViewToAssemblyMonitorDataDetailViewTranslator>();

                IList<AssemblyMonitorDataDetailViewResult> detailData = new List<AssemblyMonitorDataDetailViewResult>();
                if (overviewData != null)
                {
                    foreach (AssemblyMonitorOverviewViewResult overviewResult in overviewData)
                    {
                        detailData.Add(assemblyOverviewViewToChartDataDetailViewTranslator.TranslateFromResultToResult(overviewResult));
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
