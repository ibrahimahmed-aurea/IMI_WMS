using Imi.SupplyChain.UX.Modules.OrderManagement.Services;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Views
{
    public class UserInfoPresenter : AsyncDataPresenter<IOMSSessionContext, IUserInfoView>
    {
        private bool forceUpdate;
        private bool backendCallIsRunning = false;
        private string viewParameters;
                
        [InjectionConstructor]
        public UserInfoPresenter([ServiceDependency] IOMSSessionContext OMSUserSessionService)
            : base(OMSUserSessionService)
        {
        }

        public override void OnViewReady()
        {
            forceUpdate = true;
        }

        public override void OnViewShow()
        {
            if ((View.RefreshDataOnShow) || (forceUpdate))
            {
                RefreshView();
            }
        }

        public void RefreshView()
        {
            UpdateView(viewParameters);
        }

        public void UpdateView(string viewParameters)
        {
            this.viewParameters = viewParameters;

            if (View.IsVisible)
            {
                forceUpdate = false;
                ExecuteSearch(viewParameters);
            }
            else
            {
                forceUpdate = true;
            }
        }


        protected override object ExecuteSearchAsync(object parameters)
        {
            backendCallIsRunning = true;
            return null;
        }

        protected override void PresentData(object data)
        {
            backendCallIsRunning = false;

            View.PresentData(data);
        }

        public void Close()
        {
            // Abort running call to backend
            if (backendCallIsRunning)
            {
                backendCallIsRunning = false;
                this.Cancel();
            }

            this.View.Close();
            WorkItem.SmartParts.Remove(this.View);
        }

        protected override void OnProgressUpdated(object state, int progressPercentage)
        {
            //if (View.UpdateProgress != null)
            //    View.UpdateProgress(state, progressPercentage);
            //else
            //    base.OnProgressUpdated(state, progressPercentage);
        }
    }
}
