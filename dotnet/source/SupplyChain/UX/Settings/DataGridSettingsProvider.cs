using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Wpf.Controls;
using Xceed.Wpf.DataGrid.Settings;
using Imi.Framework.UX.Settings;

namespace Imi.SupplyChain.UX.Settings
{
    public class DataGridSettingsProvider : ISettingsProvider
    {
        public DataGridSettingsProvider()
        { 
        }
                
        public void LoadSettings(object target, object settings)
        {
            if (settings != null)
                ((DataGrid)target).LoadUserSettings((SettingsRepository)settings, UserSettings.All);
        }

        public object SaveSettings(object target)
        {
            DataGridSettingsRepository settings = new DataGridSettingsRepository();

            ((DataGrid)target).SaveUserSettings(settings, UserSettings.All);

            return settings;
        }

        public Type GetSettingsType(object target)
        {
            return typeof(DataGridSettingsRepository);
        }

        public string GetKey(object target)
        {
            return ((DataGrid)target).Name;
        }
    }
}
