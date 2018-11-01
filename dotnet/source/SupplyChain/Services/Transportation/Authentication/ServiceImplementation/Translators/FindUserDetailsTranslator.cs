using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service = Imi.SupplyChain.Transportation.Services.Authentication.DataContracts;
using Business = Imi.SupplyChain.Transportation.Authentication.BusinessEntities;
using Imi.Framework.Services;

namespace Imi.SupplyChain.Transportation.Services.Authentication.ServiceImplementation.Translators
{
    public class FindUserDetailsTranslator
    {
        public static Business.FindUserDetailsParameters TranslateFromServiceToBusiness(Service.FindUserDetailsParameters serviceEntity)
        {
            return GenericMapper.MapNew<Business.FindUserDetailsParameters>(serviceEntity);
        }

        public static Service.UserNode TranslateFromBusinessToService(Business.FindUserNodesResult businessEntity)
        {
            return GenericMapper.MapNew<Service.UserNode>(businessEntity);
        }
    }
}
