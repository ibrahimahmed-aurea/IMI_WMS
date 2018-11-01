using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;

namespace Imi.SupplyChain.OM.Services.OutputHandler.ServiceContracts
{
    [ServiceContract(Namespace = "http://Imi.SupplyChain.OM.Services.OutputHandler.ServiceContracts/2016/11")]
    [ServiceApplicationName("OutputManager")]
    public interface IOutputHandlerService
    {
        [OperationContract(Action = "CreateOutput")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        CreateOutputResponse CreateOutput(CreateOutputRequest request);

        [OperationContract(Action = "FindPrinterInfo")]
        [FaultContract(typeof(SystemFault))]
        [FaultContract(typeof(ApplicationFault))]
        FindPrinterInfoResponse FindPrinterInfo(FindPrinterInfoRequest request);
    }
}
