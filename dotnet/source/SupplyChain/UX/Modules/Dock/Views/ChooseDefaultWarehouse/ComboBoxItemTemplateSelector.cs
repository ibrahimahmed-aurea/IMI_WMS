using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Imi.SupplyChain.UX.Modules.Dock.Views
{
    public class ComboBoxItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ListTemplate { get; set; }
        public DataTemplate SelectedTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if ((item == null) ||
                (!(container is FrameworkElement)) ||
                ((container as FrameworkElement).TemplatedParent == null) ||
                (!((container as FrameworkElement).TemplatedParent is ComboBox))
               )
            return ListTemplate;

            ComboBox cb = (container as FrameworkElement).TemplatedParent as ComboBox;

            if (item == cb.SelectionBoxItem)
            {
                return SelectedTemplate;
            }
            else
            {
                return ListTemplate;
            }

        }
    }

}
