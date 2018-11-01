using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service = Imi.SupplyChain.OutputManager.Services.Initialization.DataContracts;
using Business = Imi.SupplyChain.OutputManager.Initialization.BusinessEntities;
using Imi.Framework.Services;

namespace Imi.SupplyChain.OutputManager.Services.Initialization.ServiceImplementation.Translators
{
    public class FindOutputManagersTranslator
    {
        public static Business.FindOutputManagersParameters TranslateFromServiceToBusiness(Service.FindOutputManagersParameters serviceEntity)
        {
            return GenericMapper.MapNew<Business.FindOutputManagersParameters>(serviceEntity);
        }

        public static Service.OutputManager TranslateFromBusinessToService(Business.FindOutputManagerResult businessEntity)
        {
            return GenericMapper.MapNew<Service.OutputManager>(businessEntity);
        }

        //public static Service.UserWarehouse TranslateFromBusinessToService(Business.FindUserWarehousesResult businessEntity)
        //{
        //    return GenericMapper.MapNew<Service.UserWarehouse>(businessEntity);
        //}
    }
}
