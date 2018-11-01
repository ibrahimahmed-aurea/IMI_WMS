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
    public class EnableDatabaseTracingAction : MarshalByRefObject
    {
        private const string schemaName = "OWUSER";

        public void Execute(EnableDatabaseTracingParameters parameters)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            string connectionString = settings.ConnectionString;

            if (parameters.IsTracingEnabled)
            {
                TraceContext context = TraceContext.CreateContext(
                    ApplicationContext.Current.UserId, 
                    ApplicationContext.Current.TerminalId,
                    ApplicationContext.Current.SessionId);

                context.IsTracingEnabled = true;

                ITracingDao tracingDao = PolicyInjection.Create<TracingDao, ITracingDao>(connectionString);
                StartDatabaseTracingParameters startTracingParameters = new StartDatabaseTracingParameters();

                string userId = ApplicationContext.Current.UserId;

                if ((userId.Contains('\\') || userId.Contains('/')))
                {
                    string[] userIdParts = userId.Split(new char[] { '\\', '/' });
                    userId = userIdParts[1];
                }

                startTracingParameters.UserId = userId;
                startTracingParameters.TerminalId = ApplicationContext.Current.TerminalId;
                startTracingParameters.WriteHeader = true;
                tracingDao.StartDatabaseTracing(startTracingParameters);
            }
            else
            {
                TraceContext context = TraceContext.GetContext(
                    ApplicationContext.Current.UserId, 
                    ApplicationContext.Current.TerminalId, 
                    ApplicationContext.Current.SessionId);

                if (context != null)
                {
                    context.IsTracingEnabled = false;

                    ITracingDao tracingDao = PolicyInjection.Create<TracingDao, ITracingDao>(connectionString);
                    StopDatabaseTracingParameters stopTracingParameters = new StopDatabaseTracingParameters();
                    stopTracingParameters.WriteHeader = true;
                    tracingDao.StopDatabaseTracing(stopTracingParameters);
                }
            }
        }

    }
}
