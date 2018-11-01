using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.Warehouse.UX.Contracts.ItemLoad.ServiceContracts;

namespace Imi.SupplyChain.Warehouse.UX.Views.ItemLoadOutline
{
    //Extract the following class to ItemLoadOutlineOverviewPresenterAdapted.cs in order to customize its behavior    
    public class ItemLoadOutlineOverviewPresenter : ItemLoadOutlineOverviewPresenterBase
    {
        [InjectionConstructor]
        public ItemLoadOutlineOverviewPresenter([WcfServiceDependency] IItemLoadService itemLoadService)
            : base(itemLoadService)
        {
        }

        public override void UpdateView(ItemLoadOutlineOverviewViewParameters viewParameters)
        {
            if (viewParameters != null && WorkItem.Items.Contains("UserId"))
            {
                viewParameters.UserId = WorkItem.Items.Get<string>("UserId");
                WorkItem.Items.Remove(WorkItem.Items.Get("UserId"));
            }

            base.UpdateView(viewParameters);
        }
    }
}
