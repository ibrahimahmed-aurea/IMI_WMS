using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Services;
using Service = Imi.SupplyChain.Transportation.Services.Tracing.DataContracts;
using Business = Imi.SupplyChain.Transportation.Tracing.BusinessEntities;

namespace Imi.SupplyChain.Transportation.Services.Tracing.ServiceImplementation.Translators
{
    public static class EnableDatabaseTracingTranslator
    {
        public static Business.EnableDatabaseTracingParameters TranslateFromServiceToBusiness(Service.EnableDatabaseTracingParameters enableTracingParams)
        {
            return GenericMapper.MapNew<Business.EnableDatabaseTracingParameters>(enableTracingParams);
        }

    }
}
