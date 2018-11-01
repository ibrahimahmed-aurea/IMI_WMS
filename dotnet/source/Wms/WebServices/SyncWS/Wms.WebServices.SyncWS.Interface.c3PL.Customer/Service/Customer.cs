using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using Imi.Framework.Job.Data;
using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.Customer
{
    public class Customer
    {
        public ISyncWSResult getAddresses(Database database, ISyncWSParameter dataParameter)
        {
            customerSearchParameters customerSearchParameters = (customerSearchParameters)dataParameter;

            WebServices3pl pkg = new WebServices3pl(database);

            addressSearchResult res = pkg.getAddresses(dataParameter, customerSearchParameters.clientId, customerSearchParameters.customerId);

            return (searchResult)res;
        }

        public ISyncWSResult findCustomerById(Database database, ISyncWSParameter dataParameter)
        {
            customerSearchParameters customerSearchParameters = (customerSearchParameters)dataParameter;

            WebServices3pl pkg = new WebServices3pl(database);

            customerSearchResult res = pkg.findCustomerById(dataParameter, customerSearchParameters.clientId, customerSearchParameters.customerId);

            if (dataParameter.GetReturnDetails())
            {
                foreach (customer item in res.list)
                {
                    customerSearchParameters subParams = new customerSearchParameters();
                    subParams.firstResult = null;
                    subParams.maxResult = null;

                    addressSearchResult addressResult;
                    
                    if (customerSearchParameters.customerId != null)
                        addressResult = pkg.getAddresses(subParams, customerSearchParameters.clientId, item.id);
                    else
                        addressResult = pkg.getAddresses(subParams, customerSearchParameters.clientId, null);

                    int i = 0;
                    foreach (address address in addressResult.list)
                    {
                        item.shippingAddress = new shippingAddress();
                        item.shippingAddress.shipCustNo = item.id;
                        item.shippingAddress.shiptoNo = Convert.ToString(i);
                        item.shippingAddress.address = address;
                    }
                }
            }

            return (searchResult)res;
        }

        public ISyncWSResult findPartyById(Database database, ISyncWSParameter dataParameter)
        {
            partySearchParameters partySearchParameters = (partySearchParameters)dataParameter;

            WebServices3pl pkg = new WebServices3pl(database);

            partySearchResult res = pkg.findPartyById(
                dataParameter,
                partySearchParameters.clientId, 
                partySearchParameters.partyId, 
                partySearchParameters.partyType);

            if (dataParameter.GetReturnDetails())
            {
                foreach (customer item in res.list)
                {
                    partySearchParameters subParams = new partySearchParameters();
                    subParams.firstResult = null;
                    subParams.maxResult = null;

                    addressSearchResult addressResult;

                    addressResult = pkg.getPartyAddress(subParams, partySearchParameters.clientId, item.id, partySearchParameters.partyType);

                    int i = 0;
                    foreach (address address in addressResult.list)
                    {
                        item.shippingAddress = new shippingAddress();
                        item.shippingAddress.shipCustNo = item.id;
                        item.shippingAddress.shiptoNo = Convert.ToString(i);
                        item.shippingAddress.address = address;
                    }
                }
            }

            return (searchResult)res;
        }

        public ISyncWSResult find(ISyncWSParameter dataParameter)
        {
            // test data generator
            return null;
        }
    }
}
