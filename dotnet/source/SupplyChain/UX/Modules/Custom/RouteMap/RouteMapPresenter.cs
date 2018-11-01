using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.ObjectBuilder;
using Imi.Framework.Wpf.Controls;
using Imi.Framework.Services;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Services;
using Imi.SupplyChain.Transportation.UX.Views.Route;
using Imi.SupplyChain.Transportation.UX.Views.Route.Constants;
using Imi.SupplyChain.Transportation.UX.Contracts.Route.ServiceContracts;
using Imi.SupplyChain.Transportation.UX.Contracts.Route.DataContracts;
using Imi.SupplyChain.Transportation.UX.Views.RouteMap.Actions;
using Imi.SupplyChain.UX.Infrastructure;

namespace Imi.SupplyChain.Transportation.UX.Views.RouteMap
{
    public class RouteMapPresenter : AsyncDataPresenter<IRouteService, IRouteMapView> 
    {
        [EventPublication(EventTopicNames.RouteMapViewUpdatedTopic, PublicationScope.WorkItem)]
        public event EventHandler<DataEventArgs<RouteMapViewResult>> ViewUpdated;
		
		private RouteMapViewParameters viewParameters;
		private bool forceUpdate;
        private RouteMapViewToRouteServiceTranslator viewServiceTranslator;

        [InjectionConstructor]
        public RouteMapPresenter([WcfServiceDependency] IRouteService routeService)
            : base(routeService)
        {
        }
		
        public override void OnViewReady()
        {
            viewServiceTranslator = WorkItem.Items.AddNew<RouteMapViewToRouteServiceTranslator>() as RouteMapViewToRouteServiceTranslator;
            
            if (WorkItem.Items.FindByType<RouteMapViewParameters>().Count > 0)
			{
                viewParameters = WorkItem.Items.FindByType<RouteMapViewParameters>().Last();
				forceUpdate = true;
			}
        }
		
		public override void OnViewShow()
        {
            if ((View.RefreshDataOnShow) || (forceUpdate))
            {
				RefreshView();
            }
            else
            {
                ActionProviderService.UpdateActions(this.View);
            }
        }
		
		public void RefreshView()
		{
			UpdateView(viewParameters);
		}
        		
		public void UpdateView(RouteMapViewParameters viewParameters)
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
            object data = null;
            
            if ((parameters != null) && (parameters is RouteMapViewParameters))
            {
                viewParameters = parameters as RouteMapViewParameters;

                FindRouteNodesRequest serviceRequest = new FindRouteNodesRequest();

                serviceRequest.FindRouteNodesParameters = viewServiceTranslator.TranslateFromViewToService(viewParameters);

                FindRouteNodesResponse serviceResponse = Service.FindRouteNodes(serviceRequest);

                if ((serviceResponse != null) && (serviceResponse.FindRouteNodesResultCollection != null))
                {
                    IEnumerable<RouteMapViewResult> viewResultCollection = viewServiceTranslator.TranslateFromServiceToView(serviceResponse.FindRouteNodesResultCollection);
                    data = viewResultCollection;
                }
            }

            return (data);
        }

        protected override void PresentData(object data)
        {
            View.PresentData(data);
        }
        
        public virtual void OnViewUpdated(RouteMapViewResult viewResult)
        {
			foreach (RouteMapViewResult item in WorkItem.Items.FindByType<RouteMapViewResult>())
				WorkItem.Items.Remove(item);
				
			if (viewResult != null)
				WorkItem.Items.Add(viewResult);
			
			if (ViewUpdated != null)
                ViewUpdated(this, new DataEventArgs<RouteMapViewResult>(viewResult));
                
            ActionProviderService.UpdateActions(this.View); 
        }
        
        public void Search(object parameters)
        {
            RouteMapViewParameters viewParameters = parameters as RouteMapViewParameters;
            UpdateView(viewParameters);
        }
		
        public override void OnViewSet()
        {
            // new
            WorkItem.Items.AddNew<ToggleDisplayStyleAction>();
            ActionProviderService.RegisterAction(this.View, ActionNames.ToggleDisplayStyle, "Toggle Display Style");
        }

        public virtual void ShowImportView()
        {
            
        }

    }
}
