using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.CompositeUI;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace Imi.SupplyChain.UX.Shell.Views
{
	/// <summary>
	/// Interaction logic for PopularSettingsxaml.xaml
	/// </summary>
	public partial class ResourcesView : UserControl
	{
        public event EventHandler<EventArgs> AboutClicked;
        public event EventHandler<EventArgs> ContactUsClicked;



        public string ProductName
        {
            get { return (string)GetValue(ProductNameProperty); }
            set { SetValue(ProductNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProductName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductNameProperty =
            DependencyProperty.Register("ProductName", typeof(string), typeof(ResourcesView), new UIPropertyMetadata(""));


		public ResourcesView()
		{
			this.InitializeComponent();
		}

        private void AboutButtonClick(object sender, RoutedEventArgs e)
        {
            OnAbout();
        }

        private void ContactUsButtonClick(object sender, RoutedEventArgs e)
        {
            OnContactUs();
        }

        private void OnContactUs()
        {
            var temp = ContactUsClicked;

            if (temp != null)
                temp(this, new EventArgs());
        }
    
        private void OnAbout()
        {
            var temp = AboutClicked;

            if (temp != null)
                temp(this, new EventArgs());
        }
    }
}