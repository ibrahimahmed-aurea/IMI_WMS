using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Imi.SupplyChain.Warehouse.RowTracker.BusinessEntities;
using Oracle.DataAccess.Client;

namespace Imi.SupplyChain.Warehouse.RowTracker.DataAccess
{
    class FindRowIdentityTranslator
    {
        public static IList<IDbDataParameter> TranslateParameters(FindRowIdentityParameters parameters)
        {
            IList<IDbDataParameter> parameterList = new List<IDbDataParameter>();
            IDbDataParameter dbParameter;
            
            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "ID_I";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.Value = parameters.Id;
            parameterList.Add(dbParameter);

            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "ROWIDENTITY_I";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
            dbParameter.Direction = ParameterDirection.Output;
            dbParameter.Size = 255;
            parameterList.Add(dbParameter);
                        
            return parameterList;
        }

        public static FindRowIdentityResult TranslateResult(IDataParameterCollection resultParameters)
        {
            FindRowIdentityResult result = new FindRowIdentityResult();
            object data;

            data = ((IDbDataParameter)resultParameters["ROWIDENTITY_I"]).Value;

            if (data != DBNull.Value)
                result.RowIdentity = DbTypeConvertor.Convert<string>(data);
            
            return result;
        }
    }
}
