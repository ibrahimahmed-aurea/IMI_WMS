using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Modules.OrderManagement.Configuration;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Win32;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Views
{
    public partial class WebView : UserControl, IWebView, IBuilderAware, IActionProvider, ISmartPartInfoProvider
    {
        private WebViewPresenter presenter;
        private OMSConfigurationSection configSection;
        public string menuId { get; set; }
        public string menuDescription { get; set; }
        public string oneTimePassword { get; set; }
        public string logicalUser { get; set; }
        public string loginId { get; set; }
                       
        [CreateNew]
        public WebViewPresenter Presenter
        {
            get { return presenter; }
            set
            {
                presenter = value;
                presenter.View = this;
            }
        }

        public WebView()
        {
            InitializeComponent();
            configSection = ConfigurationManager.GetSection("imi.supplychain.ux.modules.ordermanagement") as OMSConfigurationSection;
        }

        public void PresentData(object data)
        {
        }

        public void Update(object parameters)
        { 
        }

        #region IBuilderAware Members

        public void OnBuiltUp(string id)
        {
            presenter.OnViewReady();
        }

        public void OnTearingDown()
        {

        }

        #endregion


        #region IDataView Members


        public new bool IsVisible
        {
            get
            {
                return base.IsVisible;
            }
            set
            {
                if (value)
                    this.Visibility = Visibility.Visible;
                else
                    this.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        private void WebBrowserLoaded(object sender, RoutedEventArgs e)
        {
            string configKey = menuId + "_URL";
            string urlFromConfiguration = configSection.getConfigValue(configKey);
            if (urlFromConfiguration == null)
            {
                throw new Exception("Error starting web application, configuration parameter " + configKey + " is missing");
            }
            string currentUrl = this.MainBrowser.Source != null ? this.MainBrowser.Source.ToString() : null;

            string smartClientParam = urlFromConfiguration.IndexOf('?') == -1 ? "?smartClient=true" : "&smartClient=true";
            string encodedOmsKickstartURLParam = "&kickstartURL=" + System.Web.HttpUtility.UrlEncode(configSection.KickstartURL);
            string otpParam = String.IsNullOrEmpty(oneTimePassword) ? "" : "&otp="+oneTimePassword;
            string logicalUserParam = "&userId=" + logicalUser;
            string loginUserParam = "&loginId=" + System.Web.HttpUtility.UrlEncode(loginId);

            if (currentUrl == null || !currentUrl.StartsWith(urlFromConfiguration))
            {
                string completeURL = urlFromConfiguration + smartClientParam + otpParam + logicalUserParam + loginUserParam + encodedOmsKickstartURLParam;
                this.MainBrowser.Navigate(completeURL);
            }
        }

         void WebBrowserNavigated(object sender, NavigationEventArgs e)
        {
            HideScriptErrors(this.MainBrowser);
        }

        // Hides javascript errors
        private void HideScriptErrors(WebBrowser browser)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) 
                return;

            object objComWebBrowser = fiComWebBrowser.GetValue(browser);
            if (objComWebBrowser != null)
            {
                objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { true });
            }
        }

 

        #region IActionProvider Members
        public ICollection<ShellAction> GetActions()
        {
            return presenter.GetActions();
        }
        #endregion

        #region ISmartPartInfoProvider Members
        public ISmartPartInfo GetSmartPartInfo(Type smartPartInfoType)
        {
            ShellHyperlink hyperlink = new ShellHyperlink();
            hyperlink.ModuleId = ModuleController.ModuleId;
            hyperlink.Data.Add("ProgramType", "anywhere");
            hyperlink.Data.Add("DialogId", menuId);
            hyperlink.Data.Add("DialogDescription", menuDescription);
            hyperlink.Data.Add("Parameters", null);
            return new ShellSmartPartInfo("Title", "Description", hyperlink);
        }
        #endregion
    }
}
