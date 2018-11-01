using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX.Shell.Services;
using Imi.SupplyChain.UX.Infrastructure.Services;
using System.ServiceModel.Security;
using Imi.Framework.UX.Identity;
using System.Diagnostics;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public class WelcomePresenter
    {
        public IWelcomeView View { get; set; }
                
        public event EventHandler<EventArgs> LoginSuccessful;
        public event EventHandler<EventArgs> LoginFailed;
        
        public UserSessionService UserSessionService { get; set; }
        public SecurityTokenCache TokenCache { get; set; }

        public void Login()
        {
            ThreadPool.QueueUserWorkItem(LoginThread);
        }

        private void LoginThread(object state)
        {
            IChannelFactoryService channelFactoryService = null;
            string error = null;
            Exception exception = null;

            try
            {
                channelFactoryService = ServiceActivator.CreateInstance<IChannelFactoryService>(UserSessionService, TokenCache);
                UserSessionService.ConfigFilename = ConfigHelper.LoadClientConfig(channelFactoryService);
            }
            catch (SecurityNegotiationException ex)
            {
                exception = ex;
                error = StringResources.Login_InvalidCredentials;
                TokenCache.Flush();
            }
            catch (MessageSecurityException ex)
            {
                exception = ex;
                error = StringResources.Login_InvalidCredentials;
                TokenCache.Flush();
            }
            catch (Exception ex)
            {
                exception = ex;
                error = ex.ToString();
                TokenCache.Flush();
            }
            finally
            {
                channelFactoryService.Dispose();
            }

            if (exception != null)
            {
                try
                {
                    EventLog.WriteEntry(StringResources.Title, string.Format("{0} - {1}", StringResources.Login_Login, exception.ToString()), EventLogEntryType.Warning);
                }
                catch
                { 
                }
            }

            System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action<string>(LoginCompleted), error);
        }

        private void LoginCompleted(string error)
        {
            if (string.IsNullOrEmpty(error))
            {
                OnLoginSuccessful();
            }
            else
            {
                View.ShowError(error);
            }
        }
                
        public virtual void OnLoginSuccessful()
        {
            if (LoginSuccessful != null)
                LoginSuccessful(this, new EventArgs());
        }

        public virtual void OnLoginFailed()
        {
            if (LoginFailed != null)
                LoginFailed(this, new EventArgs());
        }
    }
}
