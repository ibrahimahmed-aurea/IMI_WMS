using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.CompositeUI;
using System.Diagnostics;
using Imi.Framework.Wpf.Controls;

namespace Imi.Framework.UX.Wpf.Activation
{
	public class FrameworkElementActivationService : IFrameworkElementActivationService
	{
		private WorkItem workItem;
        
		[ServiceDependency]
		public WorkItem WorkItem
		{
			set
			{
				workItem = value;
			}
		}
                
		public void FrameworkElementAdded(FrameworkElement frameworkElement)
		{
            frameworkElement.GotFocus += GotFocusEventHandler;
		}

		public void FrameworkElementRemoved(FrameworkElement frameworkElement)
		{
			frameworkElement.GotFocus -= GotFocusEventHandler;
		}

		private void GotFocusEventHandler(object sender, RoutedEventArgs e)
		{
			if ((e.OriginalSource is ListBoxItem) || (e.OriginalSource is MenuItem))
				return;
            
			if (workItem != null)
			{
                e.Handled = true;
				workItem.Activate();
			}
		}
	}
}
