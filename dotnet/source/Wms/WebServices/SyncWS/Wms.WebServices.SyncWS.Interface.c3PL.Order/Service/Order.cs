using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using Imi.Framework.Job.Data;
using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.Order
{
    public class Order
    {
        public ISyncWSResult getOpenOrders(Database database, ISyncWSParameter dataParameter)
        {
            orderSearchParameters orderSearchParameters = (orderSearchParameters)dataParameter;

            WebServices3pl pkg = new WebServices3pl(database);

            orderSearchResult res = pkg.getOpenOrders(dataParameter,
                orderSearchParameters.orderType,
                orderSearchParameters.clientId,
                orderSearchParameters.orderNo,
                orderSearchParameters.yourCono,
                orderSearchParameters.from,
                orderSearchParameters.to,
                orderSearchParameters.mark,
                boolToYesNo(orderSearchParameters.ordersFromHistory),
                boolToYesNo(orderSearchParameters.ordersFromProduction));

            Details(dataParameter, orderSearchParameters, res, pkg);

            return (searchResult)res;
        }

        public ISyncWSResult getOrderDetails(Database database, ISyncWSParameter dataParameter)
        {
            orderSearchParameters orderSearchParameters = (orderSearchParameters)dataParameter;

            WebServices3pl pkg = new WebServices3pl(database);

            orderSearchResult res = pkg.getOrderDetails(dataParameter,
                orderSearchParameters.orderType,
                orderSearchParameters.clientId,
                orderSearchParameters.orderNo,
                boolToYesNo(orderSearchParameters.ordersFromHistory),
                boolToYesNo(orderSearchParameters.ordersFromProduction));

            Details(dataParameter, orderSearchParameters, res, pkg);

            return (searchResult)res;
        }

        public ISyncWSResult find(ISyncWSParameter dataParameter)
        {
            // test data generator
            return null;
        }

        public ISyncWSResult saveOrder(Database database, ISyncWSParameter dataParameter)
        {
            orderSaveParameters orderSaveParameters = (orderSaveParameters)dataParameter;
            orderSaveResult res = null;

            switch (orderSaveParameters.order.orderType)
            {
                case orderType.CO:
                    throw new Exception("orderType CO not supported");
                case orderType.CO3PL:
                    res = saveCO3PL(database, orderSaveParameters);
                    break;
                case orderType.PO:
                    res = savePO(database, orderSaveParameters);
                    break;
                default:
                    throw new Exception("unknown orderType");
            }

            return res;
        }

        private orderSaveResult saveCO3PL(Database database, orderSaveParameters orderSaveParameters)
        {
            orderSaveResult res = new orderSaveResult();

            int COSEQ_W = 0;
            WebServices3pl pkg = new WebServices3pl(database);

            shippingAddress shippingAddress = orderSaveParameters.order.shippingAddress;
            if (shippingAddress == null)
                shippingAddress = new shippingAddress();

            if (shippingAddress.address == null)
                shippingAddress.address = new address();

            pkg.Saveco(orderSaveParameters.clientId,
                orderSaveParameters.order.coNo,
                orderSaveParameters.order.customer,
                orderSaveParameters.order.cuRef,
                orderSaveParameters.order.yourCono,
                orderSaveParameters.order.reqDelDate,
                orderSaveParameters.order.text,
                orderSaveParameters.order.coMark,
                orderSaveParameters.stockNo,
                orderSaveParameters.order.methodOfShipmentId,
                orderSaveParameters.order.customerOrderTypeId,
                COSEQ_W,
                shippingAddress.address.name1,
                shippingAddress.address.name2,
                shippingAddress.address.street1,
                shippingAddress.address.street2,
                shippingAddress.address.street3,
                shippingAddress.address.zipCode,
                shippingAddress.address.city,
                shippingAddress.address.countryCd,
                null // phone
                );

            foreach (orderLine x in orderSaveParameters.order.orderLines)
            {
                pkg.Savecoline(orderSaveParameters.clientId,
                    x.partNo,
                    x.partDescr1,
                    x.partDescr2,
                    x.custPartNo,
                    x.custPartDescr1,
                    x.custPartDescr2,
                    x.text,
                    (Nullable<double>)x.qtyUnit,
                    (Nullable<int>)x.originalQtyUnit,
                    x.unit,
                    null, ///////// unitDescr
                    x.linePos,
                    x.lineSeq,
                    x.lineId,
                    x.sellingUnit,
                    x.sellingUnitDesc,
                    x.delDate,
                    Convert.ToString(x.lineStatus),
                    orderSaveParameters.order.coNo,
                    COSEQ_W,
                    x.blockCod,
                    x.serial,
                    x.serNumb,
                    x.prodLot,
                    x.promotn,
                    x.storBat,
                    x.prodLotReq);
            }

            return res;
        }

        private orderSaveResult savePO(Database database, orderSaveParameters orderSaveParameters)
        {
            orderSaveResult res = new orderSaveResult();

            WebServices3pl pkg = new WebServices3pl(database);

            pkg.Savepo(orderSaveParameters.clientId,
                orderSaveParameters.order.poid,
                orderSaveParameters.order.poSeq,
                orderSaveParameters.order.shipFromPartyid,
                orderSaveParameters.order.reqDelDate,
                orderSaveParameters.order.text,
                orderSaveParameters.order.receivingWhid);

            foreach (orderLine x in orderSaveParameters.order.orderLines)
            {
                pkg.Savepoline(orderSaveParameters.clientId,
                    x.partNo,
                    x.partDescr1,
                    x.text,
                    (Nullable<double>)x.qtyUnit,
                    x.unit,
                    orderSaveParameters.order.poid,
                    orderSaveParameters.order.poSeq,
                    x.linePos,
                    x.lineSeq,
                    orderSaveParameters.order.reqDelDate,
                    orderSaveParameters.order.receivingWhid);
            }

            return res;
        }

        public ISyncWSResult SignalProcess(Database database, ISyncWSParameter dataParameter)
        {
            orderSaveParameters orderSaveParameters = (orderSaveParameters)dataParameter;
            orderSaveResult res = new orderSaveResult();

            WebServices3pl pkg = new WebServices3pl(database);

            pkg.SignalProcess("CO");

            res.totalHits = 0;
            return res;
        }

        private string boolToYesNo(bool yesno)
        {
            if (yesno)
                return "1";
            else
                return "0";
        }

        private void Details(ISyncWSParameter dataParameter, orderSearchParameters orderSearchParameters, orderSearchResult res, WebServices3pl pkg)
        {
            if (dataParameter.GetReturnDetails())
            {
                foreach (order item in res.list)
                {
                    orderSearchParameters subParams = new orderSearchParameters();
                    subParams.firstResult = null;
                    subParams.maxResult = null;

                    switch (orderSearchParameters.orderType)
                    {
                        case orderType.CO:
                            throw new Exception("ordertype CO not supported");
                        case orderType.CO3PL:
                            
                            if (!String.IsNullOrEmpty(item.customer))
                            {
                                address[] address = pkg.getAddresses(
                                    subParams, 
                                    orderSearchParameters.clientId, 
                                    item.customer);

                                if (address.GetLength(0) == 1)
                                {
                                    item.shippingAddress = new shippingAddress();
                                    item.shippingAddress.shipCustNo = item.customer;
                                    item.shippingAddress.shiptoNo = "0";
                                    item.shippingAddress.address = address[0];
                                }
                            }

                            break;
                        case orderType.PO:
                            {
                                address[] address = pkg.Getpartyaddress(
                                    subParams, 
                                    item.shipFromPartyid, 
                                    item.shipFromPartyQualifier, 
                                    orderSearchParameters.clientId);

                                if (address.GetLength(0) == 1)
                                {
                                    item.orderShipFrom = new orderShipFrom();
                                    item.orderShipFrom.shipFromPartyid = item.shipFromPartyid;
                                    item.orderShipFrom.shipFromPartyQualifier = ConvertPartyType(item.shipFromPartyQualifier);
                                    item.orderShipFrom.shipFromPartyQualifierSpecified = true;
                                    item.orderShipFrom.address = address[0];
                                }
                            }
                            break;
                        default:
                            throw new Exception("unknown ordertype");
                    }

                    item.orderLines = pkg.GetOrderLines(subParams,
                        orderSearchParameters.orderType,
                        orderSearchParameters.clientId,
                        item.coNo,
                        item.poid,
                        item.poSeq,
                        boolToYesNo(orderSearchParameters.ordersFromHistory),
                        boolToYesNo(orderSearchParameters.ordersFromProduction));
                }
            }
        }

        private partyType ConvertPartyType(string pt)
        {
            partyType partyQualifier;
            switch (pt)
            {
                case "CA":
                    partyQualifier = partyType.CA;
                    break;
                case "CL":
                    partyQualifier = partyType.CL;
                    break;
                case "CU":
                    partyQualifier = partyType.CU;
                    break;
                case "GO":
                    partyQualifier = partyType.GO;
                    break;
                case "OT":
                    partyQualifier = partyType.OT;
                    break;
                case "SU":
                    partyQualifier = partyType.SU;
                    break;
                case "WH":
                    partyQualifier = partyType.WH;
                    break;
                default:
                    throw new Exception("Party type not supported");
            }
            return partyQualifier;
        }

        public ISyncWSResult getCustomerOrderTypes(Database database, ISyncWSParameter dataParameter)
        {
            customerOrderSearchParameters customerOrderSearchParameters = (customerOrderSearchParameters)dataParameter;

            WebServices3pl pkg = new WebServices3pl(database);

            customerOrderTypeSearchResult res = pkg.Findcustomerordertype(dataParameter);

            return res;
        }

        public ISyncWSResult getMethodOfShipments(Database database, ISyncWSParameter dataParameter)
        {
            methodOfShipmentSearchParameters methodOfShipmentSearchParameters = (methodOfShipmentSearchParameters)dataParameter;

            WebServices3pl pkg = new WebServices3pl(database);

            methodOfShipmentSearchResult res = (methodOfShipmentSearchResult)pkg.Findmethodofshipmentbyclient(dataParameter,
                methodOfShipmentSearchParameters.clientId);

            return res;
        }
    }
}
