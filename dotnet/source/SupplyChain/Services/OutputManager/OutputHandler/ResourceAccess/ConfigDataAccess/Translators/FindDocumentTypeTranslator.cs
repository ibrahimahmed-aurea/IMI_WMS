using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Imi.Framework.DataAccess;

namespace Imi.SupplyChain.OM.OutputHandler.ConfigDataAccess
{
    class FindDocumentTypeTranslator
    {
        public static IList<DocumentType> TranslateResultSet(IDataReader reader)
        {
            IList<DocumentType> resultList = new List<DocumentType>();
            object data;

            while (reader.Read())
            {
                DocumentType result = new DocumentType();

                data = reader["DOCTYPID"];
                if (data != DBNull.Value) { result.DocumentTypeID = DbTypeConvertor.Convert<string>(data); }

                data = reader["DOCSUBTYPID"];
                if (data != DBNull.Value) { result.SubDocmentTypeID = DbTypeConvertor.Convert<string>(data); }

                data = reader["RPTID"];
                if (data != DBNull.Value) { result.ReportID = DbTypeConvertor.Convert<string>(data); }

                data = reader["UPDDTM"];
                if (data != DBNull.Value) { result.LatestUpdate = DbTypeConvertor.Convert<DateTime>(data); }

                resultList.Add(result);
            }

            return resultList;
        }
    }
}
