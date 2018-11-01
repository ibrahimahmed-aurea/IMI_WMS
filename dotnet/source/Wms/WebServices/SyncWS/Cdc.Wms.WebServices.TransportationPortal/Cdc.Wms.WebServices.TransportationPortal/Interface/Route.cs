using System;
using System.Collections.Generic;
using System.Text;

namespace Cdc.Wms.WebServices.TransportationPortal.Entities
{
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/webservices/transportationportal")]
    public partial class Route
    {
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string VehicleIdentity;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = false)]
        public DateTime EstimatedTimeOfDeparture;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string RouteIdentity;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Name;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> TotalDrivingTimeInSeconds;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> TotalUnloadingTimeInSeconds;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> TotalTimeInSeconds;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<double> TotalDistance;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public RouteStop[] RouteStops;
    }
}
