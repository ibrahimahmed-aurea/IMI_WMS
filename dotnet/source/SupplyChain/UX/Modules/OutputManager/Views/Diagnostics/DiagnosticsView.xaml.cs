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
using Imi.Framework.Wpf.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Practices.CompositeUI;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Modules.OutputManager.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class DiagnosticsView : RibbonWindow, IDiagnosticsView
    {
        private DiagnosticsPresenter presenter;

        [ServiceDependency]
        public IShellInteractionService ShellInteractionService { get; set; }

        [CreateNew]
        public DiagnosticsPresenter Presenter
        {
            get { return presenter; }
            set
            {
                presenter = value;
                presenter.View = this;

                EnableDisableButtons();
            }
        }
               
        public DiagnosticsView()
        {
            this.Owner = Application.Current.MainWindow;
            InitializeComponent();
            IsGlassEnabled = ((RibbonWindow)Application.Current.MainWindow).IsGlassEnabled;

        }
                
        private void StartDBTraceButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ShellInteractionService.ShowProgress();
                presenter.StartDatabaseTracing();
            }
            finally
            {
                ShellInteractionService.HideProgress();
            }

            EnableDisableButtons();
        }

        private void StopDBTraceButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ShellInteractionService.ShowProgress();
                presenter.StopDatabaseTracing();
            }
            finally
            {
                ShellInteractionService.HideProgress();
            }

            EnableDisableButtons();
        }

        private void RefreshInterfaceTraceStatus()
        {
            try
            {
                ShellInteractionService.ShowProgress();

                string ServerName;
                string LoggPathDirectory;

                presenter.GetServerInformation(out ServerName, out LoggPathDirectory);

                ServerNameTextBox.Text = ServerName;
                DirectoryTextBox.Text = LoggPathDirectory;
            }
            finally
            {
                ShellInteractionService.HideProgress();
            }
        }

        private void EnableDisableButtons()
        {
            if (presenter.IsDatabaseTracingEnabled)
            {
                startDBTraceButton.IsEnabled = false;
                stopDBTraceButton.IsEnabled = true;
            }
            else
            {
                startDBTraceButton.IsEnabled = true;
                stopDBTraceButton.IsEnabled = false;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshInterfaceTraceStatus();
        }
    }
}
