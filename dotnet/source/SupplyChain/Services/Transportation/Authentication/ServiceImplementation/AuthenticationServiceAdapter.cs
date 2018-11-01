using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.IdentityModel.Claims;
using Imi.SupplyChain.Transportation.Services.Authentication.ServiceContracts;
using AuthenticationActions = Imi.SupplyChain.Transportation.Authentication.BusinessLogic;
using AuthenticationEntities = Imi.SupplyChain.Transportation.Authentication.BusinessEntities;
using DataContracts = Imi.SupplyChain.Transportation.Services.Authentication.DataContracts;
using Imi.Framework.Services;


namespace Imi.SupplyChain.Transportation.Services.Authentication.ServiceImplementation
{
    public class AuthenticationServiceAdapter : MarshalByRefObject, IAuthenticationService
    {
        public FindUserDetailsResponse FindUserDetails(FindUserDetailsRequest request)
        {
            IClaimsIdentity identity = (IClaimsIdentity)Thread.CurrentPrincipal.Identity;
            string upn = identity.Claims.FindAll(c => { return c.ClaimType == ClaimTypes.Upn; }).First().Value;

            AuthenticationActions.FindUserDetailsAction action = PolicyInjection.Create<AuthenticationActions.FindUserDetailsAction>();

            AuthenticationEntities.FindUserDetailsParameters parameters = Translators.FindUserDetailsTranslator.TranslateFromServiceToBusiness(request.FindUserDetailsParameters);
            
            parameters.UserPrincipalIdentity = upn; /* user@domain */
            parameters.UserIdentity = upn.Split('@')[0];

            AuthenticationEntities.FindUserDetailsResult r = action.Execute(parameters);

            FindUserDetailsResponse response = new FindUserDetailsResponse();

            response.FindUserDetailsResult = GenericMapper.MapNew<DataContracts.FindUserDetailsResult>(r);

            response.FindUserDetailsResult.UserNodes = GenericMapper.MapListNew<DataContracts.UserNodeCollection, AuthenticationEntities.FindUserNodesResult, DataContracts.UserNode>(
                r.UserNodes, Translators.FindUserDetailsTranslator.TranslateFromBusinessToService);

            return response;
        }

        public void ModifyUserDetails(ModifyUserDetailsRequest request)
        {
            AuthenticationActions.ModifyUserDetailsAction action = PolicyInjection.Create<AuthenticationActions.ModifyUserDetailsAction>();

            action.Execute(
                Translators.ModifyUserDetailsTranslator.TranslateFromServiceToBusiness(request.ModifyUserDetailsParameters));

        }

        public LogonResponse Logon(LogonRequest request)
        {
            AuthenticationActions.LogonAction action = PolicyInjection.Create<AuthenticationActions.LogonAction>();

            AuthenticationEntities.LogonResult resultParams = action.Execute(Translators.LogonTranslator.TranslateFromServiceToBusiness(request.LogonParameters));
            LogonResponse response = new LogonResponse();

            response.LogonResult = Translators.LogonTranslator.TranslateFromBusinessToService(resultParams);

            return response;
        }

    }
}
