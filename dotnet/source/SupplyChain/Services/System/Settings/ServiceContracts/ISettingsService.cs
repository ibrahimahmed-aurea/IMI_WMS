// Generated from template: .\ServiceContracts\ServiceContractInterfaceTemplate.cst
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;

namespace Imi.SupplyChain.Services.Settings.ServiceContracts
{
    [ServiceContract(Namespace = "http://Imi.SupplyChain.Services.Settings.ServiceContracts/2011/09")]
	[ServiceApplicationName("System")]
    public interface ISettingsService
    {
	
        [OperationContract(Action = "DeleteBlob", ReplyAction="DeleteBlobResponse")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        DeleteBlobResponse DeleteBlob(
            DeleteBlobRequest request);
        [OperationContract(Action = "CreateOrUpdateBlob", ReplyAction="CreateOrUpdateBlobResponse")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        CreateOrUpdateBlobResponse CreateOrUpdateBlob(
            CreateOrUpdateBlobRequest request);
        [OperationContract(Action = "FindContainer", ReplyAction="FindContainerResponse")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        FindContainerResponse FindContainer(
            FindContainerRequest request);
        [OperationContract(Action = "FindBlob", ReplyAction="FindBlobResponse")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        FindBlobResponse FindBlob(
            FindBlobRequest request);
        [OperationContract(Action = "CreateOrUpdateContainer", ReplyAction="CreateOrUpdateContainerResponse")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        CreateOrUpdateContainerResponse CreateOrUpdateContainer(
            CreateOrUpdateContainerRequest request);
        [OperationContract(Action = "DeleteContainer", ReplyAction="DeleteContainerResponse")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        DeleteContainerResponse DeleteContainer(
            DeleteContainerRequest request);
    }
}
