using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.SupplyChain.UX.Infrastructure
{
    public interface IActionProvider
    {
        ICollection<ShellAction> GetActions();
    }
}
