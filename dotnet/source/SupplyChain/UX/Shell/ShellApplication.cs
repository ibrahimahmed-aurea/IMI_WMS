using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Configuration;
using System.Linq;
using System.IO;
using System.Collections.Specialized;
using System.Windows.Markup;
using System.Security;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Services;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI.Configuration;
using Imi.Framework.UX;
using Imi.Framework.UX.Wpf;
using Imi.Framework.UX.Services;
using Imi.Framework.UX.Identity;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Shell.BuilderStrategies;
using Imi.SupplyChain.UX.Shell.Services;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.SupplyChain.UX.Shell.Configuration;
using Imi.SupplyChain.UX.Shell.Views;

namespace Imi.SupplyChain.UX.Shell
{
    public class ShellApplication : WpfWindowShellApplication<WorkItem, MainWindow>
    {
        private SecureString _password;
        private EventWaitHandle _loadedEvent;
        private UserSessionService _userSessionService;
        private IUXSettingsService _settingsService;
        private EventWaitHandle _settingsLoadedEvent;
        private Imi.Framework.UX.Identity.SecurityTokenCache _tokenCache;
        private const string ContainerName = "Imi.SupplyChain.UX.Shell";

        public ShellApplication(object password, UserSessionService userSessionService, EventWaitHandle loadedEvent, string securityTokenXml)
        {
            if (password != null)
            {
                _password = SecureStringHelper.GetSecureString(Marshal.PtrToStringBSTR((IntPtr)password));
                SecureStringHelper.FreeString((IntPtr)password);
            }

            _userSessionService = userSessionService;
            _loadedEvent = loadedEvent;

            if (securityTokenXml != null)
            {
                _tokenCache = Imi.Framework.UX.Identity.SecurityTokenCache.Deserialize(securityTokenXml);
            }
            else
            {
                _tokenCache = new Imi.Framework.UX.Identity.SecurityTokenCache();
            }
                        
            _settingsLoadedEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
            
            Thread.CurrentThread.CurrentUICulture = userSessionService.UICulture;
            
            Run();
        }
                                
        protected override void AddServices()
        {
            base.AddServices();
                        
            RootWorkItem.Services.Add<IUserSessionService>(_userSessionService);
            
            if (_userSessionService.ActivationUri != null)
            {
                ShellHyperlink hyperlink = HyperlinkService.ConvertToShellHyperlink(_userSessionService.ActivationUri);
                
                if (hyperlink.Data.ContainsKey("ModuleId"))
                {
                    //Add shellhyperlink to workitem to prevent dialogs being lodad before the module is fully loaded. See NavigationBarView.xaml.cs function NavigationBarSelectionChangedEventHandler
                    //Hyperlink is removed from workitem in HyperlinkService.cs function ExecuteHyperlink(ShellHyperlink hyperlink)
                    RootWorkItem.Items.Add(hyperlink);
                }
            }
                                                
            if (string.IsNullOrEmpty(_userSessionService.DomainUser))
            {
                _userSessionService.UserId = string.Format("{0}\\{1}", Environment.UserDomainName, Environment.UserName);
            }
            else
            {
                _userSessionService.Password = _password;
            }

            RootWorkItem.Items.Add(_tokenCache);

            IChannelFactoryService factoryService = ServiceActivator.CreateInstance<IChannelFactoryService>(_userSessionService, _tokenCache);
            RootWorkItem.Services.Add<IChannelFactoryService>(factoryService);
                        
            IAuthorizationService authorizationService = ServiceActivator.CreateInstance<IAuthorizationService>();
            RootWorkItem.Services.Add<IAuthorizationService>(authorizationService);

            IFavoritesService favoritesService = RootWorkItem.Services.AddNew<FavoritesService, IFavoritesService>();
            RootWorkItem.Services.AddNew<ShellModuleService, IShellModuleService>();
            RootWorkItem.Services.AddNew<FileService, IFileService>();

            _settingsService = ServiceActivator.CreateInstance<IUXSettingsService>(ContainerName);
            RootWorkItem.Services.Add<IUXSettingsService>(_settingsService);

            RootWorkItem.Services.AddNew<HyperlinkService, IHyperlinkService>();

            ThreadPool.QueueUserWorkItem((e) => 
            {
                try
                {
                    _settingsService.LoadSettings();
                    favoritesService.LoadFavorites();
                }
                catch (Exception)
                { 
                }

                _settingsLoadedEvent.Set();
            });
        }
                
        protected override void AddBuilderStrategies(Builder builder)
        {
            base.AddBuilderStrategies(builder);
            
            builder.Strategies.AddNew<ShellModuleStrategy>(BuilderStage.Creation);
            builder.Strategies.AddNew<ActionActivationStrategy>(BuilderStage.Initialization);
        }
                
        protected override void AfterShellCreated()
        {
            base.AfterShellCreated();

            _settingsLoadedEvent.WaitOne();
                        
            Window mainWindow = Application.Current.MainWindow;
            mainWindow.Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.IetfLanguageTag);
            mainWindow.Loaded += MainWindowLoaded;
                        
            ControlledWorkItem<ShellController> workItem = RootWorkItem.WorkItems.AddNew<ControlledWorkItem<ShellController>>();
            workItem.Controller.Run();
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _settingsService.ApplySettings();
            }
            catch
            { 
            }

            _loadedEvent.Set();

            MainWindow mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.Loaded -= MainWindowLoaded;

                IHyperlinkService hyperlinkService = RootWorkItem.Services.Get<IHyperlinkService>();
                hyperlinkService.Start();
                hyperlinkService.ExecuteHyperlink(_userSessionService.ActivationUri);
            }
        }
    }
}
