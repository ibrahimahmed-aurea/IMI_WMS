using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using Imi.SupplyChain.UX.Shell.Services;
using Imi.SupplyChain.Services.Settings.ServiceContracts;
using System.Windows.Threading;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.Framework.UX.Services;
using System.Globalization;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public class LoadingPresenter
    {
        public ILoadingView View { get; set; }

        public delegate void AsyncCheckCompledtedDelegate(bool autoLogin);

        public event EventHandler<EventArgs> ShowLogin;

        public event EventHandler<EventArgs> AutoLogin;

        public UserSessionService UserSessionService { get; set; }
                
        public LoadingPresenter()
        {
        }

        private void CheckLoginCallback(object state)
        {
            bool autoLogin = IsAutoLoginEnabled();
            System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new AsyncCheckCompledtedDelegate(LoginCheckCompleted), autoLogin);
        }

        private bool IsAutoLoginEnabled()
        {
            /*
            try
            {
                using (IChannelFactoryService channelFactoryService = ServiceActivator.CreateInstance<IChannelFactoryService>(UserSessionService))
                {
                    ShellSettingsService shellSettingService = new ShellSettingsService(UserSessionService);
                    shellSettingService.SettingsService = channelFactoryService.CreateChannel(typeof(ISettingsService)) as ISettingsService;

                    UserSessionService.UICulture = new CultureInfo(shellSettingService.LoginSettings.CultureName);

                    return shellSettingService.LoginSettings.AutoLogin;
                }
            }
            catch (Exception)
            {
            }
            */

            return false;
        }

        private void LoginCheckCompleted(bool autoLogin)
        {
            if (autoLogin)
            {
                OnAutoLogin();
            }
            else
            {
                OnShowLogin();
            }
        }

        public void OnViewReady()
        {
            ThreadPool.QueueUserWorkItem(CheckLoginCallback);
        }

        protected virtual void OnShowLogin()
        {
            if (ShowLogin != null)
                ShowLogin(this, new EventArgs());
        }

        protected virtual void OnAutoLogin()
        {
            if (AutoLogin != null)
                AutoLogin(this, new EventArgs());
        }
    }
}
