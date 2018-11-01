using System;
using System.Data;
using System.Collections;

using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.ProductSearch
{
    public partial class WebServices3pl : IPackage
    {
        public productSearchResult Findproductbypartno(ISyncWSParameter dataParam, string clientId, string partNo)
        {
            IDataReader reader = null;
            int totalRows;
            productSearchResult res = new productSearchResult();

            Findproductbypartno(
                clientId,
                partNo,
                out reader);

            ArrayList list = ReaderHelper.Read(
                reader,
                new product(),
                dataParam.GetSkipNoFirstRows(),
                dataParam.GetMaxRows(),
                out totalRows);

            res.list = list.ToArray(typeof(product)) as product[];
            res.SetTotalRows(totalRows);

            return res;
        }

        public barcode[] Findbarcodebyproduct(ISyncWSParameter dataParam, string clientId, string partNo)
        {
            IDataReader reader = null;
            int totalRows;

            Findbarcodebyproduct(
                clientId,
                partNo,
                out reader);

            ArrayList list = ReaderHelper.Read(
                reader,
                new barcode(),
                dataParam.GetSkipNoFirstRows(),
                dataParam.GetMaxRows(),
                out totalRows);

            return list.ToArray(typeof(barcode)) as barcode[];
        }

        public productSearchResult Findproductbydescription(ISyncWSParameter dataParam, string clientId, string partDescr)
        {
            IDataReader reader = null;
            int totalRows;
            productSearchResult res = new productSearchResult();

            Findproductbydescription(
                clientId,
                partDescr,
                out reader);

            ArrayList list = ReaderHelper.Read(
                reader,
                new product(),
                dataParam.GetSkipNoFirstRows(),
                dataParam.GetMaxRows(),
                out totalRows);

            res.list = list.ToArray(typeof(product)) as product[];
            res.SetTotalRows(totalRows);

            return res;
        }

        public vendorPartNos[] Getvendorpartno(ISyncWSParameter dataParam, string clientId, string partNo)
        {
            IDataReader reader = null;
            int totalRows;

            Getvendorpartno(
                clientId,
                partNo,
                out reader);

            ArrayList list = ReaderHelper.Read(
                reader,
                new vendorPartNos(),
                dataParam.GetSkipNoFirstRows(),
                dataParam.GetMaxRows(),
                out totalRows);

            return list.ToArray(typeof(vendorPartNos)) as vendorPartNos[];
        }

        public productAvails[] Getproductavail(ISyncWSParameter dataParam, string clientId, string stockNo, string partNo)
        {
            IDataReader reader = null;
            int totalRows;

            Getproductavail(
                clientId,
                stockNo,
                partNo,
                out reader);

            ArrayList list = ReaderHelper.Read(
                reader,
                new productAvails(),
                dataParam.GetSkipNoFirstRows(),
                dataParam.GetMaxRows(),
                out totalRows);

            return list.ToArray(typeof(productAvails)) as productAvails[];
        }

    }
}