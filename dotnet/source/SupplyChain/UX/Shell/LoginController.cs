using System;
using System.Windows;
using System.Threading;
using System.Deployment.Application;
using System.Configuration;
using System.Reflection;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Windows.Threading;
using System.Diagnostics;
using Microsoft.Practices.CompositeUI.SmartParts;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using Imi.Framework.Wpf.Controls;
using Imi.Framework.UX.Identity;
using Imi.SupplyChain.UX.Shell.Views;
using Imi.SupplyChain.UX.Shell.Services;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Shell
{
    public class LoginController
    {
        private LoginWindow _loginWindow;
        private LoginView _loginView;
        private WelcomeView _welcomeView;
        private EventWaitHandle _loadedEvent;
        private SecurityTokenCache _tokenCache;
        private Thread _shellThread;
        private IWorkspace _mainWorkspace;
                        
        public UserSessionService UserSessionService { get; set; }

        public LoginController()
        {
            _loadedEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
            _tokenCache = new SecurityTokenCache();
        }

        public void Run()
        {
            _loginWindow = Application.Current.MainWindow as LoginWindow;
            _loginWindow.Closing += LoginWindowClosingEventHandler;
            _mainWorkspace = _loginWindow.mainWorkspace;
            
            ShowLogin();
        }

        private void LoginWindowClosingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_shellThread != null)
            {
                _shellThread.Abort();
            }
        }

        private void ShowLogin(bool logout = false)
        {
            UserSessionService.UserId = "";

            _loginView = new LoginView();

            _loginView.Presenter = new LoginPresenter()
            {
                UserSessionService = UserSessionService,
                View = _loginView,
                Logout = logout
            };

            _loginView.Presenter.AttemptLogin += AttemptLogin;

            _mainWorkspace.Show(_loginView);
        }
                              
        public void AttemptLogin(object sender, EventArgs args)
        {
            _tokenCache.Flush();

            _mainWorkspace.Close(_loginView);
            _loginView = null;

            _welcomeView = new WelcomeView();

            _welcomeView.Presenter = new WelcomePresenter()
            {
                UserSessionService = UserSessionService,
                TokenCache = _tokenCache,
                View = _welcomeView
            };

            _welcomeView.Presenter.LoginFailed += LoginFailed;
            _welcomeView.Presenter.LoginSuccessful += LoginSuccessful;
            
            _mainWorkspace.Show(_welcomeView);
            _welcomeView.Login();
        }
                
        public void LoginFailed(object sender, EventArgs args)
        {
            if (_welcomeView != null)
            {
                _mainWorkspace.Close(_welcomeView);
                _welcomeView = null;
            }

            ShowLogin();
        }
                
        public void LoginSuccessful(object sender, EventArgs args)
        {
            _shellThread = new Thread(() =>
            {
                StartShell();
            });

            _shellThread.SetApartmentState(ApartmentState.STA);
            _shellThread.Start();
        }
                
        private void StartShell()
        {
            AppDomain domain = null;
            IntPtr password = default(IntPtr);
            bool logout = false;

            try
            {
                AppDomainSetup setup = new AppDomainSetup();
                setup.ConfigurationFile = UserSessionService.ConfigFilename;
                setup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string assemblyName = "Imi.SupplyChain.UX.Shell";
                string instanceTypeName = "Imi.SupplyChain.UX.Shell.ShellApplication";
                domain = AppDomain.CreateDomain("login", null, setup, new PermissionSet(PermissionState.Unrestricted));
                string securityTokenXml = null;

                if (_tokenCache.RawToken != null)
                {
                    securityTokenXml = SecurityTokenCache.Serialize(_tokenCache);
                }

                if (UserSessionService.Password != null)
                {
                    password = SecureStringHelper.GetString(UserSessionService.Password);
                    UserSessionService.Password = null;
                }

                object[] args = new object[4]
                    {
                        password,
                        UserSessionService,
                        _loadedEvent,
                        securityTokenXml
                    };

                CleanTempFiles();

                ThreadPool.QueueUserWorkItem(WaitForShellStartCallback);

                domain.CreateInstance(assemblyName, instanceTypeName, true, BindingFlags.CreateInstance, null, args, null, null);

                password = default(IntPtr);

                logout = domain.GetData("Logout") != null;
            }
            catch (ThreadAbortException)
            { 
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(StringResources.Title, ex.ToString(), EventLogEntryType.Error);
                throw;
            }
            finally
            {
                if (password != default(IntPtr))
                {
                    SecureStringHelper.FreeString(password);
                }
                
                if (domain != null)
                {
                    try
                    {
                        AppDomain.Unload(domain);
                    }
                    catch (CannotUnloadAppDomainException)
                    {
                    }
                    finally
                    {
                        domain = null;
                    }
                }
                
                try
                {
                    File.Delete(UserSessionService.ConfigFilename);
                }
                catch (Exception)
                {
                }

                CleanTempFiles();

                if (logout)
                {
                    _loginWindow.Dispatcher.Invoke(new Action(() =>
                    {
                        ShowLogin(true);
                        _loginWindow.Show();
                    }));
                }
                else
                {
                    _loginWindow.Closing -= LoginWindowClosingEventHandler;

                    _loginWindow.Dispatcher.Invoke(new Action(() =>
                    {
                        _loginWindow.Close();
                    }));
                }
            }
        }

        private void CleanTempFiles()
        {
            foreach (string tempFile in Directory.GetFiles(Path.GetTempPath(), "Imi_SC_*"))
            {
                try
                {
                    File.Delete(tempFile);
                }
                catch
                {
                }
            }
        }
                        
        private void WaitForShellStartCallback(object state)
        {
            _loadedEvent.WaitOne();

            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    _mainWorkspace.Close(_welcomeView);
                    _welcomeView = null;
                    _loginWindow.Hide();
                }));
            }
        }
    }
}
