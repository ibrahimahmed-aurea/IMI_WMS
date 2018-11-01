using System;
using System.Data;
using System.Collections;

using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.Warehouse
{
    public partial class WebServices3pl : IPackage
    {
        public warehouseSearchResult Findwarehousebyclient(ISyncWSParameter dataParam, string clientId)
        {
            IDataReader reader = null;
            int totalRows;
            warehouseSearchResult res = new warehouseSearchResult();

            Findwarehousebyclient(
                clientId,
                out reader);

            ArrayList list = ReaderHelper.Read(
                reader,
                new warehouse(),
                dataParam.GetSkipNoFirstRows(),
                dataParam.GetMaxRows(),
                out totalRows);

            res.list = list.ToArray(typeof(warehouse)) as warehouse[];
            res.SetTotalRows(totalRows);

            return res;

        }
    }
}