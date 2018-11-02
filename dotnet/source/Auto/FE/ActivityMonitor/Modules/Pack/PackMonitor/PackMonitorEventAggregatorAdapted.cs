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
using Imi.SupplyChain.ActivityMonitor.UX.Modules.Pack.PackMonitor.Translators;
using Imi.SupplyChain.UX.Modules.ActivityMonitor.Infrastructure.Services;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.Pack.PackMonitor
{
    #region Compiler Settings
	  // Disable assigned property never used
	  #pragma warning disable 414 
	#endregion

    public class PackMonitorEventAggregator : PackMonitorEventAggregatorBase
    {
        public override void PackMonitorOverviewViewUpdatedEventHandler(object sender, DataEventArgs<PackMonitorOverviewViewResult> eventArgs)
        {
            IPackMonitorDataDetailView packMonitorDataDetailView = WorkItem.SmartParts.FindByType<IPackMonitorDataDetailView>().Last() as IPackMonitorDataDetailView;

            IList<PackMonitorOverviewViewResult> overviewData = null;

            if (WorkItem.SmartParts.FindByType<IList<PackMonitorOverviewViewResult>>().Count > 0)
            {
                overviewData = WorkItem.SmartParts.FindByType<IList<PackMonitorOverviewViewResult>>().Last() as IList<PackMonitorOverviewViewResult>;
            }

            if (packMonitorDataDetailView != null)
            {
                PackMonitorOverviewViewToPackMonitorDataDetailViewTranslator packMonitorOverviewViewToPackMonitorDataDetailViewTranslator = null;

                if (WorkItem.Items.FindByType<PackMonitorOverviewViewToPackMonitorDataDetailViewTranslator>().Count > 0)
                    packMonitorOverviewViewToPackMonitorDataDetailViewTranslator = WorkItem.Items.FindByType<PackMonitorOverviewViewToPackMonitorDataDetailViewTranslator>().Last();
                else
                    packMonitorOverviewViewToPackMonitorDataDetailViewTranslator = WorkItem.Items.AddNew<PackMonitorOverviewViewToPackMonitorDataDetailViewTranslator>();

                IList<PackMonitorDataDetailViewResult> detailData = new List<PackMonitorDataDetailViewResult>();
                if (overviewData != null)
                {
                    foreach (PackMonitorOverviewViewResult overviewResult in overviewData)
                    {
                        detailData.Add(packMonitorOverviewViewToPackMonitorDataDetailViewTranslator.TranslateFromResultToResult(overviewResult));
                    }
                }

                if (packMonitorDataDetailView.IsEnabled)
                {
                    packMonitorDataDetailView.RefreshDataOnShow = false;
                    packMonitorDataDetailView.PresentData(detailData);
                    packMonitorDataDetailView.UpdateRowCount(detailData.Count, detailData.Count, false);
                }
                else
                {
                    packMonitorDataDetailView.Update(null);
                    packMonitorDataDetailView.UpdateRowCount(0, 0, false);
                }
            }
        }
    }	
}
