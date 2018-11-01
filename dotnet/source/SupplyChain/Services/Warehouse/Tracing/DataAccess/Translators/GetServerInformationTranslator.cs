using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Imi.SupplyChain.Warehouse.Tracing.BusinessEntities;
using Oracle.DataAccess.Client;

namespace Imi.SupplyChain.Warehouse.Tracing.DataAccess.Translators
{
    class GetServerInformationTranslator
    {
        public static IList<IDbDataParameter> TranslateParameters(GetServerInformationParameters parameters)
        {
            return new List<IDbDataParameter>();
        }
        

        
        public static IList<GetServerInformationResult> TranslateResultSet(IDataReader reader)
        {
            IList<GetServerInformationResult> resultList = new List<GetServerInformationResult>();
            object data;

            while (reader.Read())
            {
                GetServerInformationResult result = new GetServerInformationResult();

                data = reader["SERVER_HOST"];

                if (data != DBNull.Value)
                    result.ServerHost = DbTypeConvertor.Convert<string>(data); 

                data = reader["LOGG_PATH"];

                if (data != DBNull.Value)
                    result.DirectoryPath = DbTypeConvertor.Convert<string>(data);
                
                resultList.Add(result);
            }

            return resultList;
        }
    }

}
