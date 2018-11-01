using System;
using System.Data;
using System.Collections;

using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.Order
{
    public partial class WebServices3pl : IPackage
    {
        public orderSearchResult getOpenOrders(ISyncWSParameter dataParam,
            System.Nullable<orderType> ot,
            string clientId,
            string orderNo,
            string yourCono,
            System.Nullable<System.DateTime> from,
            System.Nullable<System.DateTime> to,
            string mark,
            string ordersFromHistory,
            string ordersFromProduction)
        {
            IDataReader reader = null;
            int totalRows;
            orderSearchResult res = new orderSearchResult();

            switch (ot)
            {
                case orderType.CO:
                    throw new Exception("ordertype CO not supported");
                case orderType.CO3PL:
                    Getco(
                        clientId,
                        orderNo,
                        yourCono,
                        from,
                        to,
                        mark,
                        ordersFromHistory,
                        ordersFromProduction,
                        out reader);
                    break;
                case orderType.PO:
                    Getpo(
                        clientId,
                        orderNo,
                        from,
                        to,
                        ordersFromHistory,
                        ordersFromProduction,
                        out reader);
                    break;
                default:
                    throw new Exception("unknown ordertype");
            }

            ArrayList list = ReaderHelper.Read(
                reader,
                new order(),
                dataParam.GetSkipNoFirstRows(),
                dataParam.GetMaxRows(),
                out totalRows);

            res.list = list.ToArray(typeof(order)) as order[];
            res.SetTotalRows(totalRows);

            return res;
        }

        public orderSearchResult getOrderDetails(ISyncWSParameter dataParam,
            System.Nullable<orderType> ot,
            string clientId,
            string orderNo,
            string ordersFromHistory,
            string ordersFromProduction)
        {
            return getOpenOrders(
                dataParam,
                ot,
                clientId,
                orderNo,
                null,
                null,
                null, 
                null,
                ordersFromHistory, 
                ordersFromProduction);
        }

        public orderLine[] GetOrderLines(ISyncWSParameter dataParam,
            System.Nullable<orderType> ot,
            string clientId,
            string coNo,
            string poid,
            Nullable<int> poSeq,
            string ordersFromHistory,
            string ordersFromProduction)
        {
            IDataReader reader = null;
            int totalRows;

            switch (ot)
            {
                case orderType.CO:
                    throw new Exception("ordertype CO not supported");
                case orderType.CO3PL:
                    Getcoline(
                        clientId,
                        coNo,
                        ordersFromHistory,
                        ordersFromProduction,
                        out reader);
                    break;
                case orderType.PO:
                    Getpoline(
                        clientId,
                        poid,
                        poSeq,
                        ordersFromHistory,
                        ordersFromProduction,
                        out reader);
                    break;
                default:
                    throw new Exception("unknown ordertype");
            }

            ArrayList list = ReaderHelper.Read(
                reader,
                new orderLine(),
                dataParam.GetSkipNoFirstRows(),
                dataParam.GetMaxRows(),
                out totalRows);

            return list.ToArray(typeof(orderLine)) as orderLine[];
        }

        public address[] getAddresses(ISyncWSParameter dataParam, string clientId, string customerId)
        {
            IDataReader reader = null;
            int totalRows;

            Getaddresses(
                clientId,
                customerId,
                out reader);

            ArrayList list = ReaderHelper.Read(
                reader,
                new address(),
                dataParam.GetSkipNoFirstRows(),
                dataParam.GetMaxRows(),
                out totalRows);

            return list.ToArray(typeof(address)) as address[];
        }
        public address[] Getpartyaddress(
            ISyncWSParameter dataParam, 
            string shipFromPartyid, 
            string shipFromPartyQualifier,
            string clientId)
        {
            IDataReader reader = null;
            int totalRows;

            Getpartyaddress(                
                shipFromPartyid,
                shipFromPartyQualifier,
                clientId,
                out reader);

            ArrayList list = ReaderHelper.Read(
                reader,
                new address(),
                dataParam.GetSkipNoFirstRows(),
                dataParam.GetMaxRows(),
                out totalRows);

            return list.ToArray(typeof(address)) as address[];
        }

        public customerOrderTypeSearchResult Findcustomerordertype(ISyncWSParameter dataParam)
        {
            IDataReader reader = null;
            customerOrderTypeSearchResult res = new customerOrderTypeSearchResult();
            int totalRows;

            Findcustomerordertype(
                out reader);

            ArrayList list = ReaderHelper.Read(
                reader,
                new customerOrderType(),
                dataParam.GetSkipNoFirstRows(),
                dataParam.GetMaxRows(),
                out totalRows);

            res.list = list.ToArray(typeof(customerOrderType)) as customerOrderType[];
            res.SetTotalRows(totalRows);

            return res;
        }
        public methodOfShipmentSearchResult Findmethodofshipmentbyclient(ISyncWSParameter dataParam, string clientId)
        {
            IDataReader reader = null;
            methodOfShipmentSearchResult res = new methodOfShipmentSearchResult();
            int totalRows;

            Findmethodofshipmentbyclient(
                clientId,
                out reader);

            ArrayList list = ReaderHelper.Read(
                reader,
                new methodOfShipment(),
                dataParam.GetSkipNoFirstRows(),
                dataParam.GetMaxRows(),
                out totalRows);

            res.list = list.ToArray(typeof(methodOfShipment)) as methodOfShipment[];
            res.SetTotalRows(totalRows);

            return res;
        }
    }
}