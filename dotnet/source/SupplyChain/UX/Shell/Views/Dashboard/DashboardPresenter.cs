using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Shell.Settings;
using Imi.SupplyChain.UX.Shell.Services;
using System.Timers;
using System.Windows;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public class DashboardPresenter : Presenter<IDashboardView>, IBuilderAware
    {
        [EventPublication(EventTopicNames.DashboardRefresh, PublicationScope.Global)]
        public event EventHandler<EventArgs> DashboardRefresh;

        private Timer _timer;

        public DashboardPresenter()
        {
            _timer = new Timer();
            _timer.Elapsed += new ElapsedEventHandler(TimerElapsedEventHandler);
        }

        [EventSubscription(EventTopicNames.SettingsUpdated)]
        public void Refresh(object sender, EventArgs e)
        {
            _timer.Stop();

            if (DashboardSettingsProvider.CurrentSettings.RefreshInterval > 0)
            {
                _timer.Interval = DashboardSettingsProvider.CurrentSettings.RefreshInterval * 1000;
                _timer.Start();
            }
        }

        private void TimerElapsedEventHandler(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Refresh();
            }));
        }

        public void Refresh()
        {
            if (DashboardRefresh != null)
            {
                DashboardRefresh(this, null);
            }
        }
        
        [ServiceDependency]
        public IHyperlinkService HyperlinkService
        {
            get;
            set;
        }

        [ServiceDependency]
        public IShellModuleService ShellModuleServie
        {
            get;
            set;
        }
                
        [ServiceDependency]
        public IUXSettingsService SettingsService
        {
            get;
            set;
        }

        public DashboardSettingsRepository Settings { get; set; }

        public override void OnViewReady()
        {
            SettingsService.AddProvider(View, new DashboardSettingsProvider());
        }

        private void LoadSettings()
        {
            if (Settings != null)
            {
                try
                {
                    foreach (ShellHyperlink hyperlink in Settings.ActivationLinks)
                    {
                        try
                        {
                            if (hyperlink.ModuleId == DashboardModule.ModuleId)
                            {
                                IWebView webView = WorkItem.SmartParts.AddNew<WebView>();
                                webView.SetUrl(hyperlink.Data["Title"], hyperlink.Link);
                                View.Show(webView);
                            }
                            else
                            {
                                HyperlinkService.ExecuteHyperlink(hyperlink);
                            }
                        }
                        catch
                        {
                        }
                    }

                    View.LoadLayout();

                    if (Settings.RefreshInterval > 0)
                    {
                        _timer.Interval = Settings.RefreshInterval * 1000;
                        _timer.Start();
                    }
                }
                finally
                {
                    Settings = null;
                }
            }
        }
        
        #region IBuilderAware Members

        public override void OnBuiltUp(string id)
        {
            base.OnBuiltUp(id);
                       
            ShellModuleServie.ModuleActivated += (s, e) =>
            {
                if (e.Data.Id == DashboardModule.ModuleId)
                {
                    LoadSettings();
                }
            };
        }

        public override void OnTearingDown()
        {
            base.OnTearingDown();

            _timer.Stop();
            _timer.Close();
        }

        #endregion
    }
}
