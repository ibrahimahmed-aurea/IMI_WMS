// Generated from template: .\UX\View\ViewInterfaceTemplate.cst
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Views;

namespace Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor
{
    public interface IPackMonitorOverviewView : IPackMonitorOverviewViewBase
    {
        void SaveFavoriteSettings(PackMonitorControllerSettingsRepository settings);
        void LoadFavoriteSettings(PackMonitorControllerSettingsRepository settings);
	}
}
