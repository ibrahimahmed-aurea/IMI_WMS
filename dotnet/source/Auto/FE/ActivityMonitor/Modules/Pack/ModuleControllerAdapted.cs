// Generated from template: .\UX\Module\ModuleControllerTemplate.cst
using System;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Commands;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.SmartParts;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.ActivityMonitor.UX.Modules.Pack.Actions;
using Imi.SupplyChain.ActivityMonitor.UX.Modules.Pack.Constants;
using Imi.SupplyChain.ActivityMonitor.UX.Modules.Pack.PackMonitor;
using Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor;
using Imi.SupplyChain.UX.Infrastructure;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.Pack
{
    public class ModuleController : ModuleControllerBase
    {
        public override void OnShowPackMonitorOverview(object sender, EventArgs e)
        {
            PackMonitorOverviewViewParameters viewParameters = null;
            bool openInNewWindow = false;
            string favoriteId = string.Empty;

            if (e is MenuEventArgs)
            {
                openInNewWindow = ((MenuEventArgs)e).OpenInNewWindow;
                string parameters = ((MenuEventArgs)e).Parameters;
                favoriteId = ((MenuEventArgs)e).MenuItemId;

                if (!string.IsNullOrEmpty(parameters))
                {
                    viewParameters = new PackMonitorOverviewViewParameters();
                    HyperlinkHelper.MapQueryString(parameters, viewParameters);
                }
            }
            else if (e is MenuItemExecutedEventArgs)
            {
                openInNewWindow = ((MenuItemExecutedEventArgs)e).OpenInNewWindow;
                string parameters = ((MenuItemExecutedEventArgs)e).MenuItem.Parameters;
                favoriteId = ((MenuItemExecutedEventArgs)e).MenuItem.Id;

                if (!string.IsNullOrEmpty(parameters))
                {
                    viewParameters = new PackMonitorOverviewViewParameters();
                    HyperlinkHelper.MapQueryString(parameters, viewParameters);
                }
            }
            else if (e is DataEventArgs<PackMonitorOverviewViewParameters>)
            {
                viewParameters = ((DataEventArgs<PackMonitorOverviewViewParameters>)e).Data;
            }

            ControlledWorkItem<PackMonitorController> workItem = (from wi in WorkItem.WorkItems.FindByType<ControlledWorkItem<PackMonitorController>>()
                                                                  where wi.Items.Get<string>("ModuleId") == ShellInteractionService.ActiveModule.Id
                                                                  select wi).LastOrDefault();

            if (workItem == null || openInNewWindow)
            {
                workItem = WorkItem.WorkItems.AddNew<ControlledWorkItem<PackMonitorController>>();
                workItem.Items.Add(ShellInteractionService.ActiveModule.Id, "ModuleId");
                workItem.Controller.Run(viewParameters, favoriteId);
            }
            else
            {
                workItem.Controller.Activate(viewParameters, favoriteId);
            }
        }
	}
}
