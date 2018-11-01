using System;
using System.Collections.Generic;
using System.Text;

using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.Warehouse
{
    [System.Web.Services.WebService(Namespace = "http://im.se/webservices/c3pl")]
    [System.Web.Services.Protocols.SoapDocumentService(RoutingStyle = System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement)]
    public class WarehouseWebService : IWarehouseServiceBinding
    {
        public warehouseSearchResult findByClient(warehouseSearchParameters warehouseSearchParameters)
        {
            Warehouse p = new Warehouse();
            return (warehouseSearchResult)PackageHelper.GetResult(warehouseSearchParameters, p.findByClient, p.find);
        }
    }
}
