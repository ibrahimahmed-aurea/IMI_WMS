using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.Framework.Parsing.OracleSQLParsing
{
    public class OracleQueryParameter
    {
        public string Name { get; set; }
        public int Sequence { get; set; }
        public OracleQueryParameterDirection Direction { get; set; }
        public string Text { get; set; }

        public string DbDatatype { get; set; }
        public int? Length { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }

        public string OutTableName { get; set; }
        public string OutColumnName { get; set; }

    }
}
