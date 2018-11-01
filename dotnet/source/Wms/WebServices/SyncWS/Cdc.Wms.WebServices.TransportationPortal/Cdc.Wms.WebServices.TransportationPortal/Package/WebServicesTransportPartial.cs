using System;
using System.Data;
using System.Collections;

using Imi.Wms.WebServices.SyncWS.Framework;

namespace Cdc.Wms.WebServices.TransportationPortal.Entities
{
    public partial class WebServicesTransport : IPackage
    {
        public internalFindDepartureRouteResult Finddepartureroute(ISyncWSParameter dataParam, string DepartureIdentity)
        {
            IDataReader reader = null;
            int totalRows;
            internalFindDepartureRouteResult res = new internalFindDepartureRouteResult();

            Finddepartureroute(
                DepartureIdentity,
                out reader);

            ArrayList list = ReaderHelper.Read(
                reader,
                new Route(),
                dataParam.GetSkipNoFirstRows(),
                dataParam.GetMaxRows(),
                out totalRows);

            res.list = list.ToArray(typeof(Route)) as Route[];
            res.SetTotalRows(totalRows);

            return res;
        }

        public RouteStop[] Finddepartureroutestops(ISyncWSParameter dataParam, string DepartureIdentity)
        {
            IDataReader reader = null;
            int totalRows;

            Finddepartureroutestops(
                DepartureIdentity,
                out reader);

            ArrayList list = ReaderHelper.Read(
                reader,
                new RouteStop(),
                dataParam.GetSkipNoFirstRows(),
                dataParam.GetMaxRows(),
                out totalRows);

            return list.ToArray(typeof(RouteStop)) as RouteStop[];
        }

    }
}