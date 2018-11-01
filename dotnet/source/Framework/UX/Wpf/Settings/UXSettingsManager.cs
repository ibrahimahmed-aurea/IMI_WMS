using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.CompositeUI;
using Cdc.Framework.UX.Settings;

namespace Cdc.Framework.UX.Wpf.Settings
{
    public class UXSettingsManager : DependencyObject, ISettingsManager
    {
        private FrameworkElement target;
        private ISettingsProvider provider;

        public UXSettingsManager(FrameworkElement target, ISettingsProvider provider)
        {
            this.target = target;
            this.provider = provider;
        }

        public object SaveSettings()
        {
            return provider.SaveSettings(target);
        }

        public void LoadSettings(object state)
        {
            provider.LoadSettings(target, state);
        }

        public Type SettingsType
        {
            get
            {
                return provider.SettingsType;
            }
        }

        // Using a DependencyProperty as the backing store for StateManagerType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProviderTypeProperty =
            DependencyProperty.RegisterAttached("ProviderType", typeof(Type), typeof(UXSettingsManager), new UIPropertyMetadata(null));

        public static void SetProviderType(UIElement element, Type value)
        {
            element.SetValue(ProviderTypeProperty, value);
        }
        public static Type GetProviderType(UIElement element)
        {
            return (Type)element.GetValue(ProviderTypeProperty);
        }
    }
}
