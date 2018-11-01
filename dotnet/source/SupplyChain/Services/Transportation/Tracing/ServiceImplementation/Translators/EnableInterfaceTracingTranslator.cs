using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Services;
using Service = Imi.SupplyChain.Transportation.Services.Tracing.DataContracts;
using Business = Imi.SupplyChain.Transportation.Tracing.BusinessEntities;

namespace Imi.SupplyChain.Transportation.Services.Tracing.ServiceImplementation.Translators
{
    public static class EnableInterfaceTracingTranslator
    {
        public static Business.EnableInterfaceTracingParameters TranslateFromServiceToBusiness(Service.EnableInterfaceTracingParameters enableTracingParams)
        {
            return GenericMapper.MapNew<Business.EnableInterfaceTracingParameters>(enableTracingParams);
        }

    }
}
