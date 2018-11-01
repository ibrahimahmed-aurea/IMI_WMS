using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Threading;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using Imi.SupplyChain.UX.Shell.Configuration;
using System.Windows.Threading;
using System.Globalization;

namespace Imi.SupplyChain.UX.Shell.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginView : UserControl, ILoginView
    {
        private LoginPresenter _presenter;

        public LoginPresenter Presenter
        {
            get { return _presenter; }
            set
            {
                _presenter = value;
                _presenter.View = this;
            }
        }

        public LoginView()
        {
            InitializeComponent();
            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(LoginView_IsVisibleChanged);
            this.KeyDown += new KeyEventHandler(KeyDownEventHandler);
            handleCapsLockWarning();
        }

        public void SetLanguages(LanguageCollection languages, CultureInfo defaultCulture)
        {
            LanguageCombo.ItemsSource = languages;

            if (defaultCulture != null)
                LanguageCombo.SelectedValue = defaultCulture.Name;

            if (LanguageCombo.SelectedItem == null && LanguageCombo.Items.Count > 0)
                LanguageCombo.SelectedItem = LanguageCombo.Items[0];

            LanguageComboSelectionChanged(this, null);
        }

        private void handleCapsLockWarning()
        {
            if (Console.CapsLock)
            {
                CapsLockWarning.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                CapsLockWarning.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void LoginView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                Application.Current.MainWindow.Activate();

                if (string.IsNullOrEmpty(UserFld.Text))
                {
                    UserFld.Text = _presenter.LastLogonUserId;

                    if (string.IsNullOrEmpty(UserFld.Text) || _presenter.Logout)
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                    new Action(() =>
                                    {
                                        UserFld.Focus();
                                        UserFld.SelectAll();
                                    }));
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                                    new Action(() =>
                                    {
                                        PasswordFld.SetInputFocus();
                                    }));
                    }
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            OnBuiltUp(null); // in lieu of cab
        }

        private void KeyDownEventHandler(object sender, KeyEventArgs e)
        {
            handleCapsLockWarning();
        }

        #region ISmartPartInfoProvider Members

        public ISmartPartInfo GetSmartPartInfo(Type smartPartInfoType)
        {
            SmartPartInfo info = new SmartPartInfo("Login", "");
            return info;
        }

        #endregion

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            _presenter.Login(UserFld.Text, PasswordFld.Password);
        }

        private void LanguageComboSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageCombo.SelectedValue != null)
                _presenter.SetCulture(LanguageCombo.SelectedValue.ToString());
        }

        #region IBuilderAware Members

        public void OnBuiltUp(string id)
        {
            Presenter.OnViewReady();
        }

        public void OnTearingDown()
        {

        }

        #endregion

    }
}
