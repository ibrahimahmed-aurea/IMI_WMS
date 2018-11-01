using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Imi.SupplyChain.Transportation.Services.Authentication.ServiceContracts;

namespace Imi.SupplyChain.Transportation.Services.Authentication.ServiceImplementation
{
    [ServiceApplicationName("Transportation")]
    [ExceptionShielding("DefaultShieldingPolicy")]
    public class AuthenticationService : IAuthenticationService
    {
        public FindUserDetailsResponse FindUserDetails(FindUserDetailsRequest request)
        {
            AuthenticationServiceAdapter adapter = PolicyInjection.Create<AuthenticationServiceAdapter>();
            return adapter.FindUserDetails(request);
        }

        public void ModifyUserDetails(ModifyUserDetailsRequest request)
        {
            AuthenticationServiceAdapter adapter = PolicyInjection.Create<AuthenticationServiceAdapter>();
            adapter.ModifyUserDetails(request);
        }

        public LogonResponse Logon(LogonRequest request)
        {
            AuthenticationServiceAdapter adapter = PolicyInjection.Create<AuthenticationServiceAdapter>();
            return adapter.Logon(request);
        }

    }
}
