using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using System.Data;
using Oracle.DataAccess.Client;
using Imi.SupplyChain.OutputManager.Initialization.BusinessEntities;

namespace Imi.SupplyChain.OutputManager.Initialization.DataAccess
{
    class FindOutputManagersTranslator
    {

        public static IList<FindOutputManagerResult> TranslateResultSet(IDataReader reader)
        {
            IList<FindOutputManagerResult> resultList = new List<FindOutputManagerResult>();
            object data;

            while (reader.Read())
            {
                FindOutputManagerResult result = new FindOutputManagerResult();

                data = reader["OMID"];

                if (data != DBNull.Value)
                    result.OutputManagerIdentity = DbTypeConvertor.Convert<string>(data);

                data = reader["OM_NAME"];

                if (data != DBNull.Value)
                    result.OutputManagerName = DbTypeConvertor.Convert<string>(data);
                                
                resultList.Add(result);
            }

            return resultList;
        }
		
    }
}
