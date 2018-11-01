using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX.Views
{
    public interface IFindDialogView
    {
        void ShowInSearchWorkspace(object view);
        void ShowInMasterWorkspace(object view);
        void UpdateWorkspaceLayout(); ////WORKAROUND TO AVOID GUI FROM HANGING WHEN GRID ARE WIDER THEN WORKSPACE
    }
}
