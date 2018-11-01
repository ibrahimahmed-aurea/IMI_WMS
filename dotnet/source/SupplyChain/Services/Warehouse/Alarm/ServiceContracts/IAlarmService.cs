using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Imi.SupplyChain.Warehouse.Services.Alarm.DataContracts;

namespace Imi.SupplyChain.Warehouse.Services.Alarm.ServiceContracts
{
    [ServiceContract(Namespace = "http://Imi.SupplyChain.Warehouse.Services.Alarm.ServiceContracts/2011/09")]
    [ServiceApplicationName("Warehouse")]
    public interface IAlarmService
    {
        [OperationContract(Action = "FindAlarmText")]
        [FaultContract(typeof(SystemFault))]
        FindAlarmTextResponse FindAlarmText(FindAlarmTextRequest request);
    }
}
