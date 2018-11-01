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
using Imi.SupplyChain.ActivityMonitor.UX.Modules.PickPace.PickPaceMonitor.Translators;
using Imi.SupplyChain.UX.Modules.ActivityMonitor.Infrastructure.Services;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.PickPace.PickPaceMonitor
{
    #region Compiler Settings
    // Disable assigned property never used
#pragma warning disable 414
    #endregion

    //Extract the following class to PickPaceMonitorEventAggregatorAdapted.cs in order to customize its behavior        
    public class PickPaceMonitorEventAggregator : PickPaceMonitorEventAggregatorBase
    {
        public override void PickPaceMonitorOverviewViewUpdatedEventHandler(object sender, DataEventArgs<PickPaceMonitorOverviewViewResult> eventArgs)
        {
            IList<PickPaceMonitorOverviewViewResult> overviewData = null;

            if (WorkItem.SmartParts.FindByType<IList<PickPaceMonitorOverviewViewResult>>().Count > 0)
            {
                overviewData = WorkItem.SmartParts.FindByType<IList<PickPaceMonitorOverviewViewResult>>().Last() as IList<PickPaceMonitorOverviewViewResult>;
            }


            IPickPaceMonitorDetailView pickPaceMonitorDetailView = WorkItem.Items.FindByType<IPickPaceMonitorDetailView>().Last() as IPickPaceMonitorDetailView;

            if (pickPaceMonitorDetailView != null)
            {
                PickPaceMonitorOverviewViewToPickPaceMonitorDetailViewTranslator pickPaceMonitorOverviewViewToPickPaceMonitorDetailViewTranslator = null;

                if (WorkItem.Items.FindByType<PickPaceMonitorOverviewViewToPickPaceMonitorDetailViewTranslator>().Count > 0)
                    pickPaceMonitorOverviewViewToPickPaceMonitorDetailViewTranslator = WorkItem.Items.FindByType<PickPaceMonitorOverviewViewToPickPaceMonitorDetailViewTranslator>().Last();
                else
                    pickPaceMonitorOverviewViewToPickPaceMonitorDetailViewTranslator = WorkItem.Items.AddNew<PickPaceMonitorOverviewViewToPickPaceMonitorDetailViewTranslator>();

                IList<PickPaceMonitorDetailViewResult> detailData = new List<PickPaceMonitorDetailViewResult>();
                if (overviewData != null)
                {
                    foreach (PickPaceMonitorOverviewViewResult overviewResult in overviewData)
                    {
                        detailData.Add(pickPaceMonitorOverviewViewToPickPaceMonitorDetailViewTranslator.TranslateFromResultToResult(overviewResult));
                    }
                }

                if (pickPaceMonitorDetailView.IsEnabled)
                {
                    pickPaceMonitorDetailView.RefreshDataOnShow = false;
                    pickPaceMonitorDetailView.PresentData(detailData);
                    pickPaceMonitorDetailView.UpdateRowCount(detailData.Count, detailData.Count, false);
                }
                else
                {
                    pickPaceMonitorDetailView.Update(null);
                    pickPaceMonitorDetailView.UpdateRowCount(0, 0, false);
                }
            }
        }
    }
}
