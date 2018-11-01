using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Services;
using Service = Imi.SupplyChain.Warehouse.Services.Tracing.DataContracts;
using Business = Imi.SupplyChain.Warehouse.Tracing.BusinessEntities;

namespace Imi.SupplyChain.Warehouse.Services.Tracing.ServiceImplementation.Translators
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
