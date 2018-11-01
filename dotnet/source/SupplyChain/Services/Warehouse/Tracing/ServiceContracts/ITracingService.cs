using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Imi.SupplyChain.Warehouse.Services.Tracing.DataContracts;

namespace Imi.SupplyChain.Warehouse.Services.Tracing.ServiceContracts
{
    [ServiceContract(Namespace = "http://Imi.SupplyChain.Warehouse.Services.Tracing.ServiceContracts/2011/09")]
    [ServiceApplicationName("Warehouse")]
    public interface ITracingService
    {
        [OperationContract(Action = "EnableDatabaseTracing")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        void EnableDatabaseTracing(EnableDatabaseTracingRequest request);

        [OperationContract(Action = "EnableInterfaceTracing")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        void EnableInterfaceTracing(EnableInterfaceTracingRequest request);

        [OperationContract(Action = "GetInterfaceTracingStatus")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        GetInterfaceTracingResponse GetInterfaceTracingStatus();

        [OperationContract(Action = "GetServerInformation")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        GetServerInformationResponse GetServerInformation();
        
    }
}
