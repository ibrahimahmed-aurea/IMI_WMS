// Generated from template: .\UX\View\ViewInterfaceTemplate.cst
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Views;

namespace Imi.SupplyChain.Warehouse.UX.Views.PickLoadCarrier
{
    public interface IPackStationOverviewView : IPackStationOverviewViewBase
    {
        List<PackStationOverviewViewResult> GetData { get; }
        string ToLoadCarrierId { get; set; }
        bool EANFocus { get;}
	}
}
