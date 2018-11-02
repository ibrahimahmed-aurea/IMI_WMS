using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Services;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using Imi.Framework.Wpf.Controls;
using Imi.Framework.Services;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using ActiproSoftware.Windows.Controls.Ribbon;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Services;
using Imi.SupplyChain.UX.Views;
using Imi.SupplyChain.UX.Modules.Warehouse.Infrastructure.Services;
using Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier.Constants;
using Imi.SupplyChain.Warehouse.UX.Contracts.PickLoadCarrier.ServiceContracts;
using Imi.SupplyChain.Warehouse.UX.Contracts.PickLoadCarrier.DataContracts;
using Imi.SupplyChain.Warehouse.Services.Alarm.DataContracts;
using Imi.SupplyChain.Warehouse.Services.Alarm.ServiceContracts;

namespace Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier
{
    public class PackStationFromLcEditPresenter : PackStationFromLcEditPresenterBase
    {
        [InjectionConstructor]
        public PackStationFromLcEditPresenter([WcfServiceDependency] IPickLoadCarrierService pickLoadCarrierService)
            : base(pickLoadCarrierService)
        {
        }

        public override void OnViewShow()
        {
            if ((View.RefreshDataOnShow) || (forceUpdate))
            {
                View.Refresh();
            }

            

            IPackStationOverviewView packStationOverviewView = WorkItem.SmartParts.FindByType<IPackStationOverviewView>().LastOrDefault();
            if (packStationOverviewView != null)
            {
                if (packStationOverviewView.EANFocus)
                {
                    packStationOverviewView.SetFocus();
                }
                else
                {
                    View.SetFocus();
                }
            }
            else
            {
                View.SetFocus();
            }

        }
    }


}
