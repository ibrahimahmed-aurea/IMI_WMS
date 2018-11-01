using System;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Cdc.SupplyChain.Services.System.MobileSettings.ServiceContracts;
using Cdc.SupplyChain.Services.System.MobileSettings.BusinessLogic;
using Cdc.SupplyChain.Services.System.MobileSettings.ServiceImplementation.Translators;

namespace Cdc.SupplyChain.Services.System.MobileSettings.ServiceImplementation
{
    public class MobileSettingsServiceAdapter : MarshalByRefObject, IMobileSettingsService
    {
        public GetSystemTimeResponse GetSystemTime()
        {
            GetSystemTimeAction action = PolicyInjection.Create<GetSystemTimeAction>();

            GetSystemTimeResponse response = new GetSystemTimeResponse();

            response.GetSystemTimeResult = GetSystemTimeResultTranslator.TranslateFromBusinessToService(action.Execute());

            return response;
        }

    }
}
