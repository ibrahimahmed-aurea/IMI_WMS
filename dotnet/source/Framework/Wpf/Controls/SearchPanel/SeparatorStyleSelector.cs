using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Imi.Framework.Wpf.Controls
{
    
    public class SeparatorStyleSelector : StyleSelector
    {
        private Style itemStyle;

        public Style ItemStyle
        {
            get { return itemStyle; }
            set { itemStyle = value; }
        }
        
        private Style separatorStyle;

        public Style SeparatorStyle
        {
            get { return separatorStyle; }
            set { separatorStyle = value; }
        }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            SearchPanelItem searchItem = item as SearchPanelItem;

            if (searchItem != null)
            {
                if (searchItem.IsSeparator)
                    return separatorStyle;
            }

            return itemStyle;
        }
    }
}
