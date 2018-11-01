using Imi.SupplyChain.UX.Modules.OrderManagement.Services;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Views
{
    public class WebViewPresenter : AsyncDataPresenter<IOMSSessionContext, IWebView>
    {
        [InjectionConstructor]
        public WebViewPresenter([ServiceDependency] IOMSSessionContext OmsSessionContext)
            : base(OmsSessionContext)
        {
        }

        public override void OnViewReady()
        {
        }

        public override void OnViewShow()
        {
        }

 
        protected override object ExecuteSearchAsync(object parameters)
        {
            return null;
        }

        protected override void PresentData(object data)
        {
        }

        public void Close()
        {
            WorkItem.SmartParts.Remove(this.View);
        }

        protected override void OnProgressUpdated(object state, int progressPercentage)
        {
       }
    }
}
