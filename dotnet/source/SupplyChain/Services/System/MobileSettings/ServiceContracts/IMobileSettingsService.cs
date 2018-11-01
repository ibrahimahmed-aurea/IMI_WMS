using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Cdc.Framework.Services;

namespace Cdc.SupplyChain.Services.System.MobileSettings.ServiceContracts
{
    [ServiceContract(Namespace = "http://Cdc.SupplyChain.Services.System.MobileSettings.ServiceContracts/2010/02")]
    [ServiceApplicationName("System")]
    public interface IMobileSettingsService
    {
        [OperationContract(Action = "GetSystemTime", ReplyAction = "GetSystemTimeResponse")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        GetSystemTimeResponse GetSystemTime();
    }
}