using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX
{
    public class ShowImportViewArgs : EventArgs
    {
        public ShowImportViewArgs()
        {
        }

        public Views.IImportView ImportView { get; set; }
        public bool ShowOnly { get; set; }
    }

    public interface IImportEnabledView
    {
        event EventHandler<ShowImportViewArgs> ShowImportView;

        bool IsImportEnabled { get; set; }

        void RaiseShowImportView(ShowImportViewArgs args);
    }
}
