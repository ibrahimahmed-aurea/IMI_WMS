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
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace Imi.SupplyChain.UX.Shell.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    [SmartPart]
    public partial class AboutView : Window, IAboutView
    {
        private AboutPresenter presenter;
                
        public string Version
        {
            get { return (string)GetValue(VersionProperty); }
            set { SetValue(VersionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Version.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VersionProperty =
            DependencyProperty.Register("Version", typeof(string), typeof(AboutView), new UIPropertyMetadata(""));

        [CreateNew]
        public AboutPresenter Presenter
        {
            get { return presenter; }
            set
            {
                presenter = value;
                presenter.View = this;
            }
        }

        public AboutView()
        {
            InitializeComponent();
        }

        private void SystemInfoButtonPressed(object sender, RoutedEventArgs e)
        {
            presenter.StartSystemInfo();
        }

        private void OKButtonClick(object sender, RoutedEventArgs e)
        {
            presenter.CloseView();
        }

        private void CopyInfoButtonClick(object sender, RoutedEventArgs e)
        {
            presenter.CopyInfo();
        }

        public void SetModules(IList<string> appIdentifiers)
        {
            // TODO bind to application list create datatemplates
            ApplicationList.ItemsSource = appIdentifiers;
        }

        private Cursor oldCursor;

        public void SetWaitCursor()
        {
            oldCursor = this.Cursor;
            this.Cursor = Cursors.Wait;
        }

        public void SetNormalCursor()
        {
            this.Cursor = oldCursor;
        }

    }
}
