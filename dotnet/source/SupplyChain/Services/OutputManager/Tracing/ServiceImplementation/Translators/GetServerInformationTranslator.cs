using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Services;
using Service = Imi.SupplyChain.OutputManager.Services.Tracing.DataContracts;
using Business = Imi.SupplyChain.OutputManager.Tracing.BusinessEntities;

namespace Imi.SupplyChain.OutputManager.Services.Tracing.ServiceImplementation.Translators
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
