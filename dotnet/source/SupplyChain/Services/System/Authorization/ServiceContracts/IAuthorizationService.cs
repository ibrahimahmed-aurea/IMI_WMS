using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;

namespace Imi.SupplyChain.Services.Authorization.ServiceContracts
{
    [ServiceContract(Namespace = "http://Imi.SupplyChain.Services.Authorization.ServiceContracts/2011/09")]
    [ServiceApplicationName("System")]
    public interface IAuthorizationService
    {
        [OperationContract(Action = "CheckAuthorization")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        CheckAuthorizationResponse CheckAuthorization(CheckAuthorizationRequest request);
    }
}
