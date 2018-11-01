using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.Wms.WebServices.SyncWS.Framework
{
    public interface ISyncWSParameter
    {
        bool GetReturnDetails();
        System.Nullable<int> GetSkipNoFirstRows();
        System.Nullable<int> GetMaxRows();
    }
}
