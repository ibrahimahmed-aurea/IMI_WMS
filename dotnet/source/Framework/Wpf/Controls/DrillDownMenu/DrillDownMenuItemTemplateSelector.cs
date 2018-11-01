using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Imi.Framework.Wpf.Controls
{
    public class DrillDownMenuItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FolderTemplate { get; set; }
        public DataTemplate MenuItemTemplate { get; set; }
        public DataTemplate BackButtonTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is DrillDownMenuItem)
            {
                DrillDownMenuItem menuItem = item as DrillDownMenuItem;

                if (menuItem.IsFolder)
                    return (FolderTemplate);
                else
                {
                    if (menuItem.IsBackItem)
                        return (BackButtonTemplate);
                    else
                    {
                        return (MenuItemTemplate);
                    }
                }
            }
            else
            {
                return MenuItemTemplate; // base.SelectTemplate(item, container);
            }
        }
    }
}