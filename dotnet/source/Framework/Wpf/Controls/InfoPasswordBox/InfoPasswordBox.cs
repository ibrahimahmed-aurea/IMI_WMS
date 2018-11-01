using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Security;

namespace Imi.Framework.Wpf.Controls
{
    public partial class InfoPasswordBox : Control
    {
        private PasswordBox passwordBox;

        // HasText
        public bool HasText
        {
            get { return (bool)GetValue(HasTextProperty); }
        }

        public static readonly DependencyProperty HasTextProperty =
            DependencyProperty.Register("HasText", typeof(bool), typeof(InfoPasswordBox), new UIPropertyMetadata(null));

        // InfoText
        public string InfoText
        {
            get { return (string)GetValue(InfoTextProperty); }
            set { SetValue(InfoTextProperty, value); }
        }

        public static readonly DependencyProperty InfoTextProperty =
            DependencyProperty.Register("InfoText", typeof(string), typeof(InfoPasswordBox), new UIPropertyMetadata("Password"));
                
        public SecureString Password 
        {
            get
            {
                return passwordBox.SecurePassword;
            }
        }
                
        static InfoPasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InfoPasswordBox), new FrameworkPropertyMetadata(typeof(InfoPasswordBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Find neccessary components in template
            passwordBox = Template.FindName("PART_PasswordBox", this) as PasswordBox;
            passwordBox.PasswordChanged += new RoutedEventHandler(PasswordChanged);
        }

        void PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.SetValue(HasTextProperty, (passwordBox.Password != String.Empty));
        }
                
        protected override void OnPreviewGotKeyboardFocus(System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewGotKeyboardFocus(e);
            if (passwordBox != null)
            {
                passwordBox.SelectAll();
            }
        }

        public void SetInputFocus()
        {
            if (passwordBox != null)
            {
                passwordBox.Focus();
            }
        }
    }
}