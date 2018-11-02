using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Views;

namespace Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor
{
    public interface IDepartureMonitorOverviewView : IDepartureMonitorOverviewViewBase
    {
        void SaveFavoriteSettings(DepartureMonitorControllerSettingsRepository settings);
        void LoadFavoriteSettings(DepartureMonitorControllerSettingsRepository settings);
        string ValueBinding { get; }
    }
}
