using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Cdc.Framework.Services;
using Cdc.SupplyChain.Services.System.MobileAuthentication.ServiceContracts;

namespace Cdc.SupplyChain.Services.System.MobileAuthentication.ServiceImplementation
{
    [ExceptionShielding("DefaultShieldingPolicy")]
	[ServiceApplicationName("System")]
    public class MobileAuthenticationService : IMobileAuthenticationService
    {
        public LoginResponse Login(LoginRequest request)
        {
            MobileAuthenticationServiceAdapter adapter = PolicyInjection.Create<MobileAuthenticationServiceAdapter>();
            return adapter.Login(request);
        }
    }
}
