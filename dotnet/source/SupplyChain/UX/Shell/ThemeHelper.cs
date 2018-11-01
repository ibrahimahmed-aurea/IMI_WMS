using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActiproSoftware.Windows.Themes;
using System.Windows.Media;
using Imi.SupplyChain.UX.Shell.Settings;
using System.Windows;
using ActiproSoftware.Windows.Controls.Ribbon;
using System.Configuration;
using Imi.SupplyChain.UX.Shell.Configuration;

namespace Imi.SupplyChain.UX.Shell
{
    public class ThemeHelper
    {
        private static ThemeSettings _currentThemeSettings;

        private ThemeHelper()
        { 
        }

        static ThemeHelper()
        {
            ThemesOfficeThemeCatalogRegistrar.Register();
            ThemesMetroThemeCatalogRegistrar.Register();
            ThemeManager.AreNativeThemesEnabled = true;
        }

        public static ThemeSettings GetDefaultThemeSettings()
        {
            ShellConfigurationSection config = ConfigurationManager.GetSection(ShellConfigurationSection.SectionKey) as ShellConfigurationSection;
            ThemeSettings themeSettings = new ThemeSettings();

            themeSettings.ThemeName = config.ThemeName;
            themeSettings.IsGlassEnabled = config.IsGlassEnabled;

            if (!string.IsNullOrEmpty(config.ThemeTintColor))
            {
                themeSettings.TintColor = (Color)ColorConverter.ConvertFromString(config.ThemeTintColor);
            }
            
            return themeSettings;
        }
                
        public static void ApplyTheme(ThemeSettings themeSettings)
        {
            _currentThemeSettings = themeSettings;

            if (themeSettings.IsGlassEnabled)
            {
                ((RibbonWindow)Application.Current.MainWindow).IsGlassEnabled = themeSettings.IsGlassEnabled;
            }
            
            ThemeName theme = ThemeName.Generic;
            Enum.TryParse<ThemeName>(themeSettings.ThemeName, out theme);
            
            if (themeSettings.TintColor.HasValue)
            {
                TintedThemeCatalog catalog = new TintedThemeCatalog("Custom", theme.ToString());
                theme = ThemeName.Custom;
                                
                catalog.TintGroups.AddRange(TintGroupSets.Default, themeSettings.TintColor.Value);
                catalog.TintGroups.AddRange(TintGroupSets.ApplicationMenu, themeSettings.TintColor.Value);
                
                ThemeManager.RegisterThemeCatalog("Custom", catalog);

                if (ThemeManager.CurrentTheme == ThemeName.Custom.ToString())
                {
                    ThemeManager.ApplyTheme();
                    return;
                }
            }

            ThemeManager.CurrentTheme = theme.ToString();
        }
    }
}
