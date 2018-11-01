using System;
using System.Data;
using System.Collections;
using Imi.Framework.Job.Data;

using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.Customer
{
    public partial class address : IDataReadble
    {
        IDataReadble IDataReadble.Read(IDataReader reader)
        {
            address x = new address();

            int i = 0;

            x.name1 = ReaderHelper.GetString(reader, i++);
            x.name2 = ReaderHelper.GetString(reader, i++);
            x.street1 = ReaderHelper.GetString(reader, i++);
            x.street2 = ReaderHelper.GetString(reader, i++);
            x.street3 = ReaderHelper.GetString(reader, i++);
            x.zipCode = ReaderHelper.GetString(reader, i++);
            x.city = ReaderHelper.GetString(reader, i++);
            x.stateCd = ReaderHelper.GetString(reader, i++);
            x.countryCd = ReaderHelper.GetString(reader, i++);

            return x;
        }

        IDataReadble IDataReadble.MakeTestData()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public partial class customer : IDataReadble
    {
        IDataReadble IDataReadble.Read(IDataReader reader)
        {
            customer x = new customer();

            int i = 0;

            x.id = ReaderHelper.GetString(reader, i++);
            x.name = ReaderHelper.GetString(reader, i++);
            x.city = ReaderHelper.GetString(reader, i++);
            x.organizationNumber = ReaderHelper.GetString(reader, i++);
            x.phone = ReaderHelper.GetString(reader, i++);
            x.fax = ReaderHelper.GetString(reader, i++);
            x.customerRef = ReaderHelper.GetString(reader, i++);

            return x;
        }

        IDataReadble IDataReadble.MakeTestData()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public partial class customerSearchParameters : IDataReadble
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
