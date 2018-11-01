using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Imi.SupplyChain.Transportation.Services.Tracing.ServiceContracts;

namespace Imi.SupplyChain.Transportation.Services.Tracing.ServiceImplementation
{
    [ExceptionShielding("DefaultShieldingPolicy")]
    [ServiceApplicationName("Transportation")]
    public class TracingService : ITracingService
    {
        public TracingService()
            : base()
        {

        }
                
        public void EnableDatabaseTracing(EnableDatabaseTracingRequest request)
        {
            TracingServiceAdapter adapter = PolicyInjection.Create<TracingServiceAdapter>();
            adapter.EnableDatabaseTracing(request);
        }

        public void EnableInterfaceTracing(EnableInterfaceTracingRequest request)
        {
            TracingServiceAdapter adapter = PolicyInjection.Create<TracingServiceAdapter>();
            adapter.EnableInterfaceTracing(request);
        }

        public GetInterfaceTracingResponse GetInterfaceTracingStatus()
        {
            TracingServiceAdapter adapter = PolicyInjection.Create<TracingServiceAdapter>();
            return adapter.GetInterfaceTracingStatus();
        }

        public GetServerInformationResponse GetServerInformation()
        {
            TracingServiceAdapter adapter = PolicyInjection.Create<TracingServiceAdapter>();
            return adapter.GetServerInformation();
        }

   }
}
