using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Imi.SupplyChain.OutputManager.Services.Initialization.ServiceContracts;

namespace Imi.SupplyChain.OutputManager.Services.Initialization.ServiceImplementation
{
    [ServiceApplicationName("OutputManager")]
    [ExceptionShielding("DefaultShieldingPolicy")]
    public class InitializationService : IInitializationService
    {
        public FindOutputManagersResponse FindOutputManagers(FindOutputManagersRequest request)
        {
            InitializationServiceAdapter adapter = PolicyInjection.Create<InitializationServiceAdapter>();
            return adapter.FindOutputManagers(request);
        }

        

    }
}
