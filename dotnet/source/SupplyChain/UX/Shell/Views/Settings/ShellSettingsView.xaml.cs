using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ActiproSoftware.Windows.Controls.Ribbon;
using Microsoft.Practices.ObjectBuilder;
using Imi.SupplyChain.UX.Shell.Views.Settings;
using Imi.SupplyChain.UX.Shell.Settings;

namespace Imi.SupplyChain.UX.Shell.Views
{
 
    public partial class ShellSettingsView : RibbonWindow, IShellSettingsView
    {
        private ShellSettingsPresenter _presenter;
        
        [CreateNew]
        public ShellSettingsPresenter Presenter
        {
            get { return _presenter; }
            set
            {
                _presenter = value;
                _presenter.View = this;
            }
        }

        public ShellSettingsView()
        {
            this.InitializeComponent();
            IsGlassEnabled = ((RibbonWindow)Application.Current.MainWindow).IsGlassEnabled;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            resourcesView.AboutClicked += new EventHandler<EventArgs>(AboutClicked);
            resourcesView.ContactUsClicked += new EventHandler<EventArgs>(ContactUsClicked);
        }

        private void ContactUsClicked(object sender, EventArgs e)
        {
            Presenter.ContactUs();
        }

        private void AboutClicked(object sender, EventArgs e)
        {
            Presenter.About();
        }
        
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Presenter.CancelChanges();
        }

        private void OKButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Presenter.SaveChanges();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (!DialogResult.GetValueOrDefault())
            {
                Presenter.CancelChanges();
            }
        }
                
        public string ProductName { get; set; }

        public MainWindowSettingsRepository MainWindowSettings
        {
            get
            {
                return popularView.MainWindowSettings;
            }
            set
            {
                popularView.MainWindowSettings = value;
            }
        }

        public DashboardSettingsRepository DashboardSettings
        {
            get
            {
                return popularView.DashboardSettings;
            }
            set
            {
                popularView.DashboardSettings = value;
            }
        }
        
        private void ViewSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (popularView == null)
            {
                return;
            }

            if (sender == popularItem)
            {
                popularView.Visibility = Visibility.Visible;
                resourcesView.Visibility = Visibility.Collapsed;
                diagnosticsView.Visibility = Visibility.Collapsed;
            }

            if (sender == resourcesItem)
            {
                popularView.Visibility = Visibility.Collapsed;
                diagnosticsView.Visibility = Visibility.Collapsed;
                resourcesView.Visibility = Visibility.Visible;
            }

            if (sender == diagnosticsItem)
            {
                popularView.Visibility = Visibility.Collapsed;
                resourcesView.Visibility = Visibility.Collapsed;
                diagnosticsView.Visibility = Visibility.Visible;
            }
        }
    }
}