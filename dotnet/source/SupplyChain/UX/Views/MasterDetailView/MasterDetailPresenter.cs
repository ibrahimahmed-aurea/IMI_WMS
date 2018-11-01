using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI.EventBroker;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using Microsoft.Practices.CompositeUI;

namespace Imi.SupplyChain.UX.Views
{
    public class MasterDetailPresenter : Presenter<IMasterDetailView>
    {
        [EventSubscription("ShowDetail")]
        public void OnShowDetail(object sender, DataEventArgs<bool?> args)
        {
            View.ShowDetail(args.Data);
        }

        [ServiceDependency]
        public IWorkspaceLocatorService WorkspaceLocatorService { get; set; }

        public void Close()
        {
            WorkspaceLocatorService.FindContainingWorkspace(WorkItem, View).Close(View);
        }
    }
}
