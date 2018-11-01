using System;
using System.Collections.Generic;
using System.Text;

using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.Order
{
    public partial class orderSaveResult : ISyncWSResult
    {
        public order[] list;
        public int totalHits;

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

    public partial class customerOrderSearchParameters : ISyncWSParameter
    {
        public bool GetReturnDetails()
        {
            return true;
        }

        public System.Nullable<int> GetSkipNoFirstRows()
        {
            return null;
        }

        public System.Nullable<int> GetMaxRows()
        {
            return null;
        }
    }
}
