using System;
using System.Collections.Generic;
using System.Text;

namespace Cdc.Wms.WebServices.TransportationPortal.Entities
{
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/webservices/transportationportal")]
    public partial class FindDepartureRouteResult
    {
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Route DepartureRoute;
    }
}
