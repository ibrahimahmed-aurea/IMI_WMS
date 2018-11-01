using System;
using System.Data;
using System.Collections;
using Imi.Framework.Job.Data;

using Imi.Wms.WebServices.SyncWS.Framework;

namespace Imi.Wms.WebServices.SyncWS.Interface.c3PL.Order
{
    public partial class order : IDataReadble
    {
        internal string shipFromPartyQualifier;

        IDataReadble IDataReadble.Read(IDataReader reader)
        {
            order x = new order();

            int i = 0;

            string ot = ReaderHelper.GetString(reader, i++); // orderType

            if (ot == "CO3PL")
                x.orderType = orderType.CO3PL;
            else if (ot == "PO")
                x.orderType = orderType.PO;
            else
                throw new Exception("Order type <" + ot + "> not supported");
            x.orderTypeSpecified = true;

            x.coNo = ReaderHelper.GetString(reader, i++);
            x.customer = ReaderHelper.GetString(reader, i++);
            x.cuRef = ReaderHelper.GetString(reader, i++);
            x.yourCono = ReaderHelper.GetString(reader, i++);
            x.reqDelDate = ReaderHelper.GetDateTime(reader, i++);
            x.text = ReaderHelper.GetString(reader, i++);
            x.coMark = ReaderHelper.GetString(reader, i++);
            x.poid = ReaderHelper.GetString(reader, i++);
            x.poSeq = ReaderHelper.GetInt32(reader, i++);
            x.receivingWhid = ReaderHelper.GetString(reader, i++);
            x.shipFromPartyid = ReaderHelper.GetString(reader, i++);
            x.shipFromPartyQualifier = ReaderHelper.GetString(reader, i++);
            x.whName = ReaderHelper.GetString(reader, i++);
            x.methodOfShipmentId = ReaderHelper.GetString(reader, i++);
            x.methodOfShipmentName = ReaderHelper.GetString(reader, i++);
            x.customerOrderTypeId = ReaderHelper.GetString(reader, i++);
            x.customerOrderTypeName = ReaderHelper.GetString(reader, i++);

            return x;
        }

        IDataReadble IDataReadble.MakeTestData()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public partial class orderLine : IDataReadble
    {
        IDataReadble IDataReadble.Read(IDataReader reader)
        {
            orderLine x = new orderLine();

            int i = 0;

            x.partNo = ReaderHelper.GetString(reader, i++);
            x.partDescr1 = ReaderHelper.GetString(reader, i++);
            x.partDescr2 = ReaderHelper.GetString(reader, i++);
            x.custPartNo = ReaderHelper.GetString(reader, i++);
            x.custPartDescr1 = ReaderHelper.GetString(reader, i++);
            x.custPartDescr2 = ReaderHelper.GetString(reader, i++);
            x.text = ReaderHelper.GetString(reader, i++);
            x.qtyUnit = ReaderHelper.GetDecimal(reader, i++);
            x.originalQtyUnit = ReaderHelper.GetDecimal(reader, i++);
            x.unit = ReaderHelper.GetString(reader, i++);
            x.unitDescr = ReaderHelper.GetString(reader, i++);
            x.linePos = ReaderHelper.GetInt32(reader, i++);
            x.lineSeq = ReaderHelper.GetInt32(reader, i++);
            x.lineId = ReaderHelper.GetInt32(reader, i++);
            x.sellingUnit = ReaderHelper.GetString(reader, i++);
            x.sellingUnitDesc = ReaderHelper.GetString(reader, i++);
            x.delDate = ReaderHelper.GetDateTime(reader, i++);
            i = GetLineStatus(reader, x, i);
            x.rcvQtyUnit = ReaderHelper.GetDecimal(reader, i++);
            /*
            x.blockCod = ReaderHelper.GetString(reader, i++);
            x.serial = ReaderHelper.GetString(reader, i++);
            x.serNumb = ReaderHelper.GetString(reader, i++);
            x.prodLot = ReaderHelper.GetString(reader, i++);
            x.promotn = ReaderHelper.GetString(reader, i++);
            x.storBat = ReaderHelper.GetString(reader, i++);
            x.prodLotReq = ReaderHelper.GetString(reader, i++);
            */
            return x;
        }

        private int GetLineStatus(IDataReader reader, orderLine x, int i)
        {
            string lineStatus = ReaderHelper.GetString(reader, i++);
            try
            {
                if (lineStatus != null)
                    x.lineStatus = Convert.ToInt32(lineStatus);
                else
                    x.lineStatus = null;
            }
            catch
            {
                x.lineStatus = null;
            }
            return i;
        }

        IDataReadble IDataReadble.MakeTestData()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

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

    public partial class customerOrderType : IDataReadble
    {
        IDataReadble IDataReadble.Read(IDataReader reader)
        {
            customerOrderType x = new customerOrderType();

            int i = 0;

            x.customerOrderTypeId = ReaderHelper.GetString(reader, i++);
            x.customerOrderTypeName = ReaderHelper.GetString(reader, i++);

            return x;
        }

        IDataReadble IDataReadble.MakeTestData()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public partial class methodOfShipment : IDataReadble
    {
        IDataReadble IDataReadble.Read(IDataReader reader)
        {
            methodOfShipment x = new methodOfShipment();

            int i = 0;

            x.methodOfShipmentId = ReaderHelper.GetString(reader, i++);
            x.methodOfShipmentName = ReaderHelper.GetString(reader, i++);

            return x;
        }

        IDataReadble IDataReadble.MakeTestData()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public partial class orderSearchParameters : IDataReadble
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
