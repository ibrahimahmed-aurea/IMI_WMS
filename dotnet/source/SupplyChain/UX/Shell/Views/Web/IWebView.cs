using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public interface IWebView
    {
        void SetUrl(string title, string url);
        void Refresh();
    }
}
