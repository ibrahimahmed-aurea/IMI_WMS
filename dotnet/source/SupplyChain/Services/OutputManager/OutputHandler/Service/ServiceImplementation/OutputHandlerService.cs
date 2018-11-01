using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using System.ServiceModel;
using Imi.SupplyChain.OM.Services.OutputHandler.ServiceContracts;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;

namespace Imi.SupplyChain.OM.Services.OutputHandler.ServiceImplementation
{
    [ExceptionShielding("DefaultShieldingPolicy")]
    [ServiceApplicationName("OutputManager")]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class OutputHandlerService : IOutputHandlerService
    {
        private OutputHandlerServiceAdapter _adapter;

        public OutputHandlerService()
        {
            _adapter = PolicyInjection.Create<OutputHandlerServiceAdapter>();
        }

        public void PassArgumentsFromServiceHost(ServiceHost serviceHost)
        {
            _adapter.PassArgumentsFromServiceHost(serviceHost);
        }

        public CreateOutputResponse CreateOutput(CreateOutputRequest request)
        {
            return _adapter.CreateOutput(request);
        }

        public FindPrinterInfoResponse FindPrinterInfo(FindPrinterInfoRequest request)
        {
            return _adapter.FindPrinterInfo(request);
        }

    }
}
