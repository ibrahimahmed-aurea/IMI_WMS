using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;

namespace Cdc.Framework.Reporting.PLSQL
{
    public enum StoredProcedureDataType
    {
        NotSupported,
        String,
        Numeric,
        DateTime
    }

    public class StoredProcedureParameter
    {
        public string Name { get; set; }
        public StoredProcedureDataType DataType { get; set; }
        public OracleDbType? OracleDbType { get; set; }
        public int Length { get; set; }
        public object Value { get; set; }
        public bool IsNull { get; set; }
    }
}
