using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.SupplyChain.Warehouse.Authentication.BusinessEntities;

namespace Imi.SupplyChain.Warehouse.Authentication.DataAccess
{
    class FindUserWarehousesTranslator
    {
        public static IList<IDbDataParameter> TranslateParameters(FindUserWarehousesParameters parameters)
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

        public static IList<FindUserWarehousesResult> TranslateResultSet(IDataReader reader)
        {
            IList<FindUserWarehousesResult> resultList = new List<FindUserWarehousesResult>();
            object data;

            while (reader.Read())
            {
                FindUserWarehousesResult result = new FindUserWarehousesResult();

                data = reader["WHID"];

                if (data != DBNull.Value)
                    result.WarehouseIdentity = DbTypeConvertor.Convert<string>(data);

                data = reader["WHNAME"];

                if (data != DBNull.Value)
                    result.WarehouseName = DbTypeConvertor.Convert<string>(data);
                                
                resultList.Add(result);
            }

            return resultList;
        }
		
    }
}
