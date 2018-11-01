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
    class FindUserLogonDetailsTranslator
    {
        public static IList<IDbDataParameter> TranslateParameters(FindUserLogonDetailsParameters parameters)
        {
            IList<IDbDataParameter> parameterList = new List<IDbDataParameter>();
            IDbDataParameter dbParameter;

            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "MAPPED_EMPID";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.Value = parameters.UserPrincipalIdentity;
            parameterList.Add(dbParameter);
            
            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "EMPID";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.Value = parameters.UserIdentity;
            parameterList.Add(dbParameter);
                                    
            return parameterList;
        }

        public static IList<FindUserLogonDetailsResult> TranslateResultSet(IDataReader reader)
        {
            IList<FindUserLogonDetailsResult> resultList = new List<FindUserLogonDetailsResult>();
            object data;

            while (reader.Read())
            {
                FindUserLogonDetailsResult result = new FindUserLogonDetailsResult();

                data = reader["EMPID"];

                if (data != DBNull.Value)
                    result.UserIdentity = DbTypeConvertor.Convert<string>(data);

                data = reader["EMPNAME"];

                if (data != DBNull.Value)
                    result.UserName = DbTypeConvertor.Convert<string>(data);

                data = reader["WHID"];

                if (data != DBNull.Value)
                    result.RecentWarehouseIdentity = DbTypeConvertor.Convert<string>(data);

                data = reader["COMPANY_ID"];

                if (data != DBNull.Value)
                    result.RecentCompanyIdentity = DbTypeConvertor.Convert<string>(data);

                data = reader["LASTLOGONDTM"];

                if (data != DBNull.Value)
                    result.LastLogonTime = DbTypeConvertor.Convert<DateTime>(data);
                                
                resultList.Add(result);
            }

            return resultList;
        }
		
    }
}
