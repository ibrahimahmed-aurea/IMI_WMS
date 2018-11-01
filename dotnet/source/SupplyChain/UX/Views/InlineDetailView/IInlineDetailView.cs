using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX.Views
{
    public interface IInlineDetailView
    {
        void AddAction(string actionName, string caption, bool moveNextLine);
        void ShowInDetailWorkspace(object view);
        void UpdateActions();
    }
}
