
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.SupplyChain.UX.Modules.OrderManagement.Configuration;
using System.Configuration;
using Microsoft.Win32;

namespace Imi.SupplyChain.UX.Modules.OrderManagement
{
    public static class OMSProgramUtils
    {

        internal static void SetHostPort(OMSConfigurationSection configSection, IUserSessionService UserSessionService)
        {
            string langCode = UserSessionService.UICulture.ThreeLetterISOLanguageName;
            string aomPort = null;
            string defaultPort = null;

            AomHostPortCollection languages = configSection.AomHostPortElementCollection;
            foreach (ConfigurationElement configElement in languages)
            {
                HostPortElement hostPortElement = configElement as HostPortElement;
                if (hostPortElement.Language.Equals(langCode))
                {
                    aomPort = hostPortElement.Number;
                    break;
                }
                else if (hostPortElement.Language.Equals("default"))
                {
                    defaultPort = hostPortElement.Number;
                }
            }

            if (aomPort == null)
                aomPort = defaultPort;

            try
            {
                configSection.HostPort = int.Parse(aomPort);
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show("Error in format of AOM hostPort configuration from backend server, contact system administrator (" + aomPort + ")");
                throw e;
            }
        }

        internal static void addIE10RegistryKey()
        {
            // ensure that webbrowser control is running in IE 10 mode
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

    }
}
