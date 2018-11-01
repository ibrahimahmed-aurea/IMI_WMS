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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ActiproSoftware.Windows.Themes;
using ActiproSoftware.Windows.Controls.Ribbon;
using Microsoft.Practices.CompositeUI.EventBroker;
using ActiproSoftware.Windows.Controls.Ribbon.Controls;
using Imi.SupplyChain.UX.Shell.Settings;
using Imi.Framework.Wpf.Controls;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.Framework.UX;
using System.ComponentModel;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public partial class MainWindow : RibbonWindow
    {
        private MainWindowState state = MainWindowState.Undefined;
        private ApplicationMenu _applicationMenu = null;
        private string _accessText = string.Empty;
        private bool _logout;
        
        public bool IsKeyTipModeActive
        {
            get { return (bool)GetValue(IsKeyTipModeActiveProperty); }
            set { SetValue(IsKeyTipModeActiveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsKeyTipModeActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsKeyTipModeActiveProperty =
            DependencyProperty.Register("IsKeyTipModeActive", typeof(bool), typeof(MainWindow), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, IsKeyTipModeActiveChanged));

        [EventPublication(EventTopicNames.PrepareShutdown, PublicationScope.Global)]
        public event EventHandler<EventArgs> PrepareShutdown;
                
        [EventPublication(EventTopicNames.ShowSettingsDialog, PublicationScope.Global)]
        public event EventHandler<EventArgs> ShowSettingsDialog;

        [EventPublication(EventTopicNames.Help, PublicationScope.Global)]
        public event EventHandler<EventArgs> Help;

        [EventPublication(EventTopicNames.Close, PublicationScope.Global)]
        public event EventHandler<EventArgs> CloseCurrentWindow;

        [EventPublication(EventTopicNames.CloseAll, PublicationScope.Global)]
        public event EventHandler<EventArgs> CloseAllWindows;
                        
        private static void IsKeyTipModeActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MainWindow)
            {
                MainWindow window = d as MainWindow;

                if (!window.IsKeyTipModeActive)
                {
                    window._accessText = string.Empty;
                }
            }
        }
                        
        public MainWindowState State
        {
            get
            {
                return state;
            }
            set
            {
                SwitchState(value);
            }
        }

        private void SwitchState(MainWindowState newState)
        {
            if (state != newState)
            {
                state = newState;

                switch (state)
                {
                    case MainWindowState.Minimal:
                        mainWorkspace.Margin = new Thickness(0);
                        this.mainWindow.DocumentName = StringResources.Title;
                        this.mainWindow.ApplicationName = string.Empty;
                        break;

                    case MainWindowState.Active:
                        ribbon.ApplicationMenu = _applicationMenu;
                        break;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            
            _applicationMenu = ribbon.ApplicationMenu as ApplicationMenu;
                                                            
            ribbon.Loaded += (s, e) =>
            {
                Rectangle rect = ribbon.Template.FindName("ribbonBackground", ribbon) as Rectangle;
                rect.Fill = null;
            };
        }

        static MainWindow()
        {
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UITabbedMdiContainerDocumentsButtonToolTip.ToString(), StringResources.WindowContainer_ActiveWindows);
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIStandardSwitcherDocumentsText.ToString(), StringResources.WindowContainer_ActiveWindows);
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UITabbedMdiContainerCloseButtonToolTip.ToString(), StringResources.WindowContainer_Close);
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIStandardSwitcherToolWindowsText.ToString(), "");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandMakeFloatingWindowText.ToString(), StringResources.WindowContainer_Float);
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandMakeDockedWindowText.ToString(), StringResources.WindowContainer_Dock);
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandCloseWindowText.ToString(), StringResources.WindowContainer_Close);
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandMoveToNewHorizontalContainerText.ToString(), StringResources.WindowContainer_HorizontalTabGroup);
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandMoveToNewVerticalContainerText.ToString(), StringResources.WindowContainer_VerticalTabGroup);
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandMoveToNextContainerText.ToString(), StringResources.WindowContainer_NextTabGroup);
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandMoveToPreviousContainerText.ToString(), StringResources.WindowContainer_PreviousTabGroup);

            ActiproSoftware.Products.Navigation.SR.SetCustomString(ActiproSoftware.Products.Navigation.SRName.UINavigationBarCustomizeButtonToolTip.ToString(), StringResources.NavigationPane_ConfigureButtons);
            ActiproSoftware.Products.Navigation.SR.SetCustomString(ActiproSoftware.Products.Navigation.SRName.UINavigationBarCustomizeMenuItemShowFewerButtonsText.ToString(), StringResources.NavigationPane_FewerButtons);
            ActiproSoftware.Products.Navigation.SR.SetCustomString(ActiproSoftware.Products.Navigation.SRName.UINavigationBarCustomizeMenuItemShowMoreButtonsText.ToString(), StringResources.NavigationPane_MoreButtons);
            ActiproSoftware.Products.Navigation.SR.SetCustomString(ActiproSoftware.Products.Navigation.SRName.UINavigationBarOptionsWindowDisplayButtonsLabelText.ToString(), StringResources.NavigationPane_ButtonOrder);
            ActiproSoftware.Products.Navigation.SR.SetCustomString(ActiproSoftware.Products.Navigation.SRName.UINavigationBarCustomizeMenuItemAddRemoveButtonsText.ToString(), StringResources.NavigationPane_AddRemoveButtons);
            ActiproSoftware.Products.Navigation.SR.SetCustomString(ActiproSoftware.Products.Navigation.SRName.UINavigationBarOptionsWindowTitle.ToString(), StringResources.NavigationPane_Options_Title);
            ActiproSoftware.Products.Navigation.SR.SetCustomString(ActiproSoftware.Products.Navigation.SRName.UINavigationBarCustomizeMenuItemOptionsText.ToString(), StringResources.NavigationPane_Options);
            ActiproSoftware.Products.Navigation.SR.SetCustomString(ActiproSoftware.Products.Navigation.SRName.UINavigationBarOptionsWindowMoveDownButtonText.ToString(), StringResources.NavigationPane_MoveDown);
            ActiproSoftware.Products.Navigation.SR.SetCustomString(ActiproSoftware.Products.Navigation.SRName.UINavigationBarOptionsWindowMoveUpButtonText.ToString(), StringResources.NavigationPane_MoveUp);
            ActiproSoftware.Products.Navigation.SR.SetCustomString(ActiproSoftware.Products.Navigation.SRName.UINavigationBarOptionsWindowOkButtonText.ToString(), StringResources.Settings_OK);
            ActiproSoftware.Products.Navigation.SR.SetCustomString(ActiproSoftware.Products.Navigation.SRName.UINavigationBarOptionsWindowCancelButtonText.ToString(), StringResources.Settings_Cancel);
            ActiproSoftware.Products.Navigation.SR.SetCustomString(ActiproSoftware.Products.Navigation.SRName.UINavigationBarOptionsWindowResetButtonText.ToString(), StringResources.NavigationPane_Reset);
            ActiproSoftware.Products.Navigation.SR.SetCustomString(ActiproSoftware.Products.Navigation.SRName.UINavigationBarToggleMinimizationButtonExpandToolTip.ToString(), StringResources.NavigationPane_Expand);
            ActiproSoftware.Products.Navigation.SR.SetCustomString(ActiproSoftware.Products.Navigation.SRName.UINavigationBarToggleMinimizedPopupButtonExpandToolTip.ToString(), StringResources.NavigationPane_ExpandMenu);
            ActiproSoftware.Products.Navigation.SR.SetCustomString(ActiproSoftware.Products.Navigation.SRName.UINavigationBarToggleMinimizationButtonMinimizeToolTip.ToString(), StringResources.NavigationPane_Minimize);

            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIApplicationButtonLabelText.ToString(), StringResources.AppMenu_Title);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (IsKeyTipModeActive  && !ribbon.IsApplicationMenuOpen)
            {
                string keyText = null;

                if (e.KeyboardDevice.Modifiers == ModifierKeys.Alt)
                {
                    keyText = TranslateKey(e.SystemKey);
                }
                else
                {
                    keyText = TranslateKey(e.Key);
                }


                if (IsAccessKey(keyText))
                {
                    _accessText += keyText;

                    if (KeyTipActivationService.IsPartialOrFullTip(_accessText))
                    {
                        if (KeyTipActivationService.IsFullTip(_accessText))
                        {
                            ribbon.Focus();
                            KeyTipActivationService.Activate(_accessText);
                            _accessText = string.Empty;
                        }

                        e.Handled = true;
                    }
                    else
                    {
                        _accessText = string.Empty;
                    }
                }
                else
                {
                    return;
                }
            }

            //Control Zoom with Ctrl + and Ctrl - key combination.
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                double zoomChange = 0;
                
                if (e.Key == Key.OemPlus || e.Key == Key.Add)
                {
                    zoomChange = 0.05;
                }
                else if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
                {
                    zoomChange = -0.05;
                }

                if (zoomChange != 0)
                {
                    statusBar.scaleSlider.Value += zoomChange;
                }
            }
            
            base.OnPreviewKeyDown(e);
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
            //Control Zoom with Ctrl and mouse wheel.
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                statusBar.scaleSlider.Value += (e.Delta > 0) ? 0.05 : -0.05;
            }
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            if (e.SystemKey == Key.LeftAlt)
            {
                _accessText = null;    
            }

            base.OnPreviewKeyUp(e);
        }
        
        private string TranslateKey(Key key)
        {
            if ((key >= Key.A) && (key <= Key.Z))
            {
                return key.ToString();
            }

            if ((key >= Key.D0) && (key <= Key.D9))
            {
                return key.ToString().Substring(1);
            }

            if ((key >= Key.NumPad0) && (key <= Key.NumPad9))
            {
                return key.ToString().Substring(key.ToString().Length - 1);
            }

            return string.Empty;
        }

        private const string allowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVXYZ0123456789";

        private bool IsAccessKey(string key)
        {
            return ((key.Length == 1) && (allowedCharacters.Contains(key)));
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            State = MainWindowState.Active;
                        
            BindingOperations.SetBinding(this, MainWindow.IsKeyTipModeActiveProperty, new Binding()
            {
                Source = ribbon,
                Path = new PropertyPath("IsKeyTipModeActive")
            });
            
            Image icon = Template.FindName("PART_ApplicationIcon", this) as Image;
            
            if (icon != null)
            {
                icon.Width = 0;
                icon.Height = 0;
            }
        }

        private void CloseCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExitButtonClick(object sender, ActiproSoftware.Windows.Controls.Ribbon.Controls.ExecuteRoutedEventArgs e)
        {
            Close();
        }

        private void HelpButtonClick(object sender, ActiproSoftware.Windows.Controls.Ribbon.Controls.ExecuteRoutedEventArgs e)
        {
            OnHelp();
        }

        private void SettingsButtonClick(object sender, ActiproSoftware.Windows.Controls.Ribbon.Controls.ExecuteRoutedEventArgs e)
        {
            OnShowSettingsDialog();
        }

        public virtual void OnPrepareShutdown(CancelEventArgs e)
        {
            if (PrepareShutdown != null)
                PrepareShutdown(this, e);
        }
                       
        public virtual void OnShowSettingsDialog()
        {
            if (ShowSettingsDialog != null)
                ShowSettingsDialog(this, new EventArgs());
        }

        public virtual void OnHelp()
        {
            if (Help != null)
                Help(this, new EventArgs());
        }

        public virtual void OnCloseCurrentWindow()
        {
            if (CloseCurrentWindow != null)
                CloseCurrentWindow(this, new EventArgs());
        }

        public virtual void OnCloseAllWindows()
        {
            if (CloseAllWindows != null)
                CloseAllWindows(this, new EventArgs());
        }
                
        protected override void OnClosing(CancelEventArgs e)
        {
            PrepareShutdownEventArgs args = new PrepareShutdownEventArgs(false, _logout);
            OnPrepareShutdown(args);
            e.Cancel = args.Cancel;
            base.OnClosing(e);
        }
                
        private void CloseButtonClick(object sender, ActiproSoftware.Windows.Controls.Ribbon.Controls.ExecuteRoutedEventArgs e)
        {
            OnCloseCurrentWindow();
        }
                
        private void CloseAllButtonClick(object sender, ActiproSoftware.Windows.Controls.Ribbon.Controls.ExecuteRoutedEventArgs e)
        {
            OnCloseAllWindows();
        }

        private void LogOutButtonClick(object sender, ActiproSoftware.Windows.Controls.Ribbon.Controls.ExecuteRoutedEventArgs e)
        {
            _logout = true;

            try
            {
                Close();
            }
            finally
            {
                _logout = false;
            }
        }

        public void LoadUserSettings(MainWindowSettingsRepository settings)
        {
            if (!settings.UseDefaultWindowSettings)
            {
                Width = settings.Width;
                Height = settings.Height;
                Left = settings.Left;
                Top = settings.Top;
                this.WindowState = settings.WindowState;
                statusBar.ZoomLevel = settings.ZoomLevel;
            }
                        
            ThemeHelper.ApplyTheme(settings.ThemeSettings);
        }

        public void SaveUserSettings(MainWindowSettingsRepository settings)
        {
            settings.Width = Width;
            settings.Height = Height;
            settings.Left = Left;
            settings.Top = Top;
            settings.WindowState = WindowState;
            settings.ZoomLevel = statusBar.ZoomLevel;
        }
    }
}
