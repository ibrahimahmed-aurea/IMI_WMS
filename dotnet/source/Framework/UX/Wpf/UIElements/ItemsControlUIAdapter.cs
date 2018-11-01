using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.CompositeUI.Utility;
using Microsoft.Practices.CompositeUI.UIElements;

namespace Imi.Framework.UX.Wpf.UIElements
{
    public class ItemsControlUIAdapter : UIElementAdapter<UIElement>
    {
        ItemsControl itemsControl;

        public ItemsControlUIAdapter(ItemsControl itemsControl)
        {
            Guard.ArgumentNotNull(itemsControl, "itemsControl");

            this.itemsControl = itemsControl;
        }

        protected override UIElement Add(UIElement uiElement)
        {
            Guard.ArgumentNotNull(uiElement, "uiElement");

            itemsControl.Items.Add(uiElement);

            return uiElement;
        }

        protected override void Remove(UIElement uiElement)
        {
            Guard.ArgumentNotNull(uiElement, "uiElement");

            itemsControl.Items.Remove(uiElement);
        }
    }
}
