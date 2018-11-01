using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public partial class WelcomeView : UserControl, IWelcomeView 
	{
		public WelcomeView()
		{
			this.InitializeComponent();

            ProgressGrid.Visibility = Visibility.Visible;
            ErrorGrid.Visibility = Visibility.Collapsed;
            errorText.Text = "";
        }

        private WelcomePresenter presenter;

        public WelcomePresenter Presenter
        {
            get { return presenter; }
            set
            {
                presenter = value;
                presenter.View = this;
            }
        }

        #region ISmartPartInfoProvider Members

        public ISmartPartInfo GetSmartPartInfo(Type smartPartInfoType)
        {
            SmartPartInfo info = new SmartPartInfo("Welcome", "");
            return info;
        }

        #endregion

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            presenter.OnLoginFailed();
        }
                
        #region IWelcomeView Members

        public void ShowError(string error)
        {
            errorText.Text = error;
            ProgressGrid.Visibility = Visibility.Hidden;
            ErrorGrid.Visibility = Visibility.Visible;

        }

        public void Login()
        {
            presenter.Login();
        }

        #endregion
    }
}