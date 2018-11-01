﻿using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Services;
using Service = Imi.SupplyChain.Warehouse.Services.Tracing.DataContracts;
using Business = Imi.SupplyChain.Warehouse.Tracing.BusinessEntities;

namespace Imi.SupplyChain.Warehouse.Services.Tracing.ServiceImplementation.Translators
{
    public static class EnableInterfaceTracingTranslator
    {
        public static Business.EnableInterfaceTracingParameters TranslateFromServiceToBusiness(Service.EnableInterfaceTracingParameters enableTracingParams)
        {
            return GenericMapper.MapNew<Business.EnableInterfaceTracingParameters>(enableTracingParams);
        }

    }
}
