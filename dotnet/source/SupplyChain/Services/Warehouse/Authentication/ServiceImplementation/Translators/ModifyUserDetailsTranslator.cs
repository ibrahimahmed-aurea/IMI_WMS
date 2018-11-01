using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service = Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts;
using Business = Imi.SupplyChain.Warehouse.Authentication.BusinessEntities;
using Imi.Framework.Services;

namespace Imi.SupplyChain.Warehouse.Services.Authentication.ServiceImplementation.Translators
{
    public class ModifyUserDetailsTranslator
    {
        public static Business.ModifyUserDetailsParameters TranslateFromServiceToBusiness(Service.ModifyUserDetailsParameters serviceEntity)
        {
            return GenericMapper.MapNew<Business.ModifyUserDetailsParameters>(serviceEntity);
        }
    }
}
