using System;
using System.Collections.Generic;
using System.Text;

using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.ProductSearch
{
    [System.Web.Services.WebService(Namespace = "http://im.se/webservices/c3pl")]
    [System.Web.Services.Protocols.SoapDocumentService(RoutingStyle = System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement)]
    public class ProductSearchWebService : IProductSearchServiceBinding
    {
        public productSearchResult findProductsByPartNo(productSearchParameters productSearchParameters)
        {
            ProductSearch p = new ProductSearch();
            return (productSearchResult)PackageHelper.GetResult(productSearchParameters, p.findProductsByPartNo, p.findProducts);
        }

        public productSearchResult findProductsByDescription(productSearchParameters productSearchParameters)
        {
            ProductSearch p = new ProductSearch();
            return (productSearchResult)PackageHelper.GetResult(productSearchParameters, p.findProductsByDescription, p.findProducts);
        }
    }
}
