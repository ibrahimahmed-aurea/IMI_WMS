using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.SupplyChain.UX.Views
{
    public delegate void CancelSearchDelegate(); 

    public interface ISearchProgressView
    {
        CancelSearchDelegate CancelCallback { get; set; }
    }
}
