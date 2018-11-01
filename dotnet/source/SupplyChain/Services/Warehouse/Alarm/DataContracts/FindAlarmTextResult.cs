using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Imi.SupplyChain.Warehouse.Services.Alarm.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.Warehouse.Services.Alarm.DataContracts/2011/09")]
    public class FindAlarmTextResult
    {
        private string alarmText;

        [DataMember(Order = 0)]
        public string AlarmText
        {
            get { return alarmText; }
            set { alarmText = value; }
        }
    }
}
