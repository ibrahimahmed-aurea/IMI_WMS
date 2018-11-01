using System;
using System.Collections.Generic;
using System.Text;

using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.ProductSearch
{
    public partial class baseParameters : ISyncWSParameter
    {
        public bool GetReturnDetails()
        {
            return returnDetails;
        }

        public Nullable<int> GetSkipNoFirstRows()
        {
            return firstResult;
        }

        public Nullable<int> GetMaxRows()
        {
            return maxResult;
        }
    }

    public partial class searchResult : ISyncWSResult
    {
        public void SetTotalRows(Nullable<int> totalRows)
        {
            if (totalRows != null)
            {
                totalHits = (int)totalRows;
            }
            else
            {
                totalHits = -1;
            }
        }

        public Nullable<int> GetTotalRows()
        {
            return totalHits;
        }
    }
}
