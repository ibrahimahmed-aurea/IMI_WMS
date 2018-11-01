using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI.SmartParts;
using Imi.Framework.Wpf.Controls;
using Imi.SupplyChain.UX.Shell.Settings;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public interface IDashboardView : IWorkspace
    {
        void LoadUserSettings(DashboardSettingsRepository settings);
        void SaveUserSettings(DashboardSettingsRepository settings);
        void LoadLayout();
        void Refresh();
        void Arrange();
    }
}
