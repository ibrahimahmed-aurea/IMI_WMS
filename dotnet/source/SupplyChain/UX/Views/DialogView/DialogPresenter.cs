using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.Utility;
using Imi.Framework.UX;
using Microsoft.Practices.CompositeUI;

namespace Imi.SupplyChain.UX.Views
{
    public class DialogPresenter : Presenter<IDialogView>
    {
        public DialogType GetDialogType(object smartPart)
        {
            DialogType dialogType = DialogType.Overview;

            WorkItem current = FindContainingWorkItem(WorkItem, smartPart);

            if (current.Items.FindByType<DialogType>().Count() > 0)
                dialogType = current.Items.FindByType<DialogType>().Last();
            
            return dialogType;
        }

        private WorkItem FindContainingWorkItem(WorkItem workItem, object smartPart)
        {
            if (workItem.Items.ContainsObject(smartPart))
                return workItem;
            else
            {
                foreach (KeyValuePair<string, WorkItem> namedWorkItem in workItem.WorkItems)
                {
                    WorkItem wi = null;

                    wi = FindContainingWorkItem(namedWorkItem.Value, smartPart);

                    if (wi != null)
                        return wi;
                }

                return null;
            }
        }
    }
}
