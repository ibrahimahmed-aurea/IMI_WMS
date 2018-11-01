using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service = Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts;
using Business = Imi.SupplyChain.Warehouse.Authentication.BusinessEntities;
using Imi.Framework.Services;

namespace Imi.SupplyChain.Warehouse.Services.Authentication.ServiceImplementation.Translators
{
    public class FindUserDetailsTranslator
    {
        public static Business.FindUserDetailsParameters TranslateFromServiceToBusiness(Service.FindUserDetailsParameters serviceEntity)
        {
            return GenericMapper.MapNew<Business.FindUserDetailsParameters>(serviceEntity);
        }

        public static Service.UserCompany TranslateFromBusinessToService(Business.FindUserCompaniesResult businessEntity)
        {
            return GenericMapper.MapNew<Service.UserCompany>(businessEntity);
        }

        public static Service.UserWarehouse TranslateFromBusinessToService(Business.FindUserWarehousesResult businessEntity)
        {
            return GenericMapper.MapNew<Service.UserWarehouse>(businessEntity);
        }
    }
}
