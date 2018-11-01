using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Imi.Wms.WebServices.SyncWS.Framework
{
    public interface IDataReadble
    {
        IDataReadble Read(IDataReader reader);
        IDataReadble MakeTestData();
    }
}
