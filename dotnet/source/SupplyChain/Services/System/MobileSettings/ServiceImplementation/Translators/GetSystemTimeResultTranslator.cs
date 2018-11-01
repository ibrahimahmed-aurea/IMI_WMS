using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business = Cdc.SupplyChain.Services.System.MobileSettings.BusinessEntities;
using Service = Cdc.SupplyChain.Services.System.MobileSettings.DataContracts;

namespace Cdc.SupplyChain.Services.System.MobileSettings.ServiceImplementation.Translators
{
    public class GetSystemTimeResultTranslator
    {
        public static Business.GetSystemTimeResult TranslateFromServiceToBusiness(Service.GetSystemTimeResult serviceItem)
        {
            return new Business.GetSystemTimeResult{SystemTime = serviceItem.SystemTime};
        }

        public static Service.GetSystemTimeResult TranslateFromBusinessToService(Business.GetSystemTimeResult businessItem)
        {
            return new Service.GetSystemTimeResult { SystemTime = businessItem.SystemTime };
        }
    }
}