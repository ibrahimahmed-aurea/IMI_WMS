using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.SupplyChain.Warehouse.Alarm.BusinessEntities
{
    public class FindAlarmTextParams
    {
        private string alarmId;

        public string AlarmId
        {
            get { return alarmId; }
            set { alarmId = value; }
        }

        private string languageCode;

        public string LanguageCode
        {
            get { return languageCode; }
            set { languageCode = value; }
        }
    }
}
