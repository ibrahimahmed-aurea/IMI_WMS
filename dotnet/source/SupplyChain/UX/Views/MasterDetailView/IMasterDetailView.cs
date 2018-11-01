using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Wpf.Controls;

namespace Imi.SupplyChain.UX.Views
{
    public interface IMasterDetailView
    {
        void ShowInSearchWorkspace(object view);
        void ShowInMasterWorkspace(object view);
        void ShowInDetailWorkspace(object view);
        void ShowDetail(bool? show);
        bool HasDetailView { get; }
        void LoadUserSettings(MasterDetailViewSettingsRepository settings);
        void SaveUserSettings(MasterDetailViewSettingsRepository settings);
    }
}
