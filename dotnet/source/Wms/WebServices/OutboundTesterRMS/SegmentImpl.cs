using System;
using System.Data;
using System.Data.Common;
using Imi.Framework.Job.Data;

namespace Imi.Wms.WebServices.OutboundTesterRMS
{
    public class SegmentImpl
    {
        protected IDbCommand fStmt;

        public SegmentImpl(WSBase owner)
        {
            fStmt = owner.GetDataBase().GetConnection().CreateCommand();
            fStmt.CommandType = System.Data.CommandType.Text;
        }

        public IDbDataParameter StringParam(System.String name, int length)
        {
            IDbDataParameter p = fStmt.CreateParameter();
            p.ParameterName = name;
            p.DbType = DbType.String;
            p.Size = length;
            return (p);
        }


        public IDbDataParameter NumberParam(System.String name, byte precision, byte scale)
        {
            IDbDataParameter p = fStmt.CreateParameter();
            p.ParameterName = name;
            p.DbType = DbType.Decimal;
            p.Size = 22;
            p.Precision = precision;
            p.Scale = scale;
            return (p);
        }

        public IDbDataParameter DateParam(System.String name)
        {
            IDbDataParameter p = fStmt.CreateParameter();
            p.ParameterName = name;
            p.DbType = DbType.DateTime;
            return (p);
        }

        public System.String StringValue(System.String value)
        {
            return (value);
        }

        public System.Decimal NumberValue(System.Double value)
        {
            return (new Decimal(value));
        }

        public System.DateTime DateValue(System.DateTime value)
        {
            return (value);
        }
    }
}
