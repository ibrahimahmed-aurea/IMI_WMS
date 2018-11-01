using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.UX;
using Imi.SupplyChain.UX.Shell.Services;
using Microsoft.Practices.CompositeUI;
using System.Diagnostics;
using System.Windows;
using System.Threading;
using Microsoft.Practices.CompositeUI.EventBroker;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.SupplyChain.UX.Shell.Settings;
using Imi.Framework.UX.Services;

namespace Imi.SupplyChain.UX.Shell.Views.Settings
{
    public class ShellSettingsPresenter : Presenter<IShellSettingsView>
    {
        private IShellView _shellView;

        [ServiceDependency]
        public IUserSessionService UserSessionService { get; set; }
                
        [ServiceDependency]
        public IUXSettingsService SettingsService { get; set; }

        [EventPublication(EventTopicNames.SettingsUpdated, PublicationScope.Global)]
        public event EventHandler<EventArgs> SettingsUpdated;
                
        public override void OnViewSet()
        {
            base.OnViewSet();

            _shellView = WorkItem.SmartParts.FindByType<IShellView>().Last();

            _shellView.ShowProgress();

            try
            {
                View.MainWindowSettings = (MainWindowSettingsRepository)MainWindowSettingsProvider.CurrentSettings.Clone();
                View.DashboardSettings = (DashboardSettingsRepository)DashboardSettingsProvider.CurrentSettings.Clone();
                View.ProductName = string.Format("{0} ({1})", StringResources.Title, ((UserSessionService)UserSessionService).CurrentVersion);
            }
            finally
            {
                _shellView.HideProgress();
            }
        }

        public void SaveChanges()
        {
            _shellView.ShowProgress();

            try
            {
                ThemeHelper.ApplyTheme(View.MainWindowSettings.ThemeSettings);
                MainWindowSettingsProvider.CurrentSettings = View.MainWindowSettings;
                DashboardSettingsProvider.CurrentSettings = View.DashboardSettings;
                
                _shellView.Model = _shellView.Model;

                if (SettingsUpdated != null)
                {
                    SettingsUpdated(this, new EventArgs());
                }

                CloseView();
            }
            finally
            {
                _shellView.HideProgress();
            }
        }
        
        public void CancelChanges()
        {
            _shellView.ShowProgress();

            try
            {
                ThemeHelper.ApplyTheme(MainWindowSettingsProvider.CurrentSettings.ThemeSettings);
                CloseView();
            }
            finally
            {
                _shellView.HideProgress();
            }
        }

        public override void CloseView()
        {
            base.CloseView();
            View.Close();
        }
        
        public void ContactUs()
        {
            ProcessStartInfo info = new ProcessStartInfo(StringResources.Settings_ResourcesSupportLink);

            info.UseShellExecute = true;
            info.CreateNoWindow = true;

            Process processChild = Process.Start(info);
        }

        public void About()
        {
            var aboutView = WorkItem.SmartParts.AddNew<AboutView>();

            try
            {
                aboutView.Owner = Application.Current.MainWindow;
                aboutView.ShowDialog();
            }
            finally
            {
                WorkItem.SmartParts.Remove(aboutView);
            }
        }
    }
}
