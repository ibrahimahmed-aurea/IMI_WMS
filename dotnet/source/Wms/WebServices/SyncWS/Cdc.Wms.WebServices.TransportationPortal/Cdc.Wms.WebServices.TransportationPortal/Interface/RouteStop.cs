using System;
using System.Collections.Generic;
using System.Text;

namespace Cdc.Wms.WebServices.TransportationPortal.Entities
{
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/webservices/transportationportal")]
    public partial class RouteStop
    {
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string NodeIdentity;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> StopSequence;

        #region Stop Information

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<double> DistanceFromPreviousStop;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = false)]
        public DateTime EstimatedArrivalTime;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> TotalDrivingTimeInSeconds;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> TotalUnloadingTimeInSeconds;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> TotalTimeInSeconds;

        #endregion

        #region Address Information

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string CustomerIdentity;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Name1;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Name2;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Name3;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Name4;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Name5;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Address1;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Address2;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Address3;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Address4;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string City;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ZipCode;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Region;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string CountryCode;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Country;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Latitude;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Longitude;

        #endregion

        #region Contact information

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ContactPerson;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ContactPhone;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ContactEmail;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Instructions;

        #endregion
    }
}
