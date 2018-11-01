using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Imi.SupplyChain.OutputManager.Services.Alarm.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.OutputManager.Services.Alarm.DataContracts/2011/09")]
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
