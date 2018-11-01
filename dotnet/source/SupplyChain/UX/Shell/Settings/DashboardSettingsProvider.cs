using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Imi.Framework.Wpf.Controls;
using Imi.Framework.UX.Settings;
using Imi.SupplyChain.UX.Shell.Views;

namespace Imi.SupplyChain.UX.Shell.Settings
{
    public class DashboardSettingsProvider : ISettingsProvider
    {
        private static DashboardSettingsRepository _repository;

        public DashboardSettingsProvider()
        {
        }

        public static DashboardSettingsRepository CurrentSettings
        {
            get
            {
                return _repository;
            }
            set
            {
                _repository = value;
            }
        }
               
        public void LoadSettings(object target, object settings)
        {
            DashboardSettingsRepository repository = new DashboardSettingsRepository();
            repository.RefreshInterval = 0;
            
            if (settings != null)
                repository = (DashboardSettingsRepository)settings;

            ((IDashboardView)target).LoadUserSettings(repository);

            _repository = repository;
        }

        public object SaveSettings(object target)
        {
            ((IDashboardView)target).SaveUserSettings(_repository);

            return _repository;
        }

        public Type GetSettingsType(object target)
        {
            return typeof(DashboardSettingsRepository);
        }

        public string GetKey(object target)
        {
            return ((FrameworkElement)target).Name;
        }
    }
}
