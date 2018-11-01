// Generated from template: .\UX\View\PresenterTemplate.cst
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Services;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using Imi.Framework.Wpf.Controls;
using Imi.Framework.Services;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using ActiproSoftware.Windows.Controls.Ribbon;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Services;
using Imi.SupplyChain.UX.Views;
using Imi.SupplyChain.UX.Modules.Warehouse.Infrastructure.Services;
using Imi.SupplyChain.Warehouse.UX.Views.Location.Constants;
using Imi.SupplyChain.Warehouse.UX.Views.Product;
using Imi.SupplyChain.Warehouse.UX.Contracts.Location.ServiceContracts;
using Imi.SupplyChain.Warehouse.UX.Contracts.Location.DataContracts;
using Imi.SupplyChain.Warehouse.Services.Alarm.DataContracts;
using Imi.SupplyChain.Warehouse.Services.Alarm.ServiceContracts;
using Imi.SupplyChain.Warehouse.UX.Contracts.Product.DataContracts;
using Imi.SupplyChain.Warehouse.UX.Contracts.Product.ServiceContracts;

namespace Imi.SupplyChain.Warehouse.UX.Views.Location
{
	public class WPlaceNewPresenter : WPlaceNewPresenterBase                           
	{
        [InjectionConstructor]
        public WPlaceNewPresenter([WcfServiceDependency] ILocationService locationService)
            : base(locationService)
        {
        }

        protected override object ExecuteSearchAsync(object parameters)
        {
            object data = null;

            WPlaceNewViewParameters viewParameters = parameters as WPlaceNewViewParameters;

            if (viewParameters != null)
            {
                Imi.SupplyChain.Warehouse.UX.Contracts.Location.ServiceContracts.FindLocationRequest serviceRequest = new Imi.SupplyChain.Warehouse.UX.Contracts.Location.ServiceContracts.FindLocationRequest();

                serviceRequest.FindLocationParameters = viewServiceTranslator.TranslateFromViewToService(viewParameters);

                Imi.SupplyChain.Warehouse.UX.Contracts.Location.ServiceContracts.FindLocationResponse serviceResponse = Service.FindLocation(serviceRequest);

                if ((serviceResponse != null) && (serviceResponse.FindLocationResultCollection != null))
                    data = viewServiceTranslator.TranslateFromServiceToView(serviceResponse.FindLocationResultCollection);
            }
            else
            {
                foreach (IList<WPlaceNewViewResult> item in WorkItem.Items.FindByType<IList<WPlaceNewViewResult>>())
                {
                    data = item;

                    ((IList<WPlaceNewViewResult>)data)[0].Address = string.Empty;
                }
            }

            return data;
        }
    }
}
