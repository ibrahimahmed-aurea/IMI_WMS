using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.IdentityModel.Claims;
using Imi.Framework.Services;
using Imi.SupplyChain.Authorization.BusinessLogic;
using BusinessEntities = Imi.SupplyChain.Authorization.BusinessEntities;
using Imi.SupplyChain.Services.Authorization.DataContracts;
using Imi.SupplyChain.Services.Authorization.ServiceContracts;
using Imi.SupplyChain.Services.Authorization.ServiceImplementation.Translators;

namespace Imi.SupplyChain.Services.Authorization.ServiceImplementation
{
    public class AuthorizationServiceAdapter : MarshalByRefObject, IAuthorizationService
	{
        private CheckAuthorizationAction _checkAuthorizationAction;

        public AuthorizationServiceAdapter()
        {
            _checkAuthorizationAction = PolicyInjection.Create<CheckAuthorizationAction>();
        }

        public CheckAuthorizationResponse CheckAuthorization(CheckAuthorizationRequest request)
        {
            BusinessEntities.CheckAuthorizationParameters p = CheckAuthorizationTranslator.TranslateFromServiceToBusiness(request.CheckAuthorizationParameters);

            IClaimsIdentity claimsIdentity = Thread.CurrentPrincipal.Identity as IClaimsIdentity;
                        
            p.Roles = (from c in claimsIdentity.Claims.FindAll(c => { return c.ClaimType == ClaimTypes.GroupSid; })
                      select c.Value).ToList();

            p.Roles.Add(claimsIdentity.Claims.FindAll(c => { return c.ClaimType == ClaimTypes.Sid; }).First().Value);

            BusinessEntities.CheckAuthorizationResult businessResult = _checkAuthorizationAction.Execute(p);

            CheckAuthorizationResponse response = new CheckAuthorizationResponse();

            response.CheckAuthorizationResult = CheckAuthorizationTranslator.TranslateFromBusinessToService(businessResult);

            return response;
        }
    }
}
