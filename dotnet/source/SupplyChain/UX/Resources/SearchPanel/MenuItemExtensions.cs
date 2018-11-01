using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Imi.SupplyChain.UX
{
    public class MenuItemExtensions : DependencyObject
    {
        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.RegisterAttached("GroupName",
                                         typeof(String),
                                         typeof(MenuItemExtensions),
                                         new PropertyMetadata(String.Empty, OnGroupNameChanged));

        public static void SetGroupName(MenuItem element, String value)
        {
            element.SetValue(GroupNameProperty, value);
        }

        public static String GetGroupName(MenuItem element)
        {
            return element.GetValue(GroupNameProperty).ToString();
        }

        private static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menuItem = d as MenuItem;
            
            menuItem.Checked += MenuItemChecked;
            menuItem.Unchecked += MenuItemUnChecked;
            
        }
        
        private static void MenuItemChecked(object sender, RoutedEventArgs e)
        {
            var menuItem = e.OriginalSource as MenuItem;

            ItemsControl menu = LogicalTreeHelper.GetParent(menuItem) as ItemsControl;

            if (menu != null)
            {
                foreach (MenuItem item in menu.Items)
                {
                    if (item != menuItem && GetGroupName(item) == GetGroupName(menuItem))
                    {
                        item.Unchecked -= MenuItemUnChecked;
                        item.IsChecked = false;
                        item.Unchecked += MenuItemUnChecked;
                    }
                }
            }
        }

        private static void MenuItemUnChecked(object sender, RoutedEventArgs e)
        {
            var menuItem = e.OriginalSource as MenuItem;
            menuItem.IsChecked = true;
        }
    }

}
