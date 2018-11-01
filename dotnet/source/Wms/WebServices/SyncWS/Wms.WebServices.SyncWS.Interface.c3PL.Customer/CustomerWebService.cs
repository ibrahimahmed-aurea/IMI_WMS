using System;
using System.Collections.Generic;
using System.Text;

using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.Customer
{
    [System.Web.Services.WebService(Namespace = "http://im.se/webservices/c3pl")]
    [System.Web.Services.Protocols.SoapDocumentService(RoutingStyle = System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement)]
    public class CustomerWebService : ICustomerServiceBinding
    {
        public address[] getAddresses(customerSearchParameters customerSearchParameters)
        {
            Customer p = new Customer();
            addressSearchResult addressSearchResult = (addressSearchResult)PackageHelper.GetResult(customerSearchParameters, p.getAddresses, p.find);
            return addressSearchResult.list;
        }

        public customer findCustomerById(customerSearchParameters customerSearchParameters)
        {
            Customer p = new Customer();

            if (customerSearchParameters.customerId == null)
            {
                throw new Exception("customerId must not be null");
            }

            customerSearchResult customerSearchResult = (customerSearchResult)PackageHelper.GetResult(customerSearchParameters, p.findCustomerById, p.find);
            if (customerSearchResult.list.GetLength(0) != 1)
                return null;
            else
                return customerSearchResult.list[0];
        }

        public customer findClientById(customerSearchParameters customerSearchParameters)
        {
            Customer p = new Customer();
            
            // make sure customerId is null since this is what triggers a search for the client's party
            customerSearchParameters.customerId = null;

            customerSearchResult customerSearchResult = (customerSearchResult)PackageHelper.GetResult(customerSearchParameters, p.findCustomerById, p.find);
            if (customerSearchResult.list.GetLength(0) == 0)
                return null;
            else
            {
                // default to the first one even if many are found
                return customerSearchResult.list[0];
            }
        }

        public customer findPartyById(partySearchParameters partySearchParameters)
        {
            Customer p = new Customer();

            partySearchResult partySearchResult = (partySearchResult)PackageHelper.GetResult(partySearchParameters, p.findPartyById, p.find);
            if (partySearchResult.list.GetLength(0) == 0)
                return null;
            else
            {
                // default to the first one even if many are found
                return partySearchResult.list[0];
            }
        }

    }
}
