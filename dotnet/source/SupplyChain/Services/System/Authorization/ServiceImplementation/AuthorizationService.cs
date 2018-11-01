using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Imi.SupplyChain.Services.Authorization.ServiceContracts;

namespace Imi.SupplyChain.Services.Authorization.ServiceImplementation
{
    [ExceptionShielding("DefaultShieldingPolicy")]
    [ServiceApplicationName("System")]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class AuthorizationService : IAuthorizationService
    {
        private AuthorizationServiceAdapter adapter;

        public AuthorizationService()
            : base()
        {
            adapter = PolicyInjection.Create<AuthorizationServiceAdapter>();
        }

        public CheckAuthorizationResponse CheckAuthorization(CheckAuthorizationRequest request)
        {
            return adapter.CheckAuthorization(request);
        }
    }
}
