using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Imi.Framework.Wpf.Controls
{
    public class ComboBoxTemplateSelector : DataTemplateSelector
    {
        private DataTemplate itemTemplate;

        public DataTemplate ItemTemplate
        {
            get { return itemTemplate; }
            set { itemTemplate = value; }
        }

        private DataTemplate removeTemplate;

        public DataTemplate RemoveTemplate
        {
            get { return removeTemplate; }
            set { removeTemplate = value; }
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            SearchPanelItem searchItem = item as SearchPanelItem;

            if (searchItem != null)
            {
                if (searchItem.IsRemoveItem)
                    return removeTemplate;
            }

            return itemTemplate;

        }
    }
}
