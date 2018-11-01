using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.Framework.Wpf.Controls;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public interface INavigationBarView
    {
        void AddModel(IShellPresentationModel model);
        IShellModule SelectedModule { get; set; }
        void AddToFavorites(IShellModule module, DrillDownMenuItem drillDownMenuItem);
    }
}
