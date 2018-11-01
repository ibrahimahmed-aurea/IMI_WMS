using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Imi.SupplyChain.Warehouse.Services.Authentication.ServiceContracts;

namespace Imi.SupplyChain.Warehouse.Services.Authentication.ServiceContracts
{
    [ServiceContract(Namespace = "http://Imi.SupplyChain.Warehouse.Services.Authentication.ServiceContracts/2011/09")]
    [ServiceApplicationName("Warehouse")]
    public interface IAuthenticationService
    {
        [OperationContract(Action = "FindUserDetails")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        FindUserDetailsResponse FindUserDetails(
            FindUserDetailsRequest request);

        [OperationContract(Action = "ModifyUserDetails")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        void ModifyUserDetails(
            ModifyUserDetailsRequest request);

        [OperationContract(Action = "Logon")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        LogonResponse Logon(
            LogonRequest request);
    }

}
