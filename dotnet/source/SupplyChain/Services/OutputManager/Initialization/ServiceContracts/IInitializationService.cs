using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Imi.SupplyChain.OutputManager.Services.Initialization.ServiceContracts;

namespace Imi.SupplyChain.OutputManager.Services.Initialization.ServiceContracts
{
    [ServiceContract(Namespace = "http://Imi.SupplyChain.OutputManager.Services.Initialization.ServiceContracts/2011/09")]
    [ServiceApplicationName("OutputManager")]
    public interface IInitializationService
    {
        [OperationContract(Action = "FindOutputManagers")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        FindOutputManagersResponse FindOutputManagers(
            FindOutputManagersRequest request);
        
    }

}
