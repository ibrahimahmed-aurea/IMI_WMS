using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.SupplyChain.Transportation.Authentication.BusinessEntities;

namespace Imi.SupplyChain.Transportation.Authentication.DataAccess
{
    class FindUserNodesTranslator
    {
        public static IList<IDbDataParameter> TranslateParameters(FindUserNodesParameters parameters)
        {
            IList<IDbDataParameter> parameterList = new List<IDbDataParameter>();
            IDbDataParameter dbParameter;
            
            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "EMPID";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.Value = parameters.UserIdentity;
            parameterList.Add(dbParameter);
                                    
            return parameterList;
        }

        public static IList<FindUserNodesResult> TranslateResultSet(IDataReader reader)
        {
            IList<FindUserNodesResult> resultList = new List<FindUserNodesResult>();
            object data;

            while (reader.Read())
            {
                FindUserNodesResult result = new FindUserNodesResult();

                data = reader["NODE_ID"];

                if (data != DBNull.Value)
                    result.NodeIdentity = DbTypeConvertor.Convert<string>(data);

                data = reader["NODE_NAME"];

                if (data != DBNull.Value)
                    result.NodeName = DbTypeConvertor.Convert<string>(data);
                                
                resultList.Add(result);
            }

            return resultList;
        }
		
    }
}
