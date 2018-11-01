using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Imi.Framework.DataAccess;

namespace Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess
{
    class FindReportTranslator
    {
        public static IDictionary<string, Report> TranslateResultSet(IDataReader reader)
        {
            IDictionary<string, Report> resultList = new Dictionary<string, Report>();
            object data;

            while (reader.Read())
            {
                string reportID = string.Empty;
                data = reader["RPTID"];
                if (data != DBNull.Value) { reportID = DbTypeConvertor.Convert<string>(data); }

                if (!resultList.ContainsKey(reportID))
                {
                    Report result = new Report();

                    result.ReportID = reportID;

                    data = reader["RPTGRPID"];
                    if (data != DBNull.Value) { result.ReportGroupID = DbTypeConvertor.Convert<string>(data); }

                    data = reader["RPT_FILE"];
                    if (data != DBNull.Value) { result.ReportFile = DbTypeConvertor.Convert<byte[]>(data); }

                    data = reader["UPDDTM"];
                    if (data != DBNull.Value) { result.LatestUpdate = DbTypeConvertor.Convert<DateTime>(data); }

                    result.AdapterIDs = new List<string>();

                    resultList.Add(result.ReportID, result);
                }

                data = reader["ADAPTER_ID"];
                if (data != DBNull.Value) { resultList[reportID].AdapterIDs.Add(DbTypeConvertor.Convert<string>(data)); }

                data = reader["ADAPTERUPDDTM"];
                if (data != DBNull.Value)
                {
                    DateTime adpUpd = DbTypeConvertor.Convert<DateTime>(data);
                    if (adpUpd > resultList[reportID].LatestUpdate)
                    {
                        resultList[reportID].LatestUpdate = adpUpd;
                    }
                }

            }

            return resultList;
        }
    }
}
