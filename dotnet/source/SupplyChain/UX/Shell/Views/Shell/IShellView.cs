using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Collections;
using Microsoft.Practices.CompositeUI.SmartParts;
using Imi.SupplyChain.UX.Infrastructure;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public interface IShellView
    {
        IShellPresentationModel Model { get; set; }
        void CloseActiveSmartPart();
        void CloseActiveWorkspace();
        void CloseAllWorkspaces();
        void ShowProgress();
        void HideProgress();
        IWorkspace GetWorkspace(IShellModule module);
        void ShowNotification(string applicationName, ShellNotification notification);
    }
}
