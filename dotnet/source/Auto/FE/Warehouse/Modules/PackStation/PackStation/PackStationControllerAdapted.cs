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
using Imi.SupplyChain.UX.Modules.Warehouse.Infrastructure.Services;
using Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier;
using Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier.Constants;
using Imi.SupplyChain.Warehouse.UX.Modules.PackStation.PackStation.Translators;

namespace Imi.SupplyChain.Warehouse.UX.Modules.PackStation.PackStation
{
	public class PackStationController : PackStationControllerBase
    {
        [EventSubscription(Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier.Constants.EventTopicNames.PackStationOverviewStopPackingTopic)]
        public void PackStationOverviewStopPackingEventHandler(object sender, EventArgs eventArgs)
        {
            ActionCatalog.Execute(WorkItem.Items.FindByType<PackStationActions>().LastOrDefault().PackStationOverviewRunStopPacking, WorkItem, this, null);
        }

        [EventSubscription(Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier.Constants.EventTopicNames.PackStationOverviewPackRowTopic)]
        public void PackStationOverviewPackRowEventHandler(object sender, EventArgs eventArgs)
        {
            if (eventArgs is Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier.PackStationOverviewPackRowEventArgs)
            {
                ActionCatalog.Execute(WorkItem.Items.FindByType<PackStationActions>().LastOrDefault().PackStationOverviewRunPackRowForPackStation, WorkItem, this, eventArgs as Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier.PackStationOverviewPackRowEventArgs);
            }
        }
    
	}       
}

