using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service = Imi.SupplyChain.Transportation.Services.Authentication.DataContracts;
using Business = Imi.SupplyChain.Transportation.Authentication.BusinessEntities;
using Imi.Framework.Services;

namespace Imi.SupplyChain.Transportation.Services.Authentication.ServiceImplementation.Translators
{
    public class ModifyUserDetailsTranslator
    {
        public static Business.ModifyUserDetailsParameters TranslateFromServiceToBusiness(Service.ModifyUserDetailsParameters serviceEntity)
        {
            return GenericMapper.MapNew<Business.ModifyUserDetailsParameters>(serviceEntity);
        }
    }
}
