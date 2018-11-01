using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service = Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts;
using Business = Imi.SupplyChain.Warehouse.Authentication.BusinessEntities;
using Imi.Framework.Services;

namespace Imi.SupplyChain.Warehouse.Services.Authentication.ServiceImplementation.Translators
{
    public class LogonTranslator
    {
        public static Business.LogonParameters TranslateFromServiceToBusiness(Service.LogonParameters serviceEntity)
        {
            return GenericMapper.MapNew<Business.LogonParameters>(serviceEntity);
        }

        public static Service.LogonResult TranslateFromBusinessToService(Business.LogonResult businessEntity)
        {
            return GenericMapper.MapNew<Service.LogonResult>(businessEntity);
        }
                
    }
}
