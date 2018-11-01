using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.IdentityModel.Claims;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Imi.SupplyChain.Warehouse.Services.Authentication.ServiceContracts;
using AuthenticationActions = Imi.SupplyChain.Warehouse.Authentication.BusinessLogic;
using AuthenticationEntities = Imi.SupplyChain.Warehouse.Authentication.BusinessEntities;
using DataContracts = Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts;
using Imi.Framework.Services;

namespace Imi.SupplyChain.Warehouse.Services.Authentication.ServiceImplementation
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

            response.FindUserDetailsResult.UserWarehouses = GenericMapper.MapListNew<DataContracts.UserWarehouseCollection, AuthenticationEntities.FindUserWarehousesResult, DataContracts.UserWarehouse>(
                r.Warehouses, Translators.FindUserDetailsTranslator.TranslateFromBusinessToService);

            response.FindUserDetailsResult.UserCompanies = GenericMapper.MapListNew<DataContracts.UserCompanyCollection, AuthenticationEntities.FindUserCompaniesResult, DataContracts.UserCompany>(
                r.Companies, Translators.FindUserDetailsTranslator.TranslateFromBusinessToService);

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
