using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Cdc.Framework.Services;
using Cdc.SupplyChain.Services.System.MobileAuthentication.ServiceContracts;
using Cdc.SupplyChain.Services.System.MobileAuthentication.ServiceImplementation.Translators;
using BusinessEntities = Cdc.SupplyChain.Services.System.MobileAuthentication.BusinessEntities;
using BusinessActions = Cdc.SupplyChain.Services.System.MobileAuthentication.BusinessLogic;

namespace Cdc.SupplyChain.Services.System.MobileAuthentication.ServiceImplementation
{
    public class MobileAuthenticationServiceAdapter : MarshalByRefObject, IMobileAuthenticationService
    {
        public LoginResponse Login(LoginRequest request)
        {
            BusinessActions.LoginAction action = PolicyInjection.Create<BusinessActions.LoginAction>();

            BusinessEntities.LoginParameters parameters = LoginParametersTranslator.TranslateFromServiceToBusiness(request.LoginParameters);
            BusinessEntities.LoginResult result = action.Execute(parameters);

            LoginResponse response = new LoginResponse();
            response.LoginResult = LoginResultTranslator.TranslateFromBusinessToService(result);

            return response;
        }
    }
}
