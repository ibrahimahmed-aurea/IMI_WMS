using System;
using System.Data;
using System.Collections;
using Imi.Framework.Job.Data;

using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.Warehouse
{
    public partial class warehouse : IDataReadble
    {
        /*
            type WarehouseRow_Type is record
            ( stockNo           WH.WHID%type
            ,name               WH.WHNAME%type );
         */
        IDataReadble IDataReadble.Read(IDataReader reader)
        {
            warehouse x = new warehouse();

            int i = 0;

            x.stockNo = ReaderHelper.GetString(reader, i++);
            x.name = ReaderHelper.GetString(reader, i++);

            return x;
        }

        IDataReadble IDataReadble.MakeTestData()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public partial class warehouseSearchParameters : IDataReadble
    {
        #region IReadble Members

        public IDataReadble Read(IDataReader reader)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IDataReadble MakeTestData()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

}
