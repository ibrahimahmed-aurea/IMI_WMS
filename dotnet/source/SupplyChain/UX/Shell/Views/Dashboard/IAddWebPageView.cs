using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public interface IAddWebPageView
    {
        string Address { get; set; }
        string Title { get; set; }
        bool? ShowDialog();
    }
}
