using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Imi.Framework.DataAccess;
using Imi.SupplyChain.Warehouse.Tracing.DataAccess;
using Imi.SupplyChain.Warehouse.Tracing.BusinessEntities;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;

namespace Imi.SupplyChain.Warehouse.Tracing.BusinessLogic
{
    public class GetServerInformationAction : MarshalByRefObject
    {
        private const string schemaName = "OWUSER";
              

        
        public GetServerInformationResult Execute()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            string connectionString = settings.ConnectionString;

            ITracingDao tracingDao = PolicyInjection.Create<TracingDao, ITracingDao>(connectionString);
            IList<GetServerInformationResult> resultList = null;

            resultList = tracingDao.GetServerInformation(new GetServerInformationParameters());

            GetServerInformationResult result = new GetServerInformationResult();

            if (resultList != null && resultList.Count > 0)
            {
                result.ServerHost = resultList[0].ServerHost;
                result.DirectoryPath = resultList[0].DirectoryPath;
                
            }

            return result;
        }

    }
}
