using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Microsoft.Practices.CompositeUI.Utility;
using Microsoft.Practices.CompositeUI.UIElements;

namespace Imi.Framework.UX.Wpf.UIElements
{
    public class ItemsControlUIAdapterFactory : IUIElementAdapterFactory
    {
        #region IUIElementAdapterFactory Members

        public IUIElementAdapter GetAdapter(object uiElement)
        {
            return new ItemsControlUIAdapter(uiElement as ItemsControl);
        }

        public bool Supports(object uiElement)
        {
            if (uiElement is ItemsControl)
                return true;
            else
                return false;
        }

        #endregion
    }
}
