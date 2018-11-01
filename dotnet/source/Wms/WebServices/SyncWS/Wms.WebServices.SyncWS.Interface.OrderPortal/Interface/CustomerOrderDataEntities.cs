using System;
using System.Data;
using System.Collections;

namespace Imi.Wms.WebServices.SyncWS.Interface.OrderPortal
{
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/wms/webservices/orderportal")]
    public partial class CustomerOrderHeadInfoDoc
    {
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public CustomerOrder[] aCustomerOrderList;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public CustomerWorkOrder[] aCustomerWorkOrderList;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Departure[] aDepartureList;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public OrderQuantities[] aOrderQuantitiesList;
    }

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/wms/webservices/orderportal")]
    public partial class CustomerOrderInfoDoc
    {
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public CustomerOrder[] aCustomerOrderList;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public CustomerWorkOrder[] aCustomerWorkOrderList;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Departure[] aDepartureList;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public OrderQuantities[] aOrderQuantitiesList;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public CustomerOrderRow[] aCustomerOrderRowList;
    }

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/wms/webservices/orderportal")]
    public partial class CustomerOrderLineRangeDoc
    {
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public CustomerOrder[] aCustomerOrderList;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public CustomerWorkOrder[] aCustomerWorkOrderList;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Departure[] aDepartureList;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public OrderQuantities[] aOrderQuantitiesList;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public CustomerOrderRow[] aCustomerOrderRowList;
    }

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/wms/webservices/orderportal")]
    public partial class CustomerOrder
    {
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ClientIdentity;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string OrderNumber;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> OrderSequence;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string OrderStatusCode;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string OrderStatusText;
    }

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/wms/webservices/orderportal")]
    public partial class CustomerWorkOrder
    {
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ClientIdentity;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string OrderNumber;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> OrderSequence;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> SubSequence;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string OrderStatusCode;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string OrderStatusText;
    }

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/wms/webservices/orderportal")]
    public partial class Departure
    {
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ClientIdentity;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string OrderNumber;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> OrderSequence;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> SubSequence;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string DepartureIdentity;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string PickStatusCode;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string PickStatusText;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string LoadStatusCode;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string LoadStatusText;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<DateTime> DepartureTime;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string MethodOfShipmentId;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string MethodOfShipmentName;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ForwarderId;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ForwarderName;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string DispatchRoute;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string VehicleId;
    }

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/wms/webservices/orderportal")]
    public partial class OrderQuantities
    {
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ClientIdentity;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string OrderNumber;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> OrderSequence;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> SubSequence;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<double> TotalNoOfLines;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<double> PickedNoOfLines;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<double> EstimatedWeight;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<double> EstimatedVolume;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<double> PlannedNoOfPallets;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<double> PlannedNoOfLowestUM;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<double> PlannedVolume;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<double> PlannedWeight;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<double> PickedNoOfPallets;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<double> PickedNoOfLowestUM;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<double> PickedVolume;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<double> PickedWeight;
    }

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/wms/webservices/orderportal")]
    public partial class CustomerOrderRow
    {
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ClientIdentity;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string OrderNumber;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> OrderSequence;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> SubSequence;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> LinePosition;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> LineSequence;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<int> LineKitPosition;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string LineStatus;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<double> OrderedQuantity;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<double> PickedQuantity;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Nullable<double> DiscrepancyQuantity;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string DiscrepancyCode;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string DiscrepancyText;
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ExternalDiscrepancyCode;
    }
}
