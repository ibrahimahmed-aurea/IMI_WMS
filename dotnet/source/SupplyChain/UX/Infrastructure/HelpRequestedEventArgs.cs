using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Imi.SupplyChain.UX.Infrastructure
{
    public class HelpRequestedEventArgs : CancelEventArgs
    {
        public string HelpBaseUri { get; set; }
    }
}
