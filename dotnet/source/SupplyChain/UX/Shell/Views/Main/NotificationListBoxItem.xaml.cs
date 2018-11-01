using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ActiproSoftware.Compatibility;
using ActiproSoftware.Windows.Controls.Views;

namespace Imi.SupplyChain.UX.Shell.Views {
	
	/// <summary>
	/// Represents a product item for displaying in the variuos panels.
	/// </summary>
	public partial class NotificationListBoxItem : ListBoxItem {

        public static RoutedCommand DeleteItemCommand = new RoutedCommand("DeleteItemCommand", typeof(NotificationListBoxItem));

        public string ApplicationName
        {
            get { return (string)GetValue(ApplicationNameProperty); }
            set { SetValue(ApplicationNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ApplicationName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ApplicationNameProperty =
            DependencyProperty.Register("ApplicationName", typeof(string), typeof(NotificationListBoxItem), new UIPropertyMetadata(""));
        
		/// <summary>
		/// Initializes a new instance of the <see cref="ProductListBoxItem"/> class.
		/// </summary>
        public NotificationListBoxItem()
        {
			InitializeComponent();
		}
	}
}
