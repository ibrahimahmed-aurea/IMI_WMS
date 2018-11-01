using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Warehouse.UX.Modules.ItemLoad.ItemLoadOutline
{
    public class ItemLoadOutlineController : ItemLoadOutlineControllerBase
    {
        protected override void InitializeSearchPanel(Imi.SupplyChain.Warehouse.UX.Views.ItemLoadOutline.ItemLoadOutlineOverviewViewParameters parameters)
        {
            if (WorkItem.Items.Contains("UserId"))
            {
                parameters = new Views.ItemLoadOutline.ItemLoadOutlineOverviewViewParameters();
                parameters.UserId = WorkItem.Items.Get<string>("UserId");
            }

            base.InitializeSearchPanel(parameters);
        }
    }  
}
