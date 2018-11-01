using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public class DialogHelper
    {
        public static void InitializeUXComponentForAllViews(IList<Dialog> dialogs, Dictionary<Guid, IDomainObject> loadedObjects)
        {
            IViewHelper viewHelper = MetaManagerServices.Helpers.ViewHelper;
            foreach (Dialog dialog in dialogs)
            {
                foreach (ViewNode viewNode in dialog.ViewNodes)
                {
                    if (viewNode.View.VisualTree != null)
                    {
                        viewHelper.InitializeUXComponent(viewNode.View.VisualTree, loadedObjects);
                    }
                }

                if ((dialog.SearchPanelView != null) && (dialog.SearchPanelView.VisualTree != null))
                {
                    viewHelper.InitializeUXComponent(dialog.SearchPanelView.VisualTree, loadedObjects);
                }
            }
        }
    }
}
