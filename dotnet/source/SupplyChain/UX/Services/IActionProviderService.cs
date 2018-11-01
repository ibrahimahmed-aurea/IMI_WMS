using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Infrastructure;
using Microsoft.Practices.CompositeUI;

namespace Imi.SupplyChain.UX.Services
{
    public interface IActionProviderService
    {
        void RegisterAction(object owner, string name, string caption);
        void RegisterAction(object owner, string name, string caption, string operation);
        void RegisterDrillDownAction(object owner, string name, string caption);
        void RegisterDrillDownAction(object owner, string name, string caption, string operation);
        void UpdateActions(object owner);
        ICollection<ShellAction> GetActions(object owner);
        ShellAction GetDrillDownAction(object owner, string actionId);
        object ExecuteSpecialFunction(string action, string name, object[] args, WorkItem context);
    }
}
