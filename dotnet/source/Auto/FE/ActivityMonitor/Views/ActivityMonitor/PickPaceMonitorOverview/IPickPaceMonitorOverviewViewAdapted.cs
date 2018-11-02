using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Views;

namespace Imi.SupplyChain.ActivityMonitor.UX.Views.ActivityMonitor
{   
    public interface IPickPaceMonitorOverviewView : IPickPaceMonitorOverviewViewBase
    {
        void SaveFavoriteSettings(PickPaceMonitorControllerSettingsRepository settings);
        void LoadFavoriteSettings(PickPaceMonitorControllerSettingsRepository settings);
    }
}
