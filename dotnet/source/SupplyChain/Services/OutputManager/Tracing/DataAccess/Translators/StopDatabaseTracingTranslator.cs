using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Imi.SupplyChain.OutputManager.Tracing.BusinessEntities;
using Oracle.DataAccess.Client;

namespace Imi.SupplyChain.OutputManager.Tracing.DataAccess
{
    class StopDatabaseTracingTranslator
    {
        public static IList<IDbDataParameter> TranslateParameters(StopDatabaseTracingParameters parameters)
        {
            IList<IDbDataParameter> parameterList = new List<IDbDataParameter>();
            IDbDataParameter dbParameter;
                        
            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "WriteHeader_I";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
            dbParameter.Direction = ParameterDirection.Input;
            
            if (parameters.WriteHeader)
                dbParameter.Value = "1";
            else
                dbParameter.Value = "0";

            parameterList.Add(dbParameter);
                        
            return parameterList;
        }
    }
}
