using System;
using System.Web;
using System.Web.Services;
using System.Collections;
using System.Data;

using Imi.Framework.Job.Data;
using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.OrderPortal
{
    public partial class OrderPortal
    {
        public CustomerOrderHeadInfoDoc GetCustomerOrderHeadInfo(
          string PartnerName,
          string LanguageId,
          string ClientIdentity,
          string OrderNumber,
          Nullable<int> OrderSequence)
        {
            CustomerOrderHeadInfoDoc res = new CustomerOrderHeadInfoDoc();

            using (DBHelper dbHelper = new DBHelper(PartnerName))
            {
                try
                {
                    WebServicesQuery pkg = new WebServicesQuery(dbHelper.GetDataBase());

                    IDataReader aCustomerOrderRdr = null;
                    IDataReader aCustomerWorkOrderRdr = null;
                    IDataReader aDepartureRdr = null;
                    IDataReader aOrderQuantitiesRdr = null;

                    pkg.Getcustomerorderheadinfo(ClientIdentity,
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

                    dbHelper.GetDataBase().Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        dbHelper.GetDataBase().Rollback();
                    }
                    catch (Exception)
                    { }
                    Exception InternalError = new Exception("DataError: Error processing data", e);
                    throw (InternalError);
                }
            }

            return res;
        }

        public CustomerOrderInfoDoc GetCustomerOrderInfo(
          string PartnerName,
          string LanguageId,
          string ClientIdentity,
          string OrderNumber,
          Nullable<int> OrderSequence)
        {
            if ((PartnerName == "") || (PartnerName == null))
            {
                // test mode!
                CustomerOrderInfoDoc testres = new CustomerOrderInfoDoc();

                ArrayList aCustomerOrderList = CustomerOrder.MakeTestData();
                ArrayList aCustomerWorkOrderList = CustomerWorkOrder.MakeTestData();
                ArrayList aDepartureList = Departure.MakeTestData();
                ArrayList aOrderQuantitiesList = OrderQuantities.MakeTestData();
                ArrayList aCustomerOrderRowList = CustomerOrderRow.MakeTestData();

                testres.aCustomerOrderList = aCustomerOrderList.ToArray(typeof(CustomerOrder)) as CustomerOrder[];
                testres.aCustomerWorkOrderList = aCustomerWorkOrderList.ToArray(typeof(CustomerWorkOrder)) as CustomerWorkOrder[];
                testres.aDepartureList = aDepartureList.ToArray(typeof(Departure)) as Departure[];
                testres.aOrderQuantitiesList = aOrderQuantitiesList.ToArray(typeof(OrderQuantities)) as OrderQuantities[];
                testres.aCustomerOrderRowList = aCustomerOrderRowList.ToArray(typeof(CustomerOrderRow)) as CustomerOrderRow[];
                return testres;
            }

            CustomerOrderInfoDoc res = new CustomerOrderInfoDoc();

            using (DBHelper dbHelper = new DBHelper(PartnerName))
            {
                try
                {
                    WebServicesQuery pkg = new WebServicesQuery(dbHelper.GetDataBase());

                    IDataReader aCustomerOrderRdr = null;
                    IDataReader aCustomerWorkOrderRdr = null;
                    IDataReader aDepartureRdr = null;
                    IDataReader aOrderQuantitiesRdr = null;
                    IDataReader aCustomerOrderRowRdr = null;

                    pkg.Getcustomerorderinfo(ClientIdentity,
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

                    dbHelper.GetDataBase().Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        dbHelper.GetDataBase().Rollback();
                    }
                    catch (Exception)
                    { }
                    Exception InternalError = new Exception("DataError: Error processing data", e);
                    throw (InternalError);
                }
            }

            return res;
        }

        public CustomerOrderLineRangeDoc GetCustomerOrderLineRange(
          string PartnerName,
          string LanguageId,
          string ClientIdentity,
          string OrderNumber,
          Nullable<int> OrderSequence,
          Nullable<int> FirstLinePosition,
          Nullable<int> LastLinePosition)
        {
            CustomerOrderLineRangeDoc res = new CustomerOrderLineRangeDoc();

            using (DBHelper dbHelper = new DBHelper(PartnerName))
            {
                try
                {
                    WebServicesQuery pkg = new WebServicesQuery(dbHelper.GetDataBase());

                    IDataReader aCustomerOrderRdr = null;
                    IDataReader aCustomerWorkOrderRdr = null;
                    IDataReader aDepartureRdr = null;
                    IDataReader aOrderQuantitiesRdr = null;
                    IDataReader aCustomerOrderRowRdr = null;

                    pkg.Getcustomerorderlinerange(ClientIdentity,
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

                    dbHelper.GetDataBase().Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        dbHelper.GetDataBase().Rollback();
                    }
                    catch (Exception)
                    { }
                    Exception InternalError = new Exception("DataError: Error processing data", e);
                    throw (InternalError);
                }
            }

            return res;
        }
    }
}
