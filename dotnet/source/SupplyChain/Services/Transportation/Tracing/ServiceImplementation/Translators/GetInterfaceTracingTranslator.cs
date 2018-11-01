using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Services;
using Service = Imi.SupplyChain.Transportation.Services.Tracing.DataContracts;
using Business = Imi.SupplyChain.Transportation.Tracing.BusinessEntities;


namespace Imi.SupplyChain.Transportation.Services.Tracing.ServiceImplementation.Translators
{
    class GetInterfaceTracingTranslator
    {
        public static Service.GetInterfaceTracingResult TranslateFromBusinessToService(Business.InterfaceTracingStatusResult businessEntity)
        {
            Service.GetInterfaceTracingResult result = new Service.GetInterfaceTracingResult();

            result.LoggIsEnabled = businessEntity.LoggIsOn;
            result.LoggStopTime = businessEntity.LoggStops;

            return result;
        }
    }
}
