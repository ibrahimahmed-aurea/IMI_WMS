using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Imi.SupplyChain.Transportation.Services.Alarm.DataContracts;

namespace Imi.SupplyChain.Transportation.Services.Alarm.ServiceContracts
{
    [MessageContract(WrapperName = "FindAlarmTextRequest", WrapperNamespace = "http://Imi.SupplyChain.Transportation.Services.Alarm.ServiceContracts/2011/09")]
    public class FindAlarmTextRequest
    {
        private FindAlarmTextParams findAlarmTextParams;

        [MessageBodyMember(Order = 0)]
        public FindAlarmTextParams FindAlarmTextParams
        {
            get { return findAlarmTextParams; }
            set { findAlarmTextParams = value; }
        }

    }
}
