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
using Microsoft.Practices.CompositeUI;
using ActiproSoftware.Windows.Controls.Ribbon;

namespace Imi.SupplyChain.UX.Shell.Views
{
    /// <summary>
    /// Interaction logic for ChooseDefaultWarehouseView.xaml
    /// </summary>
    public partial class AddWebPageView : RibbonWindow, IAddWebPageView
    {
        private AddWebPagePresenter _presenter;

        [CreateNew]
        public AddWebPagePresenter Presenter
        {
            get { return _presenter; }
            set
            {
                _presenter = value;
                _presenter.View = this;
            }
        }

        public string Title { get; set; }
        public string Address { get; set; }

        public AddWebPageView()
        {
            InitializeComponent();
            this.Owner = System.Windows.Application.Current.MainWindow;
            IsGlassEnabled = ((RibbonWindow)Application.Current.MainWindow).IsGlassEnabled;
            titleTextBox.Focus();
        }
                
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Address = addressTextBox.Text;
            Title = titleTextBox.Text;
            DialogResult = true;
            Close();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void titleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Validate();
        }

        private void Validate()
        {
            OkButton.IsEnabled = (!string.IsNullOrEmpty(titleTextBox.Text) && !string.IsNullOrEmpty(addressTextBox.Text));
        }

        private void addressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Validate();
        }

    }
}
