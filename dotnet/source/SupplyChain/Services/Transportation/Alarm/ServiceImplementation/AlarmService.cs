using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Imi.SupplyChain.Transportation.Services.Alarm.ServiceContracts;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace Imi.SupplyChain.Transportation.Services.Alarm.ServiceImplementation
{
    [ServiceApplicationName("Transportation")]
    [ExceptionShielding("DefaultServicePolicy")]
    public class AlarmService : IAlarmService
    {
        public AlarmService()
            : base()
        {

        }

        public FindAlarmTextResponse FindAlarmText(FindAlarmTextRequest request)
        {
            AlarmServiceAdapter adapter = PolicyInjection.Create<AlarmServiceAdapter>();
            return adapter.FindAlarmText(request);
        }
    }
}
