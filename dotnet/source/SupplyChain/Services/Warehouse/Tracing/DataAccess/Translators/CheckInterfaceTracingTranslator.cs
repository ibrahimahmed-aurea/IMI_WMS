using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.SupplyChain.Warehouse.Tracing.BusinessEntities;

namespace Imi.SupplyChain.Warehouse.Tracing.DataAccess
{
    class CheckInterfaceTracingTranslator
    {
        public static IList<IDbDataParameter> TranslateParameters(CheckInterfaceTracingParameters parameters)
        {
            return new List<IDbDataParameter>();
        }

        public static IList<CheckInterfaceTracingResult> TranslateResultSet(IDataReader reader)
        {
            IList<CheckInterfaceTracingResult> resultList = new List<CheckInterfaceTracingResult>();
            object data;

            while (reader.Read())
            {
                CheckInterfaceTracingResult result = new CheckInterfaceTracingResult();

                data = reader["LOGG_ON"];

                if (data != DBNull.Value)
                    result.LoggOn = DbTypeConvertor.ConvertStringToBool(data.ToString()).Value;

                data = reader["LOGG_INTVL"];

                if (data != DBNull.Value)
                    result.LoggInterval = DbTypeConvertor.Convert<int>(data);

                data = reader["LOGG_STARTDTM"];

                if (data != DBNull.Value)
                    result.LoggStarted = DbTypeConvertor.Convert<DateTime>(data);

                resultList.Add(result);
            }

            return resultList;
        }
    }
}
