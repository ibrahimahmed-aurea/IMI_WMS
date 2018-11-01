using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.Framework.DataAccess;

namespace Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess
{
    class UpdateOM_URL_Translator
    {
        public static void TranslateParameters(IDataParameterCollection parameterList, string outputManagerID, bool mainService, string URL)
        {
            IDbDataParameter dbParameter;

            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "OMID_I";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.Value = outputManagerID;
            parameterList.Add(dbParameter);

            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "URL_TYPE_I";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
            dbParameter.Direction = ParameterDirection.Input;
            if (mainService)
            {
                dbParameter.Value = "MAIN";
            }
            else
            {
                dbParameter.Value = "FALLBACK";
            }
            parameterList.Add(dbParameter);

            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "URL_I";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.Value = URL;
            parameterList.Add(dbParameter);
        }
    }
}
