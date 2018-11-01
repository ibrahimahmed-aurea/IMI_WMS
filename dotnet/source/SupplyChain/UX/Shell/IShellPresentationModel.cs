using System;
using Imi.SupplyChain.UX.Infrastructure;
using System.Collections.ObjectModel;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace Imi.SupplyChain.UX.Shell
{
    public interface IShellPresentationModel
    {
        ObservableCollection<ShellDrillDownMenuItem> Actions { get; }
        string ContextInfo { get; set; }
        string InstanceName { get; set; }
        IShellModule Module { get; set; }
        ShellDrillDownMenuItem StartMenuTopItem { get; set; }
        ShellDrillDownMenuItem FavoritesMenuTopItem { get; set; }
        bool IsInitialized { get; set; }
    }
}
