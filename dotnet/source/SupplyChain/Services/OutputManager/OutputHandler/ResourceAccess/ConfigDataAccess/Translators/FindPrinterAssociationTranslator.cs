using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.Framework.DataAccess;

namespace Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess
{
    class FindPrinterAssociationTranslator
    {
        public static void TranslateParameters(IDataParameterCollection parameterList, string outputManagerID)
        {
            IDbDataParameter dbParameter;

            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "OMID";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.Value = outputManagerID;
            parameterList.Add(dbParameter);
        }

        public static IList<PrinterAssociation> TranslateResultSet(IDataReader reader)
        {
            IList<PrinterAssociation> resultList = new List<PrinterAssociation>();
            object data;

            while (reader.Read())
            {
                PrinterAssociation result = new PrinterAssociation();

                data = reader["PRTID"];
                if (data != DBNull.Value) { result.PrinterID = DbTypeConvertor.Convert<string>(data); }

                data = reader["TERGRPID"];
                if (data != DBNull.Value) { result.TerminalGroupID = DbTypeConvertor.Convert<string>(data); }

                data = reader["RPTGRPID"];
                if (data != DBNull.Value) { result.ReportGroupID = DbTypeConvertor.Convert<string>(data); }

                data = reader["UPDDTM"];
                if (data != DBNull.Value) { result.LatestUpdate = DbTypeConvertor.Convert<DateTime>(data); }

                resultList.Add(result);
            }

            return resultList;
        }
    }
}
