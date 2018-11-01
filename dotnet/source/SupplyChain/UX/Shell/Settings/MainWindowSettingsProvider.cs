using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Wpf.Controls;
using Imi.Framework.UX.Settings;
using System.Windows;
using Imi.SupplyChain.UX.Shell.Views;

namespace Imi.SupplyChain.UX.Shell.Settings
{
    public class MainWindowSettingsProvider : ISettingsProvider
    {
        private static MainWindowSettingsRepository _repository;
        
        public MainWindowSettingsProvider()
        {
        }

        public static MainWindowSettingsRepository CurrentSettings
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
            MainWindowSettingsRepository repository = new MainWindowSettingsRepository();
            repository.UseDefaultWindowSettings = true;
            repository.ThemeSettings = ThemeHelper.GetDefaultThemeSettings();
            repository.IsCommonWorkspaceEnabled = true;

            if (settings != null)
                repository = (MainWindowSettingsRepository)settings;

            ((MainWindow)target).LoadUserSettings(repository);

            _repository = repository;
        }

        public object SaveSettings(object target)
        {
            ((MainWindow)target).SaveUserSettings(_repository);

            return _repository;
        }

        public Type GetSettingsType(object target)
        {
            return typeof(MainWindowSettingsRepository);
        }

        public string GetKey(object target)
        {
            return ((FrameworkElement)target).Name;
        }
    }
}
