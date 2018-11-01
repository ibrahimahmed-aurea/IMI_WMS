using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.Framework.DataAccess;

namespace Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess
{
    class FindPrinterTranslator
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

        public static IDictionary<string, Printer> TranslateResultSet(IDataReader reader)
        {
            IDictionary<string, Printer> resultList = new Dictionary<string, Printer>();
            object data;

            while (reader.Read())
            {
                Printer result = new Printer();

                data = reader["PRTID"];
                if (data != DBNull.Value) { result.PrinterID = DbTypeConvertor.Convert<string>(data); }

                data = reader["PRT_DEVICE"];
                if (data != DBNull.Value) { result.PrinterDevice = DbTypeConvertor.Convert<string>(data); }

                data = reader["PRT_TYPE"];
                if (data != DBNull.Value) { result.PrinterType = DbTypeConvertor.Convert<string>(data); }

                data = reader["UPDDTM"];
                if (data != DBNull.Value) { result.LatestUpdate = DbTypeConvertor.Convert<DateTime>(data); }

                if (!resultList.ContainsKey(result.PrinterID))
                {
                    resultList.Add(result.PrinterID, result);
                }
            }

            return resultList;
        }
    }
}
