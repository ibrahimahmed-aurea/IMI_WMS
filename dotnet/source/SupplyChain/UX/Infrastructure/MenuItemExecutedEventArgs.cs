using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Imi.SupplyChain.UX.Infrastructure
{
    public class MenuItemExecutedEventArgs : EventArgs
    {
        public MenuItemExecutedEventArgs()
            : this(null)
        {
        }

        public MenuItemExecutedEventArgs(ShellMenuItem menuItem)
        {
            MenuItem = menuItem;
        }

        public ShellMenuItem MenuItem { get; set; }

        public bool OpenInNewWindow { get; set; }
    }
}
