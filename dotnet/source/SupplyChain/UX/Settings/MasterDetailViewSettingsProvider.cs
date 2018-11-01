using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Wpf.Controls;
using Imi.SupplyChain.UX.Views;
using Imi.Framework.UX.Settings;
using System.Windows;

namespace Imi.SupplyChain.UX.Settings
{
    public class MasterDetailViewSettingsProvider : ISettingsProvider
    {
        public MasterDetailViewSettingsProvider()
        { 
        }
                
        public void LoadSettings(object target, object settings)
        {
            MasterDetailViewSettingsRepository repository = new MasterDetailViewSettingsRepository();
            repository.DetailRowHeight = 350;

            if (settings != null)
                repository = (MasterDetailViewSettingsRepository)settings;
            
            ((IMasterDetailView)target).LoadUserSettings(repository);
        }

        public object SaveSettings(object target)
        {
            MasterDetailViewSettingsRepository settings = new MasterDetailViewSettingsRepository();

            ((IMasterDetailView)target).SaveUserSettings(settings);

            return settings;
        }

        public Type GetSettingsType(object target)
        {
            return typeof(MasterDetailViewSettingsRepository);
        }

        public string GetKey(object target)
        {
            return ((FrameworkElement)target).Name;
        }
    }
}
