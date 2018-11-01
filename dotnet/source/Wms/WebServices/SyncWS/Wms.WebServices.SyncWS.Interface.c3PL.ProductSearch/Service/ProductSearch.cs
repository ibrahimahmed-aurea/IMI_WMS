using System;
using System.Web;
using System.Web.Services;
using System.Collections;
using System.Data;

using Imi.Framework.Job.Data;
using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.ProductSearch
{
    public class ProductSearch
    {
        public ISyncWSResult findProductsByPartNo(Database database, ISyncWSParameter dataParameter)
        {
            productSearchParameters productSearchParameters = (productSearchParameters)dataParameter;

            WebServices3pl pkg = new WebServices3pl(database);

            productSearchResult res = pkg.Findproductbypartno(dataParameter, productSearchParameters.clientId, productSearchParameters.searchString);

            Details(res.list, pkg, dataParameter, productSearchParameters);

            return (searchResult)res;
        }

        public ISyncWSResult findProducts(ISyncWSParameter dataParameter)
        {
            productSearchResult res = new productSearchResult();
            ArrayList list = ReaderHelper.MakeTestData(10,new product());
            res.list = list.ToArray(typeof(product)) as product[];
            res.totalHits = res.list.GetLength(0);
            return (searchResult)res;
        }

        public ISyncWSResult findProductsByDescription(Database database, ISyncWSParameter dataParameter)
        {
            productSearchParameters productSearchParameters = (productSearchParameters)dataParameter;

            WebServices3pl pkg = new WebServices3pl(database);

            productSearchResult res = pkg.Findproductbydescription(dataParameter, productSearchParameters.clientId, productSearchParameters.searchString);

            Details(res.list, pkg, dataParameter, productSearchParameters);

            return (searchResult)res;
        }

        private void Details(product[] list, WebServices3pl pkg, ISyncWSParameter dataParameter, productSearchParameters productSearchParameters)
        {
            if (dataParameter.GetReturnDetails())
            {
                foreach (product item in list)
                {
                    productSearchParameters subParams = new productSearchParameters();
                    subParams.firstResult = null;
                    subParams.maxResult = null;

                    item.barcodes = pkg.Findbarcodebyproduct(subParams, productSearchParameters.clientId, item.partNo);

                    vendorPartNos[] vendorPartNo = pkg.Getvendorpartno(subParams, productSearchParameters.clientId, item.partNo);

                    // build vendor part no string
                    item.vendPartNo = "";

                    foreach (vendorPartNos vpn in vendorPartNo)
                    {
                        if (!String.IsNullOrEmpty(vpn.vendorPartNo))
                        {
                            if (item.vendPartNo != "")
                                item.vendPartNo += ", ";

                            item.vendPartNo += vpn.vendorPartNo;
                        }
                    }

                    if (String.IsNullOrEmpty(item.vendPartNo))
                        item.vendPartNo = null;

                    // build product availability
                    if (!String.IsNullOrEmpty(productSearchParameters.stockNo))
                    {
                        productAvails[] productAvail = pkg.Getproductavail(subParams, productSearchParameters.clientId, productSearchParameters.stockNo, item.partNo);
                        int qty = 0;
                        foreach (productAvails pa in productAvail)
                        {
                            if (pa.productAvail != null)
                                qty += (int)pa.productAvail;
                        }
                        item.availability = qty;
                    }
                }
            }
        }
    }
}
