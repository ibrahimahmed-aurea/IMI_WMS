using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Imi.SupplyChain.Warehouse.Services.Alarm.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.Warehouse.Services.Alarm.DataContracts/2011/09")]
    public class FindAlarmTextParams
    {
        private string alarmId;

        [DataMember(Order = 0)]
        public string AlarmId
        {
            get { return alarmId; }
            set { alarmId = value; }
        }

        private string languageCode;
        
        [DataMember(Order = 1)]
        public string LanguageCode
        {
            get { return languageCode; }
            set { languageCode = value; }
        }
    }
}
