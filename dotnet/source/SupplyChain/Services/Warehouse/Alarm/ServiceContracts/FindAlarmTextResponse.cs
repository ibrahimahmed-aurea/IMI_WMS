using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using Imi.SupplyChain.Warehouse.Services.Alarm.DataContracts;

namespace Imi.SupplyChain.Warehouse.Services.Alarm.ServiceContracts
{
    [MessageContract(WrapperName = "FindAlarmTextResponse", WrapperNamespace = "http://Imi.SupplyChain.Warehouse.Services.Alarm.ServiceContracts/2011/09")]
    public class FindAlarmTextResponse
    {
        private FindAlarmTextResult findAlarmTextResult;

        [MessageBodyMember(Order = 0)]
        public FindAlarmTextResult FindAlarmTextResult
        {
            get { return findAlarmTextResult; }
            set { findAlarmTextResult = value; }
        }

    }
}
