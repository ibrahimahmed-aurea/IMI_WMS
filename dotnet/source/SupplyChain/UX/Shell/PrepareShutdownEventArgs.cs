using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Imi.SupplyChain.UX.Shell
{
    public class PrepareShutdownEventArgs : CancelEventArgs
    {
        public PrepareShutdownEventArgs(bool cancel, bool logout)
            : base(cancel)
        {
            Logout = logout;
        }

        public bool Logout { get; set; }
    }
}
