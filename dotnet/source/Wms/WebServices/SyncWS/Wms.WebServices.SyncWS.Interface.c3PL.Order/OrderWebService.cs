using System;
using System.Collections.Generic;
using System.Text;

using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.Order
{
    [System.Web.Services.WebService(Namespace = "http://im.se/webservices/c3pl")]
    [System.Web.Services.Protocols.SoapDocumentService(RoutingStyle = System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement)]
    public class OrderWebService : IOrderServiceBinding
    {
        public orderSearchResult getOpenOrders(orderSearchParameters orderSearchParameters)
        {
            Order p = new Order();
            return (orderSearchResult)PackageHelper.GetResult(orderSearchParameters, p.getOpenOrders, p.find);
        }

        public order getOrderDetails(orderSearchParameters orderSearchParameters)
        {
            Order p = new Order();
            orderSearchResult orderSearchResult = (orderSearchResult)PackageHelper.GetResult(orderSearchParameters, p.getOrderDetails, p.find);
            if (orderSearchResult.list.GetLength(0) != 1)
                return null;
            else
                return orderSearchResult.list[0];
        }

        public order saveOrder(orderSaveParameters orderSaveParameters)
        {
            Order p = new Order();
            orderSaveResult orderSaveResult = (orderSaveResult)PackageHelper.GetResult(orderSaveParameters, p.saveOrder, p.find);

            switch (orderSaveParameters.order.orderType)
            {
                case orderType.CO:
                    throw new Exception("Order type CO not supported");
                case orderType.CO3PL:
                    orderSaveResult orderSignalResult = (orderSaveResult)PackageHelper.GetResult(orderSaveParameters, p.SignalProcess, p.find);
                    break;
                case orderType.PO:
                    break;
                default:
                    throw new Exception("Unknown order type");
            }

            // reload it
            orderSearchParameters orderSearchParameters = new orderSearchParameters();
            orderSearchParameters.clientId = orderSaveParameters.clientId;
            orderSearchParameters.returnDetails = true;
            orderSearchParameters.orderType = orderSaveParameters.order.orderType;

            switch (orderSaveParameters.order.orderType)
            {
                case orderType.CO:
                    throw new Exception("Order type CO not supported");
                case orderType.CO3PL:
                    orderSearchParameters.orderNo = orderSaveParameters.order.coNo;
                    break;
                case orderType.PO:
                    orderSearchParameters.orderNo = orderSaveParameters.order.poid;
                    break;
                default:
                    throw new Exception("Unknown order type");
            }

            orderSearchResult orderSearchResult = (orderSearchResult)PackageHelper.GetResult(orderSearchParameters, p.getOrderDetails, p.find);
            if (orderSearchResult.list.GetLength(0) != 1)
                return null;
            else
                return orderSearchResult.list[0];
        }

        public order validateOrder(orderSaveParameters orderSaveParameters)
        {
            return orderSaveParameters.order;
        }

        public customerOrderTypeSearchResult getCustomerOrderTypes()
        {
            customerOrderSearchParameters customerOrderSearchParameters = new customerOrderSearchParameters();

            Order p = new Order();
            return (customerOrderTypeSearchResult)PackageHelper.GetResult(
                customerOrderSearchParameters, p.getCustomerOrderTypes, p.find);
        }

        public methodOfShipmentSearchResult getMethodOfShipments(methodOfShipmentSearchParameters methodOfShipmentSearchParameters)
        {
            Order p = new Order();
            return (methodOfShipmentSearchResult)PackageHelper.GetResult(
                methodOfShipmentSearchParameters, p.getMethodOfShipments, p.find);
        }
    }
}
