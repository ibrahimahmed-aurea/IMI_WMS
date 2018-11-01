using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.Framework.DataAccess;

namespace Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess
{
    class FindAdapterTranslator
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

        public static IDictionary<string, Adapter> TranslateResultSet(IDataReader reader)
        {
            IDictionary<string, Adapter> resultList = new Dictionary<string, Adapter>();

            object data;

            while (reader.Read())
            {
                Adapter result = null;

                data = reader["ADAPTER_ID"];
                if (data != DBNull.Value)
                {
                    string adapterId = DbTypeConvertor.Convert<string>(data);

                    if (!resultList.ContainsKey(adapterId))
                    {
                        resultList.Add(adapterId, new Adapter());

                        result = resultList[adapterId];

                        result.AdapterID = adapterId;

                        result.AdapterParameters = new Dictionary<string, string>();

                        data = reader["UPDDTMADT"];
                        if (data != DBNull.Value) { result.LatestUpdate = DbTypeConvertor.Convert<DateTime>(data); }
                    }

                    data = reader["PARAM_NAME"];
                    if (data != DBNull.Value)
                    {
                        result = resultList[adapterId];

                        string parameterName = DbTypeConvertor.Convert<string>(data);
                        string parameterValue = string.Empty;

                        data = reader["PARAM_VALUE"];
                        if (data != DBNull.Value)
                        {
                            parameterValue = DbTypeConvertor.Convert<string>(data);
                        }

                        result.AdapterParameters.Add(parameterName, parameterValue);

                        data = reader["UPDDTMCONF"];
                        if (data != DBNull.Value)
                        {
                            DateTime confUpd = DbTypeConvertor.Convert<DateTime>(data);

                            if (confUpd > result.LatestUpdate)
                            {
                                result.LatestUpdate = confUpd;
                            }
                        }
                    }

                }
            }

            return resultList;
        }
    }
}
