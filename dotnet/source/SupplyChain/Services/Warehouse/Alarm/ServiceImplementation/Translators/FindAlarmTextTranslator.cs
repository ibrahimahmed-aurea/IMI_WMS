using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Services;
using Service = Imi.SupplyChain.Warehouse.Services.Alarm.DataContracts;
using Business = Imi.SupplyChain.Warehouse.Alarm.BusinessEntities;

namespace Imi.SupplyChain.Warehouse.Services.Alarm.ServiceImplementation.Translators
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
