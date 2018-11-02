using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor
{
    public interface IPickZoneMonitorOverviewView : IPickZoneMonitorOverviewViewBase
    {
        void SaveFavoriteSettings(PickZoneMonitorControllerSettingsRepository settings);
        void LoadFavoriteSettings(PickZoneMonitorControllerSettingsRepository settings);
    }
}
