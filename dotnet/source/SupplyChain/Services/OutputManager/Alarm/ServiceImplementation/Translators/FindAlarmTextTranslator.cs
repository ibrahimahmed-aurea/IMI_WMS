using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Services;
using Service = Imi.SupplyChain.OutputManager.Services.Alarm.DataContracts;
using Business = Imi.SupplyChain.OutputManager.Alarm.BusinessEntities;

namespace Imi.SupplyChain.OutputManager.Services.Alarm.ServiceImplementation.Translators
{
    public static class FindAlarmTextTranslator
    {
        public static Business.FindAlarmTextParams TranslateFromServiceToBusiness(Service.FindAlarmTextParams findAlarmTextParams)
        {
            return GenericMapper.MapNew<Business.FindAlarmTextParams>(findAlarmTextParams);
        }

        public static Service.FindAlarmTextResult TranslateFromBusinessToService(Business.FindAlarmTextResult businessEntity)
        {
            return GenericMapper.MapNew<Service.FindAlarmTextResult>(businessEntity);
        }

    }
}
