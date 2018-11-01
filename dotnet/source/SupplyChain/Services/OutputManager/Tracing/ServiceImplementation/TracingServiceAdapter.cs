using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Imi.Framework.Services;
using Imi.SupplyChain.OutputManager.Services.Tracing.ServiceContracts;
using Imi.SupplyChain.OutputManager.Tracing.BusinessLogic;
using Imi.SupplyChain.OutputManager.Tracing.BusinessEntities;

namespace Imi.SupplyChain.OutputManager.Services.Tracing.ServiceImplementation
{
    public class TracingServiceAdapter : MarshalByRefObject, ITracingService
    {
        public void EnableDatabaseTracing(EnableDatabaseTracingRequest request)
        {
            EnableDatabaseTracingAction action = PolicyInjection.Create<EnableDatabaseTracingAction>();

            action.Execute(Translators.EnableDatabaseTracingTranslator.TranslateFromServiceToBusiness(request.EnableTracingParams));
        }

        public GetServerInformationResponse GetServerInformation()
        {
            
            GetServerInformationAction action = PolicyInjection.Create<GetServerInformationAction>();
            
            GetServerInformationResult businessResult = action.Execute();

            
            GetServerInformationResponse response = new GetServerInformationResponse();

            if (businessResult != null)
            {
                response.GetServerInformationResult = Translators.GetServerInformationTranslator.TranslateFromBusinessToService(businessResult);
            }

            return response;
        }

        
    }
}
