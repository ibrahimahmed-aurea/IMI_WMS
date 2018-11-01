using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Imi.SupplyChain.OutputManager.Services.Tracing.DataContracts;

namespace Imi.SupplyChain.OutputManager.Services.Tracing.ServiceContracts
{
    [ServiceContract(Namespace = "http://Imi.SupplyChain.OutputManager.Services.Tracing.ServiceContracts/2011/09")]
    [ServiceApplicationName("OutputManager")]
    public interface ITracingService
    {
        [OperationContract(Action = "EnableDatabaseTracing")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        void EnableDatabaseTracing(EnableDatabaseTracingRequest request);
                
        [OperationContract(Action = "GetServerInformation")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        GetServerInformationResponse GetServerInformation();
                
    }
}
