using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using Imi.Framework.Job.Data;
using Imi.Wms.WebServices.SyncWS.Framework;

using Cdc.Wms.WebServices.TransportationPortal.Entities;

namespace Cdc.Wms.WebServices.TransportationPortal
{
    public class TransportationPortal
    {
        public ISyncWSResult FindDepartureRoute(Database database, ISyncWSParameter dataParameter)
        {
            internalFindDepartureRouteSearchParameters internalFindDepartureRouteSearchParameters = (internalFindDepartureRouteSearchParameters)dataParameter;

            WebServicesTransport pkg = new WebServicesTransport(database);

            internalFindDepartureRouteResult res = pkg.Finddepartureroute(dataParameter, internalFindDepartureRouteSearchParameters.DepartureIdentity);

            foreach (Route item in res.list)
            {
                // use same parameter structure
                internalFindDepartureRouteSearchParameters subParams = new internalFindDepartureRouteSearchParameters();
                subParams.DepartureIdentity = internalFindDepartureRouteSearchParameters.DepartureIdentity;
                subParams.firstResult = null;
                subParams.maxResult = null;

                item.RouteStops = pkg.Finddepartureroutestops(subParams, subParams.DepartureIdentity);
            }

            return res;
        }

        public ISyncWSResult find(ISyncWSParameter dataParameter)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
