using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Warehouse.UX.Modules.ItemLoad.Actions
{
    public class RunCreateItemLoadOutlineAction : RunCreateItemLoadOutlineActionBase
    {
        public override void OnRunCreateItemLoadOutline(Microsoft.Practices.CompositeUI.WorkItem context, object caller, object target)
        {
            base.OnRunCreateItemLoadOutline(context, caller, target);

            RunCreateItemLoadOutlineActionParameters actionParameters = target as RunCreateItemLoadOutlineActionParameters;

            if (context.Parent.Items.Contains("UserId"))
                context.Parent.Items.Remove(context.Parent.Items["UserId"]);

            context.Parent.Items.Add(actionParameters.UserId, "UserId");
        }
    }
}
