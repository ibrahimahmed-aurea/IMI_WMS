using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Imi.Framework.Wpf.Controls
{
    public class SearchPanelItemContainerStyleSelector : StyleSelector
    {
        private Style defaultStyle;

        public Style DefaultStyle
        {
            get { return defaultStyle; }
            set { defaultStyle = value; }
        }

        private Style isFixedStyle;

        public Style IsFixedStyle
        {
            get { return isFixedStyle; }
            set { isFixedStyle = value; }
        }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            SearchPanelItem searchItem = item as SearchPanelItem;

            if (searchItem != null)
            {
                if (searchItem.IsFixed)
                    return isFixedStyle;
            }

            return defaultStyle;

        }
    }
}
