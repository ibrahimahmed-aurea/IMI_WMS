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
    public class InterfaceTracingStatusAction : MarshalByRefObject
    {
        private const string schemaName = "OWUSER";

        public InterfaceTracingStatusResult Execute()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            string connectionString = settings.ConnectionString;

            ITracingDao tracingDao = PolicyInjection.Create<TracingDao, ITracingDao>(connectionString);
            IList<CheckInterfaceTracingResult> resultList = null;

            resultList = tracingDao.CheckInterfaceTracing(new CheckInterfaceTracingParameters());

            InterfaceTracingStatusResult result = new InterfaceTracingStatusResult();

            if (resultList != null && resultList.Count > 0)
            {
                if (resultList[0].LoggOn)
                {
                    result.LoggIsOn = true;
                    result.LoggStops = resultList[0].LoggStarted.AddSeconds(resultList[0].LoggInterval);
                }
                else
                {
                    result.LoggIsOn = false;
                    result.LoggStops = null;
                }
            }

            return result;
        }
    }
}
