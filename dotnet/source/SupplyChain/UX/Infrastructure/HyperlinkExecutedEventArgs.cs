using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX.Infrastructure
{
    public class HyperlinkExecutedEventArgs : EventArgs
    {
        public ShellHyperlink Hyperlink { get; set; }

        public HyperlinkExecutedEventArgs()
            : this(null)
        {
        }

        public HyperlinkExecutedEventArgs(ShellHyperlink hyperlink)
        {
            Hyperlink = hyperlink;
        }
    }
}
