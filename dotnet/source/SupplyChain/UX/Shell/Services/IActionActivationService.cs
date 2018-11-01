using System.Windows;

namespace Imi.SupplyChain.UX.Shell.Services
{
	public interface IActionActivationService
	{
		void FrameworkElementAdded(FrameworkElement frameworkElement);
		void FrameworkElementRemoved(FrameworkElement frameworkElement);
	}
}