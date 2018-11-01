using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Imi.SupplyChain.OutputManager.Services.Alarm.ServiceContracts;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace Imi.SupplyChain.OutputManager.Services.Alarm.ServiceImplementation
{
    [ExceptionShielding("DefaultShieldingPolicy")]
    [ServiceApplicationName("OutputManager")]
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
