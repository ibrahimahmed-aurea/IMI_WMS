using System;
using System.Collections.Generic;
using System.Text;
using Cdc.Framework.Services;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Cdc.SupplyChain.Services.System.MobileSettings.ServiceContracts;

namespace Cdc.SupplyChain.Services.System.MobileSettings.ServiceImplementation
{
    [ExceptionShielding("DefaultShieldingPolicy")]
	[ServiceApplicationName("System")]
    public class MobileSettingsService : IMobileSettingsService
    {
        public GetSystemTimeResponse GetSystemTime()
        {
            MobileSettingsServiceAdapter adapter = PolicyInjection.Create<MobileSettingsServiceAdapter>();
            return adapter.GetSystemTime();
        }
    }
}
