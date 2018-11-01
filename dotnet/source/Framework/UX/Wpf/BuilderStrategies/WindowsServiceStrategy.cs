using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using Imi.Framework.UX.Wpf.Activation;
using Imi.Framework.UX.Services;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace Imi.Framework.UX.Wpf.BuilderStrategies
{
	/// <summary>
	/// Implements a <see cref="BuilderStrategy"/> to ensure work items have the correct services added to them.
	/// </summary>
	public class WindowsServiceStrategy : BuilderStrategy
	{
		public override object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			WorkItem workItem = existing as WorkItem;

			if (workItem != null && !workItem.Services.ContainsLocal(typeof(IFrameworkElementActivationService)))
			{
				workItem.Services.Add<IFrameworkElementActivationService>(new FrameworkElementActivationService());
			}

            if (workItem != null && !workItem.Services.ContainsLocal(typeof(IWorkspaceLocatorService)))
            {
                workItem.Services.Add<IWorkspaceLocatorService>(new WorkspaceLocatorService());
            }

			return base.BuildUp(context, typeToBuild, existing, idToBuild);
		}
	}
}
