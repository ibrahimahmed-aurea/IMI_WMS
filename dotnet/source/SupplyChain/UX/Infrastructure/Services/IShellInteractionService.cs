using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Collections;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.CompositeUI.Utility;
using System.ComponentModel;

namespace Imi.SupplyChain.UX.Infrastructure.Services
{
    public interface IShellInteractionService : IWorkspace
    {
        IShellModule ActiveModule { get; }
        void ShowProgress();
        void HideProgress();
        ICollection<ShellAction> Actions { get; }
        MessageBoxResult ShowMessageBox(string caption, string message, string details, MessageBoxButton button, MessageBoxImage image);
        void ShowMessageBox(Exception ex);
        MessageBoxResult ShowNotAuhtorizedMessageBox();
        string ContextInfo { get; set; }
        void ShowNotification(ShellNotification notification);
        event EventHandler ModuleActivated;
        event EventHandler ModuleDeactivated;
        event EventHandler<HelpRequestedEventArgs> HelpRequested;
        event EventHandler<CancelEventArgs> ShellTerminating;
        event EventHandler ShellTerminated;
        event EventHandler<MenuItemExecutedEventArgs> MenuItemExecuted;
        void AddToFavorites(ShellMenuItem item);
        event EventHandler<HyperlinkExecutedEventArgs> HyperlinkExecuted;
    }
}
