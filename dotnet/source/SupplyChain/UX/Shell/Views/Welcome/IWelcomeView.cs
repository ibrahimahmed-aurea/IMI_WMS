using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public interface IWelcomeView
    {
        void ShowError(string error);
        void Login();
    }
}
