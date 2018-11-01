using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Imi.Framework.Services
{
    [ServiceContract]
    public interface IWorkerProcessService
    {
        [OperationContract]
        [FaultContract(typeof(FaultException))]
        string Process(List<string> parameters);

        [OperationContract]
        void Terminate();

        [OperationContract]
        bool IsAlive();
    }
}
