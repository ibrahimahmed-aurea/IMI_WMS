using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Imi.Framework.DataAccess;

namespace Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess
{
    class FindTerminalTranslator
    {
        public static IDictionary<string, Terminal> TranslateResultSet(IDataReader reader)
        {
            IDictionary<string, Terminal> resultList = new Dictionary<string, Terminal>();
            object data;

            while (reader.Read())
            {
                Terminal result = new Terminal();

                data = reader["TERID"];
                if (data != DBNull.Value) { result.TerminalID = DbTypeConvertor.Convert<string>(data); }

                data = reader["TERGRPID"];
                if (data != DBNull.Value) { result.TerminalGroupID = DbTypeConvertor.Convert<string>(data); }

                data = reader["UPDDTM"];
                if (data != DBNull.Value) { result.LatestUpdate = DbTypeConvertor.Convert<DateTime>(data); }

                if (!resultList.ContainsKey(result.TerminalID))
                {
                    resultList.Add(result.TerminalID, result);
                }
            }

            return resultList;
        }
    }
}
