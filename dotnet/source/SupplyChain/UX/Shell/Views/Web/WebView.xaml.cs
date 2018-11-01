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
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using Imi.SupplyChain.UX.Infrastructure;

namespace Imi.SupplyChain.UX.Shell.Views
{
    /// <summary>
    /// Interaction logic for ExplorerView.xaml
    /// </summary>

    public partial class WebView : UserControl, IWebView, ISmartPartInfoProvider
    {
        private WebPresenter _presenter;
        private string _title;
        private string _url;
        private bool _refresh;

        [CreateNew]
        public WebPresenter Presenter
        {
            get { return _presenter; }
            set
            {
                _presenter = value;
                _presenter.View = this;
            }
        }

        public WebView()
        {
            InitializeComponent();
            browser.Navigated += BrowserNavigatedEventHandler;

            IsVisibleChanged += (s, e) =>
            {
                if ((bool)e.NewValue)
                {
                    if (_refresh)
                    {
                        Refresh();
                    }
                }
            };
        }

        private void BrowserNavigatedEventHandler(object sender, NavigationEventArgs e)
        {
            if (browser.Source != null)
            {
                addressTextBox.Text = browser.Source.AbsoluteUri;
            }

            backButton.IsEnabled = browser.CanGoBack;
            forwardButton.IsEnabled = browser.CanGoForward;
        }

        public ISmartPartInfo GetSmartPartInfo(Type smartPartInfoType)
        {
            ShellHyperlink hyperlink = null;
            
            if (browser.Source != null)
            {
                hyperlink = new ShellHyperlink();
                hyperlink.Data["Title"] = _title;
                hyperlink.ModuleId = DashboardModule.ModuleId;
                hyperlink.Link = _url;
            }

            ShellSmartPartInfo info = new ShellSmartPartInfo(_title, "", hyperlink);
            return info;
        }

        public void Refresh()
        {
            if (IsVisible)
            {
                _refresh = false;
                browser.Refresh();
            }
            else
            {
                _refresh = true;
            }
        }
                
        public void SetUrl(string title, string url)
        {
            try
            {
                _title = title;
                _url = url;
                Navigate(url);
            }
            catch (Exception)
            {
            }
        }

        private void Navigate(string url)
        {
            if (!url.ToLower().StartsWith("http://") && !url.ToLower().StartsWith("https://"))
            {
                url = "http://" + url;
            }

            browser.Navigate(url);
        }

        private void BackButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                browser.GoBack();
            }
            catch
            {
            }
        }

        private void ForwardButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                browser.GoForward();
            }
            catch
            { 
            }
        }

        private void addressTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                Navigate(addressTextBox.Text);
            }
        }
    }
}