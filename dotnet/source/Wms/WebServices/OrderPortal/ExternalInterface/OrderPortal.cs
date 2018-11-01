using System;
using System.Web;
using System.Web.Services;
using System.Collections;
using Imi.Wms.WebServices.OrderPortal.PLSQLInterface;
using Imi.Framework.Job.Data;
using System.Data;

namespace Imi.Wms.WebServices.OrderPortal.ExternalInterface
{
    public class OrderPortal : WSBase
    {
        public CustomerOrderHeadInfoDoc GetCustomerOrderHeadInfo(
          string ChannelId,
          string LanguageId,
          string ClientIdentity,
          string OrderNumber,
          Nullable<int> OrderSequence)
        {
            CustomerOrderHeadInfoDoc res = new CustomerOrderHeadInfoDoc();

            try
            {
                BeginWebmethod(ChannelId);

                try
                {
                    WebServicesQuery pkg = new WebServicesQuery(GetDataBase());

                    IDataReader aCustomerOrderRdr = null;
                    IDataReader aCustomerWorkOrderRdr = null;
                    IDataReader aDepartureRdr = null;
                    IDataReader aOrderQuantitiesRdr = null;

                    pkg.GetCustomerOrderHeadInfo(ClientIdentity,
                                                 OrderNumber,
                                                 OrderSequence,
                                                 LanguageId,
                                                 out aCustomerOrderRdr,
                                                 out aCustomerWorkOrderRdr,
                                                 out aDepartureRdr,
                                                 out aOrderQuantitiesRdr);

                    ArrayList aCustomerOrderList = CustomerOrder.Read(aCustomerOrderRdr);
                    ArrayList aCustomerWorkOrderList = CustomerWorkOrder.Read(aCustomerWorkOrderRdr);
                    ArrayList aDepartureList = Departure.Read(aDepartureRdr);
                    ArrayList aOrderQuantitiesList = OrderQuantities.Read(aOrderQuantitiesRdr);

                    if (aCustomerOrderList.Count > 0)
                        res.aCustomerOrderList = aCustomerOrderList.ToArray(typeof(CustomerOrder)) as CustomerOrder[];

                    if (aCustomerWorkOrderList.Count > 0)
                        res.aCustomerWorkOrderList = aCustomerWorkOrderList.ToArray(typeof(CustomerWorkOrder)) as CustomerWorkOrder[];

                    if (aDepartureList.Count > 0)
                        res.aDepartureList = aDepartureList.ToArray(typeof(Departure)) as Departure[];

                    if (aOrderQuantitiesList.Count > 0)
                        res.aOrderQuantitiesList = aOrderQuantitiesList.ToArray(typeof(OrderQuantities)) as OrderQuantities[];

                    GetDataBase().Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        GetDataBase().Rollback();
                    }
                    catch (Exception)
                    { }
                    Exception InternalError = new Exception("DataError: Error processing data", e);
                    throw (InternalError);
                }
            }

            finally
            {
                EndWebmethod();
            }

            return res;
        }

        public CustomerOrderInfoDoc GetCustomerOrderInfo(
          string ChannelId,
          string LanguageId,
          string ClientIdentity,
          string OrderNumber,
          Nullable<int> OrderSequence)
        {
            CustomerOrderInfoDoc res = new CustomerOrderInfoDoc();

            try
            {
                BeginWebmethod(ChannelId);

                try
                {
                    WebServicesQuery pkg = new WebServicesQuery(GetDataBase());

                    IDataReader aCustomerOrderRdr = null;
                    IDataReader aCustomerWorkOrderRdr = null;
                    IDataReader aDepartureRdr = null;
                    IDataReader aOrderQuantitiesRdr = null;
                    IDataReader aCustomerOrderRowRdr = null;

                    pkg.GetCustomerOrderInfo(ClientIdentity,
                                             OrderNumber,
                                             OrderSequence,
                                             LanguageId,
                                             out aCustomerOrderRdr,
                                             out aCustomerWorkOrderRdr,
                                             out aDepartureRdr,
                                             out aOrderQuantitiesRdr,
                                             out aCustomerOrderRowRdr);

                    ArrayList aCustomerOrderList = CustomerOrder.Read(aCustomerOrderRdr);
                    ArrayList aCustomerWorkOrderList = CustomerWorkOrder.Read(aCustomerWorkOrderRdr);
                    ArrayList aDepartureList = Departure.Read(aDepartureRdr);
                    ArrayList aOrderQuantitiesList = OrderQuantities.Read(aOrderQuantitiesRdr);
                    ArrayList aCustomerOrderRowList = CustomerOrderRow.Read(aCustomerOrderRowRdr);

                    res.aCustomerOrderList = aCustomerOrderList.ToArray(typeof(CustomerOrder)) as CustomerOrder[];
                    res.aCustomerWorkOrderList = aCustomerWorkOrderList.ToArray(typeof(CustomerWorkOrder)) as CustomerWorkOrder[];
                    res.aDepartureList = aDepartureList.ToArray(typeof(Departure)) as Departure[];
                    res.aOrderQuantitiesList = aOrderQuantitiesList.ToArray(typeof(OrderQuantities)) as OrderQuantities[];
                    res.aCustomerOrderRowList = aCustomerOrderRowList.ToArray(typeof(CustomerOrderRow)) as CustomerOrderRow[];

                    GetDataBase().Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        GetDataBase().Rollback();
                    }
                    catch (Exception)
                    { }
                    Exception InternalError = new Exception("DataError: Error processing data", e);
                    throw (InternalError);
                }
            }

            finally
            {
                EndWebmethod();
            }

            return res;
        }

        public CustomerOrderLineRangeDoc GetCustomerOrderLineRange(
          string ChannelId,
          string LanguageId,
          string ClientIdentity,
          string OrderNumber,
          Nullable<int> OrderSequence,
          Nullable<int> FirstLinePosition,
          Nullable<int> LastLinePosition)
        {
            CustomerOrderLineRangeDoc res = new CustomerOrderLineRangeDoc();

            try
            {
                BeginWebmethod(ChannelId);

                try
                {
                    WebServicesQuery pkg = new WebServicesQuery(GetDataBase());

                    IDataReader aCustomerOrderRdr = null;
                    IDataReader aCustomerWorkOrderRdr = null;
                    IDataReader aDepartureRdr = null;
                    IDataReader aOrderQuantitiesRdr = null;
                    IDataReader aCustomerOrderRowRdr = null;

                    pkg.GetCustomerOrderLineRange(ClientIdentity,
                                                  OrderNumber,
                                                  OrderSequence,
                                                  LanguageId,
                                                  FirstLinePosition,
                                                  LastLinePosition,
                                                  out aCustomerOrderRdr,
                                                  out aCustomerWorkOrderRdr,
                                                  out aDepartureRdr,
                                                  out aOrderQuantitiesRdr,
                                                  out aCustomerOrderRowRdr);

                    ArrayList aCustomerOrderList = CustomerOrder.Read(aCustomerOrderRdr);
                    ArrayList aCustomerWorkOrderList = CustomerWorkOrder.Read(aCustomerWorkOrderRdr);
                    ArrayList aDepartureList = Departure.Read(aDepartureRdr);
                    ArrayList aOrderQuantitiesList = OrderQuantities.Read(aOrderQuantitiesRdr);
                    ArrayList aCustomerOrderRowList = CustomerOrderRow.Read(aCustomerOrderRowRdr);

                    res.aCustomerOrderList = aCustomerOrderList.ToArray(typeof(CustomerOrder)) as CustomerOrder[];
                    res.aCustomerWorkOrderList = aCustomerWorkOrderList.ToArray(typeof(CustomerWorkOrder)) as CustomerWorkOrder[];
                    res.aDepartureList = aDepartureList.ToArray(typeof(Departure)) as Departure[];
                    res.aOrderQuantitiesList = aOrderQuantitiesList.ToArray(typeof(OrderQuantities)) as OrderQuantities[];
                    res.aCustomerOrderRowList = aCustomerOrderRowList.ToArray(typeof(CustomerOrderRow)) as CustomerOrderRow[];

                    GetDataBase().Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        GetDataBase().Rollback();
                    }
                    catch (Exception)
                    { }
                    Exception InternalError = new Exception("DataError: Error processing data", e);
                    throw (InternalError);
                }
            }

            finally
            {
                EndWebmethod();
            }

            return res;
        }
    }
}
