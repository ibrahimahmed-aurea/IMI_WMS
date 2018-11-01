using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Imi.SupplyChain.OutputManager.Services.Tracing.ServiceContracts;

namespace Imi.SupplyChain.OutputManager.Services.Tracing.ServiceImplementation
{
    [ExceptionShielding("DefaultShieldingPolicy")]
    [ServiceApplicationName("OutputManager")]
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
         
        public GetServerInformationResponse GetServerInformation()
        {
            TracingServiceAdapter adapter = PolicyInjection.Create<TracingServiceAdapter>();            
            return adapter.GetServerInformation();
        }

       

    }
}
