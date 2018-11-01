using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Cdc.Framework.Services;

namespace Cdc.SupplyChain.Services.System.MobileAuthentication.ServiceContracts
{
    [ServiceContract(Namespace = "http://Cdc.SupplyChain.Services.System.MobileAuthentication.ServiceContracts/2007/05")]
	[ServiceApplicationName("System")]
    public interface IMobileAuthenticationService
    {
        [OperationContract(Action = "Login", ReplyAction="LoginResponse")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        LoginResponse Login(
            LoginRequest request);
    }
}
