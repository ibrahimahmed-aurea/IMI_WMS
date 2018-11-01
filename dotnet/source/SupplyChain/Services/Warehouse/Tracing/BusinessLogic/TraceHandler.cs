using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading;
using System.Collections.Specialized;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Imi.SupplyChain.Warehouse.Tracing.BusinessEntities;
using Imi.SupplyChain.Warehouse.Tracing.DataAccess;
using Imi.Framework.DataAccess;
using Imi.Framework.Services;

namespace Imi.SupplyChain.Warehouse.Tracing.BusinessLogic
{
    [ConfigurationElementType(typeof(CustomCallHandlerData))]
    public class TraceHandler : ICallHandler
    {
        private const string schemaName = "OWUSER";
        private string connectionString;

        public TraceHandler(NameValueCollection ignore)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            connectionString = settings.ConnectionString;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn msg = null;
            TraceContext context = null;

            if (ApplicationContext.Current != null)
            {
                context = TraceContext.GetContext(
                    ApplicationContext.Current.UserId, 
                    ApplicationContext.Current.TerminalId, 
                    ApplicationContext.Current.SessionId);
            }
                                    
            if (context != null && context.IsTracingEnabled)
            {
                lock (context)
                {
                    using (ITransactionScope scope = new TransactionScope())
                    {
                        string userId = context.UserId;

                        if ((userId.Contains('\\') || userId.Contains('/')))
                        {
                            string[] userIdParts = userId.Split(new char[] { '\\', '/' });
                            userId = userIdParts[1];
                        }

                        ITracingDao tracingDao = new TracingDao(connectionString);

                        StartDatabaseTracingParameters startParameters = new StartDatabaseTracingParameters();
                        startParameters.TerminalId = context.TerminalId;
                        startParameters.UserId = userId;
                        startParameters.WriteHeader = false;

                        tracingDao.StartDatabaseTracing(startParameters);

                        try
                        {
                            msg = getNext()(input, getNext);
                        }
                        finally
                        {
                            StopDatabaseTracingParameters stopParameters = new StopDatabaseTracingParameters();
                            stopParameters.WriteHeader = false;
                            tracingDao.StopDatabaseTracing(stopParameters);
                        }

                        try
                        {
                            scope.Complete();
                        }
                        catch (InvalidOperationException)
                        { 
                        }
                    }
                }
            }
            else
            {
                msg = getNext()(input, getNext);
            }

            return msg;
        }

        private int order;

        public int Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
            }
        }
    }
}
