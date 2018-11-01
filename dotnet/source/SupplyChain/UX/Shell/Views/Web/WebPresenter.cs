using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.Utility;
using Imi.Framework.UX;
using Microsoft.Win32;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public class WebPresenter : Presenter<IWebView>
    {
        public WebPresenter()
        {
            const string REG_KEY = "SOFTWARE\\Microsoft\\Internet Explorer\\MAIN\\FeatureControl\\FEATURE_BROWSER_EMULATION";
#if DEBUG            
            const string SMART_CLIENT_NAME = "Imi.SupplyChain.UX.SmartClient.vshost.exe";
#else
            
            const string SMART_CLIENT_NAME = "Imi.SupplyChain.UX.SmartClient.exe";
#endif
            
            RegistryKey key = Registry.CurrentUser;
            key = key.OpenSubKey(REG_KEY) == null ? key.CreateSubKey(REG_KEY) : key.OpenSubKey(REG_KEY, true);

            if (key.GetValue(SMART_CLIENT_NAME) == null)
            {
                key.SetValue(SMART_CLIENT_NAME, "10001", RegistryValueKind.DWord);
            }
        }

        [EventSubscription(EventTopicNames.DashboardRefresh)]
        public void Refresh(object sender, EventArgs e)
        {
            View.Refresh();
        }
    }
}
