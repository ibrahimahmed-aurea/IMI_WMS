using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.SupplyChain.Transportation.Alarm.BusinessEntities;

namespace Imi.SupplyChain.Transportation.Alarm.DataAccess
{
    class FindAlarmTextTranslator
    {
        public static IList<IDbDataParameter> TranslateParameters(FindAlarmTextParams parameters)
        {
            IList<IDbDataParameter> parameterList = new List<IDbDataParameter>();
            IDbDataParameter dbParameter;
            
            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "ALMID";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.Value = parameters.AlarmId;
            parameterList.Add(dbParameter);

            dbParameter = new OracleParameter();
            dbParameter.ParameterName = "NLANGCOD";
            dbParameter.DbType = DbTypeConvertor.ConvertToDbType(typeof(string));
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.Value = parameters.LanguageCode;
            parameterList.Add(dbParameter);
                                    
            return parameterList;
        }

        public static IList<FindAlarmTextResult> TranslateResultSet(IDataReader reader)
        {
            IList<FindAlarmTextResult> resultList = new List<FindAlarmTextResult>();
            object data;

            while (reader.Read())
            {
                FindAlarmTextResult result = new FindAlarmTextResult();

                data = reader["ALMTXT"];

                if (data != DBNull.Value)
                    result.AlarmText = DbTypeConvertor.Convert<string>(data);
                                
                resultList.Add(result);
            }

            return resultList;
        }
		
    }
}
