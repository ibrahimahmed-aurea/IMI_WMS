using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace Imi.Framework.UX.Services
{
	public interface IWorkspaceLocatorService
	{
		IWorkspace FindContainingWorkspace(WorkItem workItem, object smartPart);
	}
}
