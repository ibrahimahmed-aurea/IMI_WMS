using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Imi.SupplyChain.Warehouse.Tracing.BusinessEntities;
using Oracle.DataAccess.Client;

namespace Imi.SupplyChain.Warehouse.Tracing.DataAccess
{
    class ModifyInterfaceTracingTranslator
    {
        public static IList<IDbDataParameter> TranslateParameters(ModifyInterfaceTracingParameters parameters)
        {
            IList<IDbDataParameter> parameterList = new List<IDbDataParameter>();
            IDbDataParameter dbParameter;

            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "LOGG_ON_I";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.Value = parameters.LoggOn ? "1" : "0";
            parameterList.Add(dbParameter);

            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "LOGG_INTVL_I";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(int));
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.Value = parameters.LoggInterval;
            parameterList.Add(dbParameter);

            return parameterList;
        }
    }
}
