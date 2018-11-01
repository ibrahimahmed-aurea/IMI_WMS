using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Imi.Framework.UX.Wpf.BuilderStrategies
{
    public class FrameworkElementStrategySettings : DependencyObject
    {
        public bool IgnoreStrategy
        {
            get { return (bool)GetValue(IsIgnoredProperty); }
            set { SetValue(IsIgnoredProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IgnoreStrategy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsIgnoredProperty =
            DependencyProperty.RegisterAttached("IsIgnored", typeof(bool), typeof(FrameworkElementStrategySettings), new UIPropertyMetadata(false));

        public static void SetIsIgnored(UIElement element, Boolean value)
        {
            element.SetValue(IsIgnoredProperty, value);
        }
        public static Boolean GetIsIgnored(UIElement element)
        {
            return (Boolean)element.GetValue(IsIgnoredProperty);
        }

        // Using a DependencyProperty as the backing store for StateManagerType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SettingsProviderTypeProperty =
            DependencyProperty.RegisterAttached("SettingsProviderType", typeof(Type), typeof(FrameworkElementStrategySettings), new UIPropertyMetadata(null));

        public static void SetSettingsProviderType(UIElement element, Type value)
        {
            element.SetValue(SettingsProviderTypeProperty, value);
        }
        public static Type GetSettingsProviderType(UIElement element)
        {
            return (Type)element.GetValue(SettingsProviderTypeProperty);
        }



        public static string GetApplicationIdentifier(DependencyObject obj)
        {
            return (string)obj.GetValue(ApplicationIdentifierProperty);
        }

        public static void SetApplicationIdentifier(DependencyObject obj, string value)
        {
            obj.SetValue(ApplicationIdentifierProperty, value);
        }

        // Using a DependencyProperty as the backing store for ApplicationIdentifierProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ApplicationIdentifierProperty =
            DependencyProperty.RegisterAttached("ApplicationIdentifier", typeof(string), typeof(FrameworkElementStrategySettings), new UIPropertyMetadata(null));



        public static Key GetCompleteScanKey(DependencyObject obj)
        {
            return (Key)obj.GetValue(CompleteScanKeyProperty);
        }

        public static void SetCompleteScanKey(DependencyObject obj, Key value)
        {
            obj.SetValue(CompleteScanKeyProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CompleteScanKeyProperty =
            DependencyProperty.RegisterAttached("CompleteScanKey", typeof(Key), typeof(FrameworkElementStrategySettings), new UIPropertyMetadata(null));




        public static bool GetEnableScanning(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableScanningProperty);
        }

        public static void SetEnableScanning(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableScanningProperty, value);
        }

        // Using a DependencyProperty as the backing store for EnableScanning.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableScanningProperty =
            DependencyProperty.RegisterAttached("EnableScanning", typeof(bool), typeof(FrameworkElementStrategySettings), new UIPropertyMetadata(null));

        


        public FrameworkElementStrategySettings()
        { 
        
        }
    }
}
