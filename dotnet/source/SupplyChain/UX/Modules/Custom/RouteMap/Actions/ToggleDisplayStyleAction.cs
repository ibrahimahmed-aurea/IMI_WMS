using System;
using System.Linq;
using Imi.Framework.UX.Services;
using Microsoft.Practices.CompositeUI;
using Imi.SupplyChain.UX.Views;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Microsoft.Practices.ObjectBuilder;

namespace Imi.SupplyChain.Transportation.UX.Views.RouteMap.Actions
{
    public class ToggleDisplayStyleAction : IBuilderAware
    {
        [ServiceDependency]
        public IUserSessionService UserSessionService { get; set; }
                
        public void OnToggleDisplayStyle(WorkItem context, object caller, object target)
        {
            MapBrowserController mapController = null;

            if (context.Items.FindByType<MapBrowserController>().Count > 0)
            {
                mapController = context.Items.FindByType<MapBrowserController>().First();
                mapController.ToggleDisplayStyle();
            }

            return;
        }

        [ServiceDependency]
        public IActionCatalogService ActionCatalog { get; set; }

        #region IBuilderAware Members

        public void OnBuiltUp(string id)
        {
            ActionCatalog.RegisterActionImplementation(ActionNames.ToggleDisplayStyle, OnToggleDisplayStyle);
        }

        public void OnTearingDown()
        {
            ActionCatalog.RemoveActionImplementation(ActionNames.ToggleDisplayStyle);
        }

        #endregion
    }
}
