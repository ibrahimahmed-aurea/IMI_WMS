using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Deployment.Application;
using System.Configuration;
using System.Collections.Specialized;
using System.Web;
using System.Reflection;
using Microsoft.Practices.CompositeUI.Configuration;
using ActiproSoftware.Windows.Themes;
using Imi.SupplyChain.UX.Shell.Views;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.SupplyChain.UX.Shell.Services;
using Imi.SupplyChain.UX.Shell.Configuration;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.Framework.UX.Services;
using Imi.Framework.UX.Identity;
using System.Windows.Media;

namespace Imi.SupplyChain.UX.Shell
{
    public class LoginApplication
    {
        private UserSessionService _userSessionService;
        private HyperlinkService _hyperlinkService;
        private LoginController _loginController;
                        
        public LoginApplication()
        {
        }

        public void Run()
        {
            AddServices();

            if (_userSessionService.ActivationUri != null && _hyperlinkService.RedirectToExistingInstance(_userSessionService.ActivationUri))
            {
                return;
            }
            
            Application app = new Application();
                        
            _loginController = new LoginController();
            _loginController.UserSessionService = _userSessionService;
            
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Loaded += LoginWindowLoaded;
            app.Run(loginWindow);
        }
                
        private void LoginWindowLoaded(object sender, RoutedEventArgs e)
        {
            ThemeHelper.ApplyTheme(ThemeHelper.GetDefaultThemeSettings());
            _loginController.Run();
        }

        protected void AddServices()
        {
            _userSessionService = new UserSessionService();
                        
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                _userSessionService.ActivationUri = ApplicationDeployment.CurrentDeployment.ActivationUri;

                NameValueCollection args = HttpUtility.ParseQueryString(_userSessionService.ActivationUri.Query);
                                
                _userSessionService.InstanceName = args["instance"];
                _userSessionService.HostName = args["HostServerName"];
                _userSessionService.HostPort = Convert.ToInt32(args["HostPort"]);
                _userSessionService.CurrentVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;
            }
            else
            {
                ShellConfigurationSection config = ConfigurationManager.GetSection(ShellConfigurationSection.SectionKey) as ShellConfigurationSection;
                _userSessionService.InstanceName = config.InstanceNameLocalInstall;
                _userSessionService.HostName = config.HostName;
                _userSessionService.HostPort = config.HostPort;
                _userSessionService.CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version;
            }
                        
            _userSessionService.TerminalId = Environment.MachineName;
            _hyperlinkService = new HyperlinkService(_userSessionService);
        }
    }
}
