using System;
using System.Collections.Generic;
using System.Text;

using Imi.Wms.WebServices.SyncWS.Framework;
using Cdc.Wms.WebServices.TransportationPortal.Entities;

namespace Cdc.Wms.WebServices.TransportationPortal
{
    [System.Web.Services.WebService(Namespace = "http://im.se/webservices/transportationportal")]
    [System.Web.Services.WebServiceBinding(ConformsTo = System.Web.Services.WsiProfiles.BasicProfile1_1)]
    public class TransportationPortalWebService : System.Web.Services.WebService, ITransportationPortal
    {
        /*
        public warehouseSearchResult findByClient(warehouseSearchParameters warehouseSearchParameters)
        {
            Warehouse p = new Warehouse();
            return (warehouseSearchResult)PackageHelper.GetResult(warehouseSearchParameters, p.findByClient, p.find);
        }*/
        [System.Web.Services.WebMethod]
        public FindDepartureRouteResult FindDepartureRoute(string ChannelId, string Language, string DepartureIdentity)
        {
            TransportationPortal p = new TransportationPortal();

            internalFindDepartureRouteSearchParameters internalFindDepartureRouteSearchParameters = new internalFindDepartureRouteSearchParameters();
            internalFindDepartureRouteSearchParameters.DepartureIdentity = DepartureIdentity;

            internalFindDepartureRouteResult internalFindDepartureRouteResult = (internalFindDepartureRouteResult)PackageHelper.GetResult(internalFindDepartureRouteSearchParameters, p.FindDepartureRoute, p.find);
            if (internalFindDepartureRouteResult.list.GetLength(0) != 1)
                return null;
            else
            {
                FindDepartureRouteResult FindDepartureRouteResult = new FindDepartureRouteResult();
                FindDepartureRouteResult.DepartureRoute = internalFindDepartureRouteResult.list[0];
                return FindDepartureRouteResult;
            }
        }
    }
}
