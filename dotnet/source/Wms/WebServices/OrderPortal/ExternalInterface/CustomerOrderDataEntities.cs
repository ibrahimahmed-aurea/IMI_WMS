using System;
using System.Data;
using System.Collections;
using Imi.Framework.Job.Data;

namespace Imi.Wms.WebServices.OrderPortal.ExternalInterface
{

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/wms/webservices/")]
    public class CustomerOrderHeadInfoDoc
    {
        public CustomerOrder[] aCustomerOrderList;
        public CustomerWorkOrder[] aCustomerWorkOrderList;
        public Departure[] aDepartureList;
        public OrderQuantities[] aOrderQuantitiesList;
    }

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/wms/webservices/")]
    public class CustomerOrderInfoDoc
    {
        public CustomerOrder[] aCustomerOrderList;
        public CustomerWorkOrder[] aCustomerWorkOrderList;
        public Departure[] aDepartureList;
        public OrderQuantities[] aOrderQuantitiesList;
        public CustomerOrderRow[] aCustomerOrderRowList;
    }

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/wms/webservices/")]
    public class CustomerOrderLineRangeDoc
    {
        public CustomerOrder[] aCustomerOrderList;
        public CustomerWorkOrder[] aCustomerWorkOrderList;
        public Departure[] aDepartureList;
        public OrderQuantities[] aOrderQuantitiesList;
        public CustomerOrderRow[] aCustomerOrderRowList;
    }

    /*
      select   CO.COMPANY_ID            ClientIdentity
              ,CO.COID                  OrderNumber
              ,CO.COSEQ                 OrderSequence
              ,CO.COSTATID              OrderStatusCode
              ,OLACODNL.OLACODTXT       OrderStatusText
     */
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/wms/webservices/orderportal")]
    public class CustomerOrder
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

        static public ArrayList Read(IDataReader reader)
        {
            ArrayList aCustomerOrderList = new ArrayList();

            while (reader.Read())
            {
                CustomerOrder aCustomerOrder = new CustomerOrder();
                aCustomerOrder.ClientIdentity = reader.GetString(0);
                aCustomerOrder.OrderNumber = reader.GetString(1);
                aCustomerOrder.OrderSequence = reader.GetInt32(2);
                aCustomerOrder.OrderStatusCode = reader.GetString(3);
                aCustomerOrder.OrderStatusText = reader.GetString(4);

                aCustomerOrderList.Add(aCustomerOrder);
            }

            reader.Close();

            return aCustomerOrderList;
        }
    }

    /*
      select   COWORK.COMPANY_ID        ClientIdentity
              ,COWORK.COID              OrderNumber
              ,COWORK.COSEQ             OrderSequence
              ,COWORK.COSUBSEQ          SubSequence
              ,COWORK.COSTATID          OrderStatusCode
              ,OLACODNL.OLACODTXT       OrderStatusText
     */
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/wms/webservices/orderportal")]
    public class CustomerWorkOrder
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

        static public ArrayList Read(IDataReader reader)
        {
            ArrayList aCustomerWorkOrderList = new ArrayList();

            while (reader.Read())
            {
                CustomerWorkOrder aCustomerWorkOrder = new CustomerWorkOrder();
                aCustomerWorkOrder.ClientIdentity = reader.GetString(0);
                aCustomerWorkOrder.OrderNumber = reader.GetString(1);
                aCustomerWorkOrder.OrderSequence = reader.GetInt32(2);
                aCustomerWorkOrder.SubSequence = reader.GetInt32(3);
                aCustomerWorkOrder.OrderStatusCode = reader.GetString(4);
                aCustomerWorkOrder.OrderStatusText = reader.GetString(5);

                aCustomerWorkOrderList.Add(aCustomerWorkOrder);
            }

            reader.Close();

            return aCustomerWorkOrderList;
        }
    }

    /*
      select   COWORK.COMPANY_ID             ClientIdentity
              ,COWORK.COID                   OrderNumber
              ,COWORK.COSEQ                  OrderSequence
              ,COWORK.COSUBSEQ               SubSequence
              ,DEP.DEPARTURE_ID              DepartureIdentity
              ,DEP.DEPARTURE_PICKSTAT        PickStatusCode
              ,OLACODNL_PICKSTAT.OLACODTXT   PickStatusText
              ,DEP.DEPARTURE_LOADSTAT        LoadStatusCode
              ,OLACODNL_LOADSTAT.OLACODTXT   LoadStatusText
              ,DEP.DEPARTURE_DTM             DepartureTime
              ,DEP.DLVRYMETH_ID              MethodOfShipmentId
              ,DEP.DLVRYMETH_NAME            MethodOfShipmentName
              ,DEP.FREID                     ForwarderId
              ,FRE.FRENAME                   ForwarderName
              ,DEP.ROUTID                    DispatchRoute
              ,DEP.TRUCKID                   VehicleId
     */
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/wms/webservices/orderportal")]
    public class Departure
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

        static public ArrayList Read(IDataReader reader)
        {
            ArrayList aDepartureList = new ArrayList();

            while (reader.Read())
            {
                Departure aDeparture = new Departure();

                aDeparture.ClientIdentity = reader.GetString(0);
                aDeparture.OrderNumber = reader.GetString(1);
                aDeparture.OrderSequence = reader.GetInt32(2);
                aDeparture.SubSequence = reader.GetInt32(3);
                aDeparture.DepartureIdentity = reader.GetString(4);
                aDeparture.PickStatusCode = reader.GetString(5);
                aDeparture.PickStatusText = reader.GetString(6);
                aDeparture.LoadStatusCode = reader.GetString(7);
                aDeparture.LoadStatusText = reader.GetString(8);
                aDeparture.DepartureTime = reader.GetDateTime(9);
                aDeparture.MethodOfShipmentId = reader.GetString(10);
                aDeparture.MethodOfShipmentName = reader.GetString(11);
                aDeparture.ForwarderId = reader.GetString(12);
                aDeparture.ForwarderName = reader.GetString(13);
                aDeparture.DispatchRoute = reader.GetString(14);
                aDeparture.VehicleId = reader.GetString(15);

                aDepartureList.Add(aDeparture);
            }

            reader.Close();

            return aDepartureList;
        }
    }

    /*
      select   COWORK.COMPANY_ID        ClientIdentity
              ,COWORK.COID              OrderNumber
              ,COWORK.COSEQ             OrderSequence
              ,COWORK.COSUBSEQ          SubSequence
              ,COWORK.NOROWS            TotalNoOfLines
              ,COWORK.NOPIKROWS         PickedNoOfLines
              ,COWORK.WEIGHT            EstimatedWeight
              ,COWORK.VOLUME            EstimatedVolume
              ,sum(COROW.ESTNOPBPALS)   PlannedNoOfPallets
              ,sum(COROW.ESTNOPBPAKS)   PlannedNoOfLowestUM
              ,sum(COROW.ESTVOLUME)     PlannedVolume
              ,sum(COROW.ESTWEIGHT)     PlannedWeight
              ,sum(COROW.PIKNOPBPALS)   PickedNoOfPallets
              ,sum(COROW.PIKNOPBPAKS)   PickedNoOfLowestUM
              ,sum(COROW.PIKVOLUME)     PickedVolume
              ,sum(COROW.PIKWEIGHT)     PickedWeight
     */
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/wms/webservices/orderportal")]
    public class OrderQuantities
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

        static public ArrayList Read(IDataReader reader)
        {
            ArrayList aOrderQuantitiesList = new ArrayList();

            while (reader.Read())
            {
                OrderQuantities aOrderQuantities = new OrderQuantities();

                aOrderQuantities.ClientIdentity = reader.GetString(0);
                aOrderQuantities.OrderNumber = reader.GetString(1);
                aOrderQuantities.OrderSequence = reader.GetInt32(2);
                aOrderQuantities.SubSequence = reader.GetInt32(3);
                
                aOrderQuantities.TotalNoOfLines = Convert.ToDouble(reader.GetDecimal(4));
                aOrderQuantities.PickedNoOfLines = Convert.ToDouble(reader.GetDecimal(5));
                aOrderQuantities.EstimatedWeight = Convert.ToDouble(reader.GetDecimal(6));
                aOrderQuantities.EstimatedVolume = Convert.ToDouble(reader.GetDecimal(7));
                aOrderQuantities.PlannedNoOfPallets = Convert.ToDouble(reader.GetDecimal(8));
                aOrderQuantities.PlannedNoOfLowestUM = Convert.ToDouble(reader.GetDecimal(9));
                aOrderQuantities.PlannedVolume = Convert.ToDouble(reader.GetDecimal(10));
                aOrderQuantities.PlannedWeight = Convert.ToDouble(reader.GetDecimal(11));
                aOrderQuantities.PickedNoOfPallets = Convert.ToDouble(reader.GetDecimal(12));
                aOrderQuantities.PickedNoOfLowestUM = Convert.ToDouble(reader.GetDecimal(13));
                aOrderQuantities.PickedVolume = Convert.ToDouble(reader.GetDecimal(14));
                aOrderQuantities.PickedWeight = Convert.ToDouble(reader.GetDecimal(15));

                aOrderQuantitiesList.Add(aOrderQuantities);
            }

            reader.Close();

            return aOrderQuantitiesList;
        }
    }

    /*
      select   COROW.COMPANY_ID                      ClientIdentity
              ,COROW.COID                            OrderNumber
              ,COROW.COSEQ                           OrderSequence
              ,COROW.COSUBSEQ                        SubSequence
              ,COROW.COPOS                           LinePosition
              ,COROW.COSUBPOS                        LineSequence
              ,COROW.COROWKITPOS                     LineKitPosition
              ,COROW.COROWSTAT                       LineStatus
              ,COROW.ORDQTY                          OrderedQuantity
              ,COROW.PICKQTY                         PickedQuantity
              ,COROW.RESTQTY                         DiscrepancyQuantity
              ,COROW.RESTCOD                         DiscrepancyCode
              ,OLACODNL.OLACODTXT                    DiscrepancyText
              ,COMPANY_PICKDISCCOD.MSPICKDISCCOD_ID  ExternalDiscrepancyCode
     */
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://im.se/wms/webservices/orderportal")]
    public class CustomerOrderRow
    {
        public string ClientIdentity;
        public string OrderNumber;
        public Nullable<int> OrderSequence;
        public Nullable<int> SubSequence;
        public Nullable<int> LinePosition;
        public Nullable<int> LineSequence;
        public Nullable<int> LineKitPosition;
        public string LineStatus;
        public Nullable<double> OrderedQuantity;
        public Nullable<double> PickedQuantity;
        public Nullable<double> DiscrepancyQuantity;
        public string DiscrepancyCode;
        public string DiscrepancyText;
        public string ExternalDiscrepancyCode;

        static public ArrayList Read(IDataReader reader)
        {
            ArrayList aCustomerOrderRowList = new ArrayList();

            while (reader.Read())
            {
                CustomerOrderRow aCustomerOrderRow = new CustomerOrderRow();
                int i = 0;
                aCustomerOrderRow.ClientIdentity = reader.GetString(i++);
                aCustomerOrderRow.OrderNumber = reader.GetString(i++);
                aCustomerOrderRow.OrderSequence = reader.GetInt32(i++);
                aCustomerOrderRow.SubSequence = reader.GetInt32(i++);
                aCustomerOrderRow.LinePosition = reader.GetInt32(i++);
                aCustomerOrderRow.LineSequence = reader.GetInt32(i++);
                aCustomerOrderRow.LineKitPosition = reader.GetInt32(i++);
                aCustomerOrderRow.LineStatus = reader.GetString(i++);
                aCustomerOrderRow.OrderedQuantity = Convert.ToDouble(reader.GetDecimal(i++));
                aCustomerOrderRow.PickedQuantity = Convert.ToDouble(reader.GetDecimal(i++));
                aCustomerOrderRow.DiscrepancyQuantity = Convert.ToDouble(reader.GetDecimal(i++));
                aCustomerOrderRow.DiscrepancyCode = reader.GetString(i++);
                aCustomerOrderRow.DiscrepancyText = reader.GetString(i++);
                aCustomerOrderRow.ExternalDiscrepancyCode = reader.GetString(i++);

                aCustomerOrderRowList.Add(aCustomerOrderRow);
            }

            reader.Close();

            return aCustomerOrderRowList;
        }
    }
}
