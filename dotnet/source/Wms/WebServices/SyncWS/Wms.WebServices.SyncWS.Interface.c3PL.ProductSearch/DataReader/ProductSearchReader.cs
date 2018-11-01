using System;
using System.Data;
using System.Collections;
using Imi.Framework.Job.Data;

using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.ProductSearch
{
    public partial class product : IDataReadble
    {
        IDataReadble IDataReadble.Read(IDataReader reader)
        {
            product x = new product();

            int i = 0;

            x.partNo = ReaderHelper.GetString(reader, i++);
            x.partDescr1 = ReaderHelper.GetString(reader, i++);
            x.partDescr2 = ReaderHelper.GetString(reader, i++);
            x.unit = ReaderHelper.GetString(reader, i++);
            x.unitDescr = ReaderHelper.GetString(reader, i++);
            x.numbDec = ReaderHelper.GetInt32NotNull(reader, i++);
            x.vendPartNo = ReaderHelper.GetString(reader, i++);

            return x;
        }

        IDataReadble IDataReadble.MakeTestData()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public partial class barcode : IDataReadble
    {
        /*
        type BarcodeRow_Type is record
        ( type              EAN.BARCODETYPE%type
        ,value             EAN.EANDUN%type );
         */
        IDataReadble IDataReadble.Read(IDataReader reader)
        {
            barcode x = new barcode();

            int i = 0;

            x.type = ReaderHelper.GetString(reader, i++);
            x.value = ReaderHelper.GetString(reader, i++);

            return x;
        }

        IDataReadble IDataReadble.MakeTestData()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public partial class vendorPartNos : IDataReadble
    {
        public string vendorPartNo;

        IDataReadble IDataReadble.Read(IDataReader reader)
        {
            vendorPartNos x = new vendorPartNos();

            int i = 0;

            x.vendorPartNo = ReaderHelper.GetString(reader, i++);

            return x;
        }

        IDataReadble IDataReadble.MakeTestData()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public partial class productAvails : IDataReadble
    {
        public Nullable<int> productAvail;

        IDataReadble IDataReadble.Read(IDataReader reader)
        {
            productAvails x = new productAvails();

            int i = 0;

            x.productAvail = ReaderHelper.GetInt32(reader, i++);

            return x;
        }

        IDataReadble IDataReadble.MakeTestData()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

}
