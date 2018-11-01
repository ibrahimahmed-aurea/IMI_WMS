using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Infrastructure;

namespace Imi.SupplyChain.UX.Shell
{
    public enum StartOption
    { 
        Default,
        NewWindow,
        Dashboard
    }

    public class StartMenuItemExecutedEventArgs : EventArgs
    {
        public IShellModule Module { get; set; }
        public ShellDrillDownMenuItem MenuItem { get; set; }
        public StartOption StartOption { get; set; }

        public StartMenuItemExecutedEventArgs(ShellDrillDownMenuItem menuItem, IShellModule module, StartOption startOption)
        {
            MenuItem = menuItem;
            Module = module;
            StartOption = startOption;
        }
    }
}
