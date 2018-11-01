using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI.EventBroker;
using Imi.Framework.UX.Services;
using Imi.Framework.UX;
using Imi.SupplyChain.UX.Services;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX
{
	public abstract class DataPresenter<TView> : Presenter<TView>
        where TView : class
	{
        [EventPublication("ShowDetail", PublicationScope.WorkItem)]
        public event EventHandler<DataEventArgs<bool?>> ShowDetailEvent;
        
        [ServiceDependency]
        public IShellInteractionService ShellInteractionService
        {
            get;
            set;
        }

        [ServiceDependency(Required = false)]
        public IActionProviderService ActionProviderService
        {
            get;
            set;
        }

        public virtual DialogType DialogType
        {
            get
            {
                DialogType dialogType = DialogType.Overview;

                if (WorkItem.Items.FindByType<DialogType>().Count() > 0)
                    dialogType = WorkItem.Items.FindByType<DialogType>().Last();

                return dialogType;
            }
        }

        public override void CloseView()
        {
            ShellInteractionService.Close(this.View);
        }

        public virtual ICollection<ShellAction> GetActions()
        {
            if (ActionProviderService != null)
            {
                return ActionProviderService.GetActions(this.View);
            }
            else
                return new List<ShellAction>();
        }

        public virtual object ExecuteActionSpecialFunction(string action, string name, object[] args, WorkItem context)
        {
            if (ActionProviderService != null)
            {
                return ActionProviderService.ExecuteSpecialFunction(action, name, args, context);
            }

            return null;
        }

        public ShellAction GetDrillDownAction(string actionId)
        {
            if (ActionProviderService != null)
            {
                return ActionProviderService.GetDrillDownAction(this.View, actionId);
            }
            else
            {
                return null;
            }
        }


        public void ShowDetail(bool? show)
        {
            if (ShowDetailEvent != null)
                ShowDetailEvent(this, new DataEventArgs<bool?>(show));
        }

    }
}

