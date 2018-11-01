using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using Imi.SupplyChain.UX.Shell.Services;
using Imi.SupplyChain.UX.Infrastructure;

namespace Imi.SupplyChain.UX.Shell.BuilderStrategies
{
	/// <summary>
	/// Implements a <see cref="BuilderStrategy"/> for <see cref="FrameworkElement"/>s.
	/// </summary>
	public class ActionActivationStrategy : BuilderStrategy
	{
		public override object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			FrameworkElement frameworkElement = existing as FrameworkElement;

			if ((frameworkElement != null) && (frameworkElement is IActionProvider))
			{
				WorkItem workItem = context.Locator.Get<WorkItem>(new DependencyResolutionLocatorKey(typeof(WorkItem), null));
                
                IActionActivationService service = workItem.Services.Get<IActionActivationService>(false);
                
                if (service != null)
                    service.FrameworkElementAdded(frameworkElement);
			}

			return base.BuildUp(context, typeToBuild, existing, idToBuild);
		}

		public override object TearDown(IBuilderContext context, object item)
		{
			FrameworkElement frameworkElement = item as FrameworkElement;

			if ((frameworkElement != null) && (frameworkElement is IActionProvider))
			{
				WorkItem workItem = context.Locator.Get<WorkItem>(new DependencyResolutionLocatorKey(typeof(WorkItem), null));
                
                IActionActivationService service = workItem.Services.Get<IActionActivationService>(false);

                if (service != null)
                    service.FrameworkElementRemoved(frameworkElement);
			}

			return base.TearDown(context, item);
		}
	}
}
