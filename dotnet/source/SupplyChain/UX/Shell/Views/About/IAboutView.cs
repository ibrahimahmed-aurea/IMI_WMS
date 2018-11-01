using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public interface IAboutView
    {
        void Close();
        void SetModules(IList<string> moduleIdentifiers);
        void SetWaitCursor();
        string Version { get; set; }
        void SetNormalCursor();
    }
}
