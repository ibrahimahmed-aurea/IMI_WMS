using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.Framework.DataAccess;

namespace Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess
{
    class LoggErrorTranslator
    {
        public static void TranslateParameters(IDataParameterCollection parameterList, string outputManagerID, string errorMessage)
        {
            IDbDataParameter dbParameter;

            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "OMID_I";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.Value = outputManagerID;
            parameterList.Add(dbParameter);

            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "ERROR_MSG_I";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
            ((OracleParameter)dbParameter).OracleDbType = OracleDbType.Clob;
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.Value = errorMessage;
            parameterList.Add(dbParameter);
        }
    }
}
