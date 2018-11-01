using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Wms.WebServices.SyncWS.Framework
{
    public interface ISyncWSResult
    {
        void SetTotalRows(Nullable<int> totalRows);
        Nullable<int> GetTotalRows();
    }
}
