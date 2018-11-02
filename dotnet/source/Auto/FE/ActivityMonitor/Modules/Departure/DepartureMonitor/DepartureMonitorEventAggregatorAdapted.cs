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
using Imi.SupplyChain.ActivityMonitor.UX.Modules.Departure.DepartureMonitor.Translators;
using Imi.SupplyChain.UX.Modules.ActivityMonitor.Infrastructure.Services;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.Departure.DepartureMonitor
{
    public class DepartureMonitorEventAggregator : DepartureMonitorEventAggregatorBase
    {
        [EventSubscription(EventTopicNames.DepartureMonitorOverviewViewUpdatedTopic)]
        public virtual void DepartureMonitorOverviewViewUpdatedEventHandler(object sender, DataEventArgs<DepartureMonitorOverviewViewResult> eventArgs)
        {
            IDepartureMonitorOverviewView departureOverView = WorkItem.Items.FindByType<IDepartureMonitorOverviewView>().Last();

            if (departureOverView != null)
            {
                string valueBinding = departureOverView.ValueBinding;

                IList<DepartureMonitorOverviewViewResult> overviewData = null;

                if (WorkItem.SmartParts.FindByType<IList<DepartureMonitorOverviewViewResult>>().Count > 0)
                {
                    overviewData = WorkItem.SmartParts.FindByType<IList<DepartureMonitorOverviewViewResult>>().Last() as IList<DepartureMonitorOverviewViewResult>;
                }


                IDepartureMonitorDetailView departureMonitorDetailView = WorkItem.Items.FindByType<IDepartureMonitorDetailView>().Last() as IDepartureMonitorDetailView;

                if (departureMonitorDetailView != null)
                {
                    DepartureMonitorOverviewViewToDepartureMonitorDetailViewTranslator departureMonitorOverviewViewToCasePickMonitorDataDetailViewTranslator = null;

                    if (WorkItem.Items.FindByType<DepartureMonitorOverviewViewToDepartureMonitorDetailViewTranslator>().Count > 0)
                    {
                        departureMonitorOverviewViewToCasePickMonitorDataDetailViewTranslator = WorkItem.Items.FindByType<DepartureMonitorOverviewViewToDepartureMonitorDetailViewTranslator>().Last();
                    }
                    else
                    {
                        departureMonitorOverviewViewToCasePickMonitorDataDetailViewTranslator = WorkItem.Items.AddNew<DepartureMonitorOverviewViewToDepartureMonitorDetailViewTranslator>();
                    }


                    IList<DepartureMonitorDetailViewResult> detailData = new List<DepartureMonitorDetailViewResult>();
                    if (overviewData != null)
                    {
                        foreach (DepartureMonitorOverviewViewResult overviewResult in overviewData)
                        {
                            detailData.Add(departureMonitorOverviewViewToCasePickMonitorDataDetailViewTranslator.TranslateFromResultToResult(overviewResult, valueBinding));
                        }
                    }

                    if (departureMonitorDetailView.IsEnabled)
                    {
                        departureMonitorDetailView.RefreshDataOnShow = false;
                        departureMonitorDetailView.PresentData(detailData);
                        departureMonitorDetailView.UpdateRowCount(detailData.Count, detailData.Count, false);
                    }
                    else
                    {
                        departureMonitorDetailView.Update(null);
                        departureMonitorDetailView.UpdateRowCount(0, 0, false);
                    }

                }
            }
        }
    }

    public class DepartureMonitorOverviewViewToDepartureMonitorDetailViewTranslator
    {
        public virtual DepartureMonitorDetailViewResult TranslateFromResultToResult(DepartureMonitorOverviewViewResult fromViewResult, string valueBinding)
        {
            DepartureMonitorDetailViewResult toViewResult = null;

            if (fromViewResult != null)
            {
                toViewResult = new DepartureMonitorDetailViewResult();

                toViewResult.DepartureId = fromViewResult.DepartureId;
                toViewResult.PlannedDepartureTime = fromViewResult.PlannedDepartureTime;
                toViewResult.RouteId = fromViewResult.RouteId;
                toViewResult.RouteDescription = fromViewResult.RouteDescription;
                toViewResult.ForwarderId = fromViewResult.ForwarderId;
                toViewResult.NumOfNotComposedOrders = fromViewResult.NumOfNotComposedOrders;
                toViewResult.NumOfNotReceivedTransits = fromViewResult.NumOfNotReceivedTransits;

                Type fromViewResultType = fromViewResult.GetType();

                toViewResult.OnShipLoc = ((decimal?)fromViewResultType.GetProperty(valueBinding + "OnShipLoc").GetValue(fromViewResult, null)).GetValueOrDefault();
                toViewResult.PickNotOnShipLoc = ((decimal?)fromViewResultType.GetProperty(valueBinding + "PickNotOnShipLoc").GetValue(fromViewResult, null)).GetValueOrDefault();
                toViewResult.PallNotOnShipLoc = ((decimal?)fromViewResultType.GetProperty(valueBinding + "PallNotOnShipLoc").GetValue(fromViewResult, null)).GetValueOrDefault();
                toViewResult.TransNotOnShipLoc = ((decimal?)fromViewResultType.GetProperty(valueBinding + "TransNotOnShipLoc").GetValue(fromViewResult, null)).GetValueOrDefault();
                toViewResult.OtherNotOnShipLoc = ((decimal?)fromViewResultType.GetProperty(valueBinding + "OtherNotOnShipLoc").GetValue(fromViewResult, null)).GetValueOrDefault();
            }

            return toViewResult;
        }
    }
}
