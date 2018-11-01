using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.Utility;
using Microsoft.Practices.CompositeUI;
using Imi.Framework.UX;
using System.Threading;
using System.Globalization;
using System.Configuration;
using System.Security;
using Imi.SupplyChain.UX.Shell.Configuration;
using Imi.SupplyChain.UX.Shell.Services;
using Imi.SupplyChain.UX.Shell.Properties;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public class LoginPresenter
    {
        private ShellConfigurationSection _config;
        public event EventHandler<EventArgs> AttemptLogin;
        
        public ILoginView View { get; set; }
        
        public UserSessionService UserSessionService { get; set; }

        public bool Logout { get; set; }

        public LoginPresenter()
        {
            _config = ConfigurationManager.GetSection(ShellConfigurationSection.SectionKey) as ShellConfigurationSection;
        }

        public void OnViewReady()
        {
            View.SetLanguages(_config.LanguageElementCollection, LastLogonUICulture);
        }

        public void Login(string user, SecureString password)
        {
            LastLogonUserId = user;
            LastLogonUICulture = UserSessionService.UICulture;

            UserSessionService.UserId = user;
            UserSessionService.Password = password;
            
            OnAttemptLogin();
        }

        public string LastLogonUserId
        {
            get
            {
                if (string.IsNullOrEmpty(LocalUserSettings.Default.LastLogonUserId))
                    return string.Format("{0}\\{1}", Environment.UserDomainName, Environment.UserName);
                else
                    return LocalUserSettings.Default.LastLogonUserId;
            }
            set
            { 
                LocalUserSettings.Default.LastLogonUserId = value;
                LocalUserSettings.Default.Save();
            }
        }

        public CultureInfo LastLogonUICulture
        {
            get
            {
                CultureInfo info = null;

                try
                {
                    if (!string.IsNullOrEmpty(LocalUserSettings.Default.LastLogonUICultureName))
                    {
                        info = new CultureInfo(LocalUserSettings.Default.LastLogonUICultureName);
                    }
                    else
                    {
                        info = UserSessionService.UICulture;
                    }
                }
                catch (ArgumentException)
                {
                }

                return info;
            }
            set
            {
                LocalUserSettings.Default.LastLogonUICultureName = value.ToString();
                LocalUserSettings.Default.Save();
            }
        }
                
        public virtual void OnAttemptLogin()
        {
            if (AttemptLogin != null)
                AttemptLogin(this, new EventArgs());
        }
                
        public void SetCulture(string cultureName)
        {
            UserSessionService.UICulture = new CultureInfo(cultureName);
        }
    }
}
