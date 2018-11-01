using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Wpf.Controls;
using Imi.Framework.UX.Settings;

namespace Imi.SupplyChain.UX.Settings
{
    public class SearchPanelSettingsProvider : ISettingsProvider
    {
        public SearchPanelSettingsProvider()
        {
        }
                
        public void LoadSettings(object target, object settings)
        {
            if (settings != null)
                ((SearchPanel)target).LoadUserSettings((SearchPanelSettingsRepository)settings);
        }

        public object SaveSettings(object target)
        {
            SearchPanelSettingsRepository settingsRepository = new SearchPanelSettingsRepository();
            ((SearchPanel)target).SaveUserSettings(settingsRepository);

            return settingsRepository;
        }
               
        public Type GetSettingsType(object target)
        {
            return typeof(SearchPanelSettingsRepository);
        }

        public string GetKey(object target)
        {
            return ((SearchPanel)target).Name;
        }
    }
}
