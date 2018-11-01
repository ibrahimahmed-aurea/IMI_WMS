using System.Windows;

namespace Imi.Framework.UX.Wpf.Activation
{
	/// <summary>
	/// An activation service for <see cref="FrameworkElement"/>s.
	/// </summary>
	public interface IFrameworkElementActivationService
	{
		void FrameworkElementAdded(FrameworkElement frameworkElement);
        void FrameworkElementRemoved(FrameworkElement frameworkElement);
	}
}