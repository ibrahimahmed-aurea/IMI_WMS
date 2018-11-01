using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Infrastructure;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public interface IMessageBoxView
    {
        MessageBoxResult Show(string caption, string message, string details, MessageBoxButton button, MessageBoxImage image);
    }
}
