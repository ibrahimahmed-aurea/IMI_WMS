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
using Imi.SupplyChain.ActivityMonitor.UX.Modules.Movement.MovementMonitor.Translators;
using Imi.SupplyChain.UX.Modules.ActivityMonitor.Infrastructure.Services;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.Movement.MovementMonitor
{
    #region Compiler Settings
	  // Disable assigned property never used
	  #pragma warning disable 414 
	#endregion

    public class MovementMonitorEventAggregator : MovementMonitorEventAggregatorBase
    {
        public override void MovementMonitorOverviewViewUpdatedEventHandler(object sender, DataEventArgs<MovementMonitorOverviewViewResult> eventArgs)
        {
            IMovementMonitorDataDetailView movementMonitorDataDetailView = WorkItem.SmartParts.FindByType<IMovementMonitorDataDetailView>().Last() as IMovementMonitorDataDetailView;

            IList<MovementMonitorOverviewViewResult> overviewData = null;

            if (WorkItem.SmartParts.FindByType<IList<MovementMonitorOverviewViewResult>>().Count > 0)
            {
                overviewData = WorkItem.SmartParts.FindByType<IList<MovementMonitorOverviewViewResult>>().Last() as IList<MovementMonitorOverviewViewResult>;
            }

            if (movementMonitorDataDetailView != null)
            {
                MovementMonitorOverviewViewToMovementMonitorDataDetailViewTranslator movementMonitorOverviewViewToMovementMonitorDataDetailViewTranslator = null;

                if (WorkItem.Items.FindByType<MovementMonitorOverviewViewToMovementMonitorDataDetailViewTranslator>().Count > 0)
                    movementMonitorOverviewViewToMovementMonitorDataDetailViewTranslator = WorkItem.Items.FindByType<MovementMonitorOverviewViewToMovementMonitorDataDetailViewTranslator>().Last();
                else
                    movementMonitorOverviewViewToMovementMonitorDataDetailViewTranslator = WorkItem.Items.AddNew<MovementMonitorOverviewViewToMovementMonitorDataDetailViewTranslator>();

                IList<MovementMonitorDataDetailViewResult> detailData = new List<MovementMonitorDataDetailViewResult>();
                if (overviewData != null)
                {
                    foreach (MovementMonitorOverviewViewResult overviewResult in overviewData)
                    {
                        detailData.Add(movementMonitorOverviewViewToMovementMonitorDataDetailViewTranslator.TranslateFromResultToResult(overviewResult));
                    }
                }

                if (movementMonitorDataDetailView.IsEnabled)
                {
                    movementMonitorDataDetailView.RefreshDataOnShow = false;
                    movementMonitorDataDetailView.PresentData(detailData);
                    movementMonitorDataDetailView.UpdateRowCount(detailData.Count, detailData.Count, false);
                }
                else
                {
                    movementMonitorDataDetailView.Update(null);
                    movementMonitorDataDetailView.UpdateRowCount(0, 0, false);
                }
            }
        }
    }	
}
