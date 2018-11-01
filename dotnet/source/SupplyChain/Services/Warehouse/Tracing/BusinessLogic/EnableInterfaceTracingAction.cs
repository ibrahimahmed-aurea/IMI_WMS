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
    public class EnableInterfaceTracingAction : MarshalByRefObject
    {
        private const string schemaName = "OWUSER";

        public void Execute(EnableInterfaceTracingParameters parameters)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            string connectionString = settings.ConnectionString;

            ITracingDao tracingDao = PolicyInjection.Create<TracingDao, ITracingDao>(connectionString);
            ModifyInterfaceTracingParameters startTracingParameters = new ModifyInterfaceTracingParameters();

            startTracingParameters.LoggOn = parameters.IsTracingEnabled;
            startTracingParameters.LoggInterval = parameters.DurationInSeconds;
            tracingDao.ModifyInterfaceTracing(startTracingParameters);

        }

    }
}
