using System;
using System.Collections.Generic;
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
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.SmartParts;
using ActiproSoftware.Windows.Themes;
using ActiproSoftware.Windows.Controls.Ribbon;
using ActiproSoftware.Windows;
using Imi.SupplyChain.UX.Shell.Settings;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Shell.Views
{
	/// <summary>
	/// Interaction logic for PopularSettingsxaml.xaml
	/// </summary>
    [SmartPart]
	public partial class PopularSettingsView : UserControl
	{
        private MainWindowSettingsRepository _mainWindowSettings;
        private DashboardSettingsRepository _dashboardSettings;

        [EventPublication(EventTopicNames.CloseAllWorkspaces, PublicationScope.Global)]
        public event EventHandler<EventArgs> CloseAllWorkspaces;

        [ServiceDependency]
        public WorkItem WorkItem { get; set; }

        public MainWindowSettingsRepository MainWindowSettings
        {
            get
            {
                return _mainWindowSettings;
            }
            set
            {
                _mainWindowSettings = value;
             
                isGlassEnabledCheckBox.IsChecked = _mainWindowSettings.ThemeSettings.IsGlassEnabled;

                ThemeName theme = ThemeName.Custom;

                if (!_mainWindowSettings.ThemeSettings.TintColor.HasValue)
                {
                    Enum.TryParse<ThemeName>(_mainWindowSettings.ThemeSettings.ThemeName, out theme);
                }

                themeNameComboBox.SelectedValue = theme;

                if (_mainWindowSettings.ThemeSettings.TintColor.HasValue)
                {
                    themeTintColorPicker.SelectedColor = _mainWindowSettings.ThemeSettings.TintColor.Value;
                }

                themeNameComboBox.SelectionChanged += ThemeNameComboBoxSelectionChanged;
                themeTintColorPicker.SelectedColorChanged += ThemeTintColorSelectionChanged;
                isGlassEnabledCheckBox.Click += IsGlassEnabledCheckBoxClick;

                ApplyTheme();
            }
        }


        public DashboardSettingsRepository DashboardSettings
        {
            get
            {
                return _dashboardSettings;
            }
            set
            {
                _dashboardSettings = value;
                dashboardRefreshEditBox.Value = (int)_dashboardSettings.RefreshInterval / 60;
            }
        }
                    
		public PopularSettingsView()
		{
			this.InitializeComponent();
            DataContext = this;
		}
                          
        private static RibbonWindow GetParentWindow(DependencyObject child)
        {
            DependencyObject parentObject = LogicalTreeHelper.GetParent(child);

            if (parentObject == null)
            {
                return null;
            }
                        
            if (parentObject is RibbonWindow)
            {
                return (RibbonWindow)parentObject;
            }
            else
            {
                return GetParentWindow(parentObject);
            }
        }

        private void UseDefaultThemeButtonClick(object sender, RoutedEventArgs e)
        {
            themeNameComboBox.SelectionChanged -= ThemeNameComboBoxSelectionChanged;
            themeTintColorPicker.SelectedColorChanged -= ThemeTintColorSelectionChanged;
            isGlassEnabledCheckBox.Click -= IsGlassEnabledCheckBoxClick;

            _mainWindowSettings.ThemeSettings = ThemeHelper.GetDefaultThemeSettings();
            ApplyTheme();

            themeNameComboBox.SelectionChanged += ThemeNameComboBoxSelectionChanged;
            themeTintColorPicker.SelectedColorChanged += ThemeTintColorSelectionChanged;
            isGlassEnabledCheckBox.Click += IsGlassEnabledCheckBoxClick;
        }

        private void IsGlassEnabledCheckBoxClick(object sender, RoutedEventArgs e)
        {
            _mainWindowSettings.ThemeSettings.IsGlassEnabled = isGlassEnabledCheckBox.IsChecked.GetValueOrDefault();
            ApplyTheme();
        }

        private void IsCommonWorkspaceEnabledCheckBoxClick(object sender, RoutedEventArgs e)
        {
            IMessageBoxView view = WorkItem.SmartParts.AddNew<MessageBoxView>();
            
            if (view.Show(StringResources.Settings_PopularCommonWorkspace, StringResources.Settings_CloseAllWindows, null, Infrastructure.MessageBoxButton.YesNo, Infrastructure.MessageBoxImage.Question) == Infrastructure.MessageBoxResult.Yes)
            {
                if (CloseAllWorkspaces != null)
                {
                    CloseAllWorkspaces(this, new EventArgs());
                }
            }
            else
            {
                isCommonWorkspaceEnabledCheckBox.IsChecked = !isCommonWorkspaceEnabledCheckBox.IsChecked;
            }
        }

        private void DashboardRefreshEditBoxValueChangedEventHandler(object sender, PropertyChangedRoutedEventArgs<int?> e)
        {
            _dashboardSettings.RefreshInterval = e.NewValue.Value * 60;
        }

        private void ThemeNameComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ThemeName theme = (ThemeName)e.AddedItems[0];
            _mainWindowSettings.ThemeSettings.TintColor = null;
            _mainWindowSettings.ThemeSettings.ThemeName = theme.ToString();
            ApplyTheme();
        }
                
        private void ThemeTintColorSelectionChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            _mainWindowSettings.ThemeSettings.TintColor = themeTintColorPicker.SelectedColor;

            if ((themeNameComboBox.SelectedItem == null) || (!themeNameComboBox.SelectedItem.Equals(ThemeName.Custom)))
            {
                themeNameComboBox.SelectionChanged -= ThemeNameComboBoxSelectionChanged;
                themeNameComboBox.SelectedItem = ThemeName.Custom;
                themeNameComboBox.SelectionChanged += ThemeNameComboBoxSelectionChanged;
            }
                        
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            GetParentWindow(this).IsGlassEnabled = isGlassEnabledCheckBox.IsChecked;
            ThemeHelper.ApplyTheme(_mainWindowSettings.ThemeSettings);
            ThemeManager.ApplyTheme(GetParentWindow(this));
        }
	}
}