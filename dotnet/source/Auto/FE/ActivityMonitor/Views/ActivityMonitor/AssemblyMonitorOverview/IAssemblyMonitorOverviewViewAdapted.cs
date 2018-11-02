// Generated from template: .\UX\View\ViewInterfaceTemplate.cst
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Views;
using System.Windows;

namespace Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor
{
    public interface IAssemblyMonitorOverviewView : IAssemblyMonitorOverviewViewBase
    {
        void SaveFavoriteSettings(AssemblyMonitorControllerSettingsRepository settings);
        void LoadFavoriteSettings(AssemblyMonitorControllerSettingsRepository settings);
	}
}
