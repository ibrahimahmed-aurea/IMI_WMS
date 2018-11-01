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
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.ActivityMonitor.UX.Modules.CasePick.Actions;
using Imi.SupplyChain.ActivityMonitor.UX.Modules.CasePick.Constants;
using Imi.SupplyChain.ActivityMonitor.UX.Modules.CasePick.CasePickMonitor;
using Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor;

namespace Imi.SupplyChain.ActivityMonitor.UX.Modules.CasePick
{
    public class ModuleController : ModuleControllerBase
    {
        public override void OnShowCasePickMonitorOverview(object sender, EventArgs e)
        {
            CasePickMonitorOverviewViewParameters viewParameters = null;
            bool openInNewWindow = false;
            string favoriteId = string.Empty;

            if (e is MenuEventArgs)
            {
                openInNewWindow = ((MenuEventArgs)e).OpenInNewWindow;
                string parameters = ((MenuEventArgs)e).Parameters;
                favoriteId = ((MenuEventArgs)e).MenuItemId;

                if (!string.IsNullOrEmpty(parameters))
                {
                    viewParameters = new CasePickMonitorOverviewViewParameters();
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
                    viewParameters = new CasePickMonitorOverviewViewParameters();
                    HyperlinkHelper.MapQueryString(parameters, viewParameters);
                }
            }
            else if (e is DataEventArgs<CasePickMonitorOverviewViewParameters>)
            {
                viewParameters = ((DataEventArgs<CasePickMonitorOverviewViewParameters>)e).Data;
            }

            ControlledWorkItem<CasePickMonitorController> workItem = (from wi in WorkItem.WorkItems.FindByType<ControlledWorkItem<CasePickMonitorController>>()
                                                                  where wi.Items.Get<string>("ModuleId") == ShellInteractionService.ActiveModule.Id
                                                                  select wi).LastOrDefault();

            if (workItem == null || openInNewWindow)
            {
                workItem = WorkItem.WorkItems.AddNew<ControlledWorkItem<CasePickMonitorController>>();
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
