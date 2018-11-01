using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Imi.SupplyChain.UX.Shell.Settings;

namespace Imi.SupplyChain.UX.Shell.Views.Settings
{
    public interface IShellSettingsView
    {
        void Close();
        MainWindowSettingsRepository MainWindowSettings { get; set; }
        DashboardSettingsRepository DashboardSettings { get; set; }
        string ProductName { get; set; }
    }
}
