using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Linq;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using Imi.SupplyChain.UX.Shell.Services;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.SupplyChain.UX.Shell.Views;
using Imi.Framework.UX.Services;

namespace Imi.SupplyChain.UX.Shell.BuilderStrategies
{
	/// <summary>
	/// Implements a <see cref="BuilderStrategy"/> for <see cref="FrameworkElement"/>s.
	/// </summary>
	public class ShellModuleStrategy : BuilderStrategy
	{
		public override object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
            IShellModule module = existing as IShellModule;

            if (module != null)
			{
                WorkItem workItem = context.Locator.Get<WorkItem>(new DependencyResolutionLocatorKey(typeof(WorkItem), null));
                IShellInteractionService shellInteractionService = new ShellInteractionService(workItem, module, workItem.Services.Get<IShellModuleService>());
                workItem.Services.Add<IShellInteractionService>(shellInteractionService);
                workItem.Services.AddNew<ActionCatalogService, IActionCatalogService>();
                workItem.Services.AddNew<ActionActivationService, IActionActivationService>();
			}

			return base.BuildUp(context, typeToBuild, existing, idToBuild);
		}
	}
}
