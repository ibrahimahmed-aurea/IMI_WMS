using System;
using System.Collections.Generic;
using System.Text;
using Cdc.Wms.WebServices.TransportationPortal.Entities;

namespace Cdc.Wms.WebServices.TransportationPortal
{
    interface ITransportationPortal
    {
        FindDepartureRouteResult FindDepartureRoute(string ChannelId, string Language,string DepartureIdentity);
    }
}
