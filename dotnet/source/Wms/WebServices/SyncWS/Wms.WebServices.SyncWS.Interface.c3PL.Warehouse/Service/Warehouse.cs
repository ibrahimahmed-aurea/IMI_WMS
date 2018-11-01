using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using Imi.Framework.Job.Data;
using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.Warehouse
{
    public class Warehouse
    {
        public ISyncWSResult findByClient(Database database, ISyncWSParameter dataParameter)
        {
            warehouseSearchParameters warehouseSearchParameters = (warehouseSearchParameters)dataParameter;

            WebServices3pl pkg = new WebServices3pl(database);

            warehouseSearchResult res = pkg.Findwarehousebyclient(dataParameter, warehouseSearchParameters.clientId);

            return (searchResult)res;
        }

        public ISyncWSResult find(ISyncWSParameter dataParameter)
        {
            warehouseSearchResult res = new warehouseSearchResult();
            ArrayList list = ReaderHelper.MakeTestData(10, new warehouse());
            res.list = list.ToArray(typeof(warehouse)) as warehouse[];
            res.totalHits = res.list.GetLength(0);
            return (searchResult)res;
        }
    }
}
