using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Services;
using Service = Imi.SupplyChain.Transportation.Services.Tracing.DataContracts;
using Business = Imi.SupplyChain.Transportation.Tracing.BusinessEntities;

namespace Imi.SupplyChain.Transportation.Services.Tracing.ServiceImplementation.Translators
{
    class GetServerInformationTranslator
    {
        public static Service.GetServerInformationResult TranslateFromBusinessToService(Business.GetServerInformationResult businessEntity)
        {
            Service.GetServerInformationResult result = new Service.GetServerInformationResult();

            result.ServerHost = businessEntity.ServerHost;
            result.DirectoryPath = businessEntity.DirectoryPath;

            return result;
        }
    }
}
