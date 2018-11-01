using System;
using System.Data;
using System.Collections;
using Imi.Framework.Job.Data;

namespace Imi.Wms.WebServices.SyncWS.Interface.OrderPortal
{
    /*
      select   CO.COMPANY_ID            ClientIdentity
              ,CO.COID                  OrderNumber
              ,CO.COSEQ                 OrderSequence
              ,CO.COSTATID              OrderStatusCode
              ,OLACODNL.OLACODTXT       OrderStatusText
     */
    public partial class CustomerOrder
    {
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

        static public ArrayList MakeTestData()
        {
            ArrayList aCustomerOrderList = new ArrayList();

            for (int i = 0; i < 10; i++)
            {
                CustomerOrder aCustomerOrder = new CustomerOrder();
                aCustomerOrder.ClientIdentity = "IMISTD";
                aCustomerOrder.OrderNumber = "1234";
                aCustomerOrder.OrderSequence = i;
                aCustomerOrder.OrderStatusCode = "AB";
                aCustomerOrder.OrderStatusText = "ABCD";

                aCustomerOrderList.Add(aCustomerOrder);
            }

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
    public partial class CustomerWorkOrder
    {
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

        static public ArrayList MakeTestData()
        {
            ArrayList aCustomerWorkOrderList = new ArrayList();

            for (int i = 0; i < 10; i++)
            {
                CustomerWorkOrder aCustomerWorkOrder = new CustomerWorkOrder();
                aCustomerWorkOrder.ClientIdentity = "IMISTD";
                aCustomerWorkOrder.OrderNumber = "1234";
                aCustomerWorkOrder.OrderSequence = i;
                aCustomerWorkOrder.SubSequence = i;
                aCustomerWorkOrder.OrderStatusCode = "AB";
                aCustomerWorkOrder.OrderStatusText = "ABCD";

                aCustomerWorkOrderList.Add(aCustomerWorkOrder);
            }

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
    public partial class Departure
    {
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

        static public ArrayList MakeTestData()
        {
            ArrayList aDepartureList = new ArrayList();

            for (int i = 0; i < 10; i++)
            {
                Departure aDeparture = new Departure();

                aDeparture.ClientIdentity = "IMISTD";
                aDeparture.OrderNumber = "1234";
                aDeparture.OrderSequence = i;
                aDeparture.SubSequence = i;
                aDeparture.DepartureIdentity = "D1234";
                aDeparture.PickStatusCode = "A";
                aDeparture.PickStatusText = "A-text";
                aDeparture.LoadStatusCode = "B";
                aDeparture.LoadStatusText = "B-text";
                aDeparture.DepartureTime = DateTime.Now;
                aDeparture.MethodOfShipmentId = "MOS";
                aDeparture.MethodOfShipmentName = "MOS-Name";
                aDeparture.ForwarderId = "FWD";
                aDeparture.ForwarderName = "FWD.Name";
                aDeparture.DispatchRoute = "DisR";
                aDeparture.VehicleId = "Veh";

                aDepartureList.Add(aDeparture);
            }

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
    public partial class OrderQuantities
    {
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
        static public ArrayList MakeTestData()
        {
            ArrayList aOrderQuantitiesList = new ArrayList();

            for (int i = 0; i < 10; i++)
            {
                OrderQuantities aOrderQuantities = new OrderQuantities();

                aOrderQuantities.ClientIdentity = "IMISTD";
                aOrderQuantities.OrderNumber = "1234";
                aOrderQuantities.OrderSequence = i;
                aOrderQuantities.SubSequence = i;

                aOrderQuantities.TotalNoOfLines = 4;
                aOrderQuantities.PickedNoOfLines = 5;
                aOrderQuantities.EstimatedWeight = 6;
                aOrderQuantities.EstimatedVolume = 7;
                aOrderQuantities.PlannedNoOfPallets = 8;
                aOrderQuantities.PlannedNoOfLowestUM = 9;
                aOrderQuantities.PlannedVolume = 10;
                aOrderQuantities.PlannedWeight = 11;
                aOrderQuantities.PickedNoOfPallets = 12;
                aOrderQuantities.PickedNoOfLowestUM = 13;
                aOrderQuantities.PickedVolume = 14;
                aOrderQuantities.PickedWeight = 15;

                aOrderQuantitiesList.Add(aOrderQuantities);
            }

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
    public partial class CustomerOrderRow
    {
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
        static public ArrayList MakeTestData()
        {
            ArrayList aCustomerOrderRowList = new ArrayList();

            for (int i = 0; i < 10; i++)
            {
                CustomerOrderRow aCustomerOrderRow = new CustomerOrderRow();
                aCustomerOrderRow.ClientIdentity = "IMISTD";
                aCustomerOrderRow.OrderNumber = "1234";
                aCustomerOrderRow.OrderSequence = i;
                aCustomerOrderRow.SubSequence = i;
                aCustomerOrderRow.LinePosition = i;
                aCustomerOrderRow.LineSequence = i;
                aCustomerOrderRow.LineKitPosition = i;
                aCustomerOrderRow.LineStatus = "AB";
                aCustomerOrderRow.OrderedQuantity = 1;
                aCustomerOrderRow.PickedQuantity = 2;
                aCustomerOrderRow.DiscrepancyQuantity = 3;
                aCustomerOrderRow.DiscrepancyCode = "DC";
                aCustomerOrderRow.DiscrepancyText = "Disp";
                aCustomerOrderRow.ExternalDiscrepancyCode = "ExtDisp";

                aCustomerOrderRowList.Add(aCustomerOrderRow);
            }

            return aCustomerOrderRowList;
        }
    }
}
