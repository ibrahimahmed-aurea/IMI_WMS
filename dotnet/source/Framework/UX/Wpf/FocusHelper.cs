using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imi.Framework.UX.Wpf
{
    public static class FocusHelper
    {
        public static bool SetFocus(FrameworkElement element, IList<string> focusableElementNames)
        {
            element.Focus();
            return TryFocus(element, element, focusableElementNames);
        }

        public static bool SetFocus(FrameworkElement element, string elementName)
        {
            UIElement child = element.FindName(elementName) as UIElement;

            if (child != null)
                return element.Focus();

            return false;
        }
        
        private static bool TryFocus(FrameworkElement root, DependencyObject o, IList<string> focusableElementNames)
        {
            bool focused = false;

            FrameworkElement e = o as FrameworkElement;

            if (e != null && e != root && focusableElementNames.Contains(e.Name))
                focused = e.Focus();

            if (!focused)
            {
                foreach (object child in LogicalTreeHelper.GetChildren(o))
                {
                    if (child is DependencyObject)
                    {
                        focused = TryFocus(root, child as DependencyObject, focusableElementNames);

                        if (focused)
                            break;
                    }
                }
            }

            return focused;
        }

    }
}
