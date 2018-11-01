using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.Framework.DataAccess;
using Imi.SupplyChain.Warehouse.Authentication.BusinessEntities;

namespace Imi.SupplyChain.Warehouse.Authentication.DataAccess
{
    class FindUserCompaniesTranslator
    {
        public static IList<IDbDataParameter> TranslateParameters(FindUserCompaniesParameters parameters)
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

        public static IList<FindUserCompaniesResult> TranslateResultSet(IDataReader reader)
        {
            IList<FindUserCompaniesResult> resultList = new List<FindUserCompaniesResult>();
            object data;

            while (reader.Read())
            {
                FindUserCompaniesResult result = new FindUserCompaniesResult();

                data = reader["COMPANY_ID"];

                if (data != DBNull.Value)
                    result.CompanyIdentity = DbTypeConvertor.Convert<string>(data);

                data = reader["COMPANY_NAME"];

                if (data != DBNull.Value)
                    result.CompanyName = DbTypeConvertor.Convert<string>(data);

                data = reader["HOSTINTFACETYPE"];

                if (data != DBNull.Value)
                {
                    string hostIntFaceType = DbTypeConvertor.Convert<string>(data);
                    result.IsClientInterfaceHAPI = hostIntFaceType.Equals("H");
                    result.IsClientInterfaceWebServices = hostIntFaceType.Equals("W");
                    result.IsClientInterfaceEDI = hostIntFaceType.Equals("E");
                }

                data = reader["WHID"];

                if (data != DBNull.Value)
                    result.WarehouseIdentity = DbTypeConvertor.Convert<string>(data);

                resultList.Add(result);
            }

            return resultList;
        }

    }
}
