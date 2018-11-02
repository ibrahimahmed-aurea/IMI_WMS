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
using Imi.SupplyChain.ActivityMonitor.UX.Modules.Receive.ReceiveMonitor.Translators;
using Imi.SupplyChain.UX.Modules.ActivityMonitor.Infrastructure.Services;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.Receive.ReceiveMonitor
{
    #region Compiler Settings
	  // Disable assigned property never used
	  #pragma warning disable 414 
	#endregion

    public class ReceiveMonitorEventAggregator : ReceiveMonitorEventAggregatorBase
    {
        public override void ReceiveMonitorOverviewViewUpdatedEventHandler(object sender, DataEventArgs<ReceiveMonitorOverviewViewResult> eventArgs)
        {
            IReceiveMonitorDataDetailView receiveMonitorDataDetailView = WorkItem.SmartParts.FindByType<IReceiveMonitorDataDetailView>().Last() as IReceiveMonitorDataDetailView;

            IList<ReceiveMonitorOverviewViewResult> overviewData = null;

            if (WorkItem.SmartParts.FindByType<IList<ReceiveMonitorOverviewViewResult>>().Count > 0)
            {
                overviewData = WorkItem.SmartParts.FindByType<IList<ReceiveMonitorOverviewViewResult>>().Last() as IList<ReceiveMonitorOverviewViewResult>;
            }

            if (receiveMonitorDataDetailView != null)
            {
                ReceiveMonitorOverviewViewToReceiveMonitorDataDetailViewTranslator receiveMonitorOverviewViewToReceiveMonitorDataDetailViewTranslator = null;

                if (WorkItem.Items.FindByType<ReceiveMonitorOverviewViewToReceiveMonitorDataDetailViewTranslator>().Count > 0)
                    receiveMonitorOverviewViewToReceiveMonitorDataDetailViewTranslator = WorkItem.Items.FindByType<ReceiveMonitorOverviewViewToReceiveMonitorDataDetailViewTranslator>().Last();
                else
                    receiveMonitorOverviewViewToReceiveMonitorDataDetailViewTranslator = WorkItem.Items.AddNew<ReceiveMonitorOverviewViewToReceiveMonitorDataDetailViewTranslator>();

                IList<ReceiveMonitorDataDetailViewResult> detailData = new List<ReceiveMonitorDataDetailViewResult>();
                if (overviewData != null)
                {
                    foreach (ReceiveMonitorOverviewViewResult overviewResult in overviewData)
                    {
                        detailData.Add(receiveMonitorOverviewViewToReceiveMonitorDataDetailViewTranslator.TranslateFromResultToResult(overviewResult));
                    }
                }

                if (receiveMonitorDataDetailView.IsEnabled)
                {
                    receiveMonitorDataDetailView.RefreshDataOnShow = false;
                    receiveMonitorDataDetailView.PresentData(detailData);
                    receiveMonitorDataDetailView.UpdateRowCount(detailData.Count, detailData.Count, false);
                }
                else
                {
                    receiveMonitorDataDetailView.Update(null);
                    receiveMonitorDataDetailView.UpdateRowCount(0, 0, false);
                }
            }
        }
    }	
}
