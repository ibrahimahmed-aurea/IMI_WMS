using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Imi.Framework.Services;
using Imi.SupplyChain.Transportation.Services.Tracing.ServiceContracts;
using Imi.SupplyChain.Transportation.Tracing.BusinessLogic;
using Imi.SupplyChain.Transportation.Tracing.BusinessEntities;

namespace Imi.SupplyChain.Transportation.Services.Tracing.ServiceImplementation
{
    public class TracingServiceAdapter : MarshalByRefObject, ITracingService
    {
        public void EnableDatabaseTracing(EnableDatabaseTracingRequest request)
        {
            EnableDatabaseTracingAction action = PolicyInjection.Create<EnableDatabaseTracingAction>();

            action.Execute(Translators.EnableDatabaseTracingTranslator.TranslateFromServiceToBusiness(request.EnableTracingParams));
        }

        public void EnableInterfaceTracing(EnableInterfaceTracingRequest request)
        {
            EnableInterfaceTracingAction action = PolicyInjection.Create<EnableInterfaceTracingAction>();

            action.Execute(Translators.EnableInterfaceTracingTranslator.TranslateFromServiceToBusiness(request.EnableTracingParams));
        }

        public GetInterfaceTracingResponse GetInterfaceTracingStatus()
        {
            InterfaceTracingStatusAction action = PolicyInjection.Create<InterfaceTracingStatusAction>();

            InterfaceTracingStatusResult businessResult = action.Execute();

            GetInterfaceTracingResponse response = new GetInterfaceTracingResponse();

            if (businessResult != null)
            {
                response.GetInterfaceTracingResult = Translators.GetInterfaceTracingTranslator.TranslateFromBusinessToService(businessResult);
            }

            return response;
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
