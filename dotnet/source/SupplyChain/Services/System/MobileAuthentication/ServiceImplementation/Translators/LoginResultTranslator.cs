using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.Framework.Services;
using Business = Cdc.SupplyChain.Services.System.MobileAuthentication.BusinessEntities;
using Service = Cdc.SupplyChain.Services.System.MobileAuthentication.DataContracts;

namespace Cdc.SupplyChain.Services.System.MobileAuthentication.ServiceImplementation.Translators
{
    public class LoginResultTranslator
    {
        public static DataContracts.LoginResult TranslateFromBusinessToService(Business.LoginResult businessItem)
        {
			if(businessItem == null)
			{
				return null;
			}

			using (TranslationScope scope = new TranslationScope())
            {
                if (scope.Cache.Contains(businessItem))
                    return (DataContracts.LoginResult)scope.Cache.Translate(businessItem);

            	DataContracts.LoginResult serviceItem = new DataContracts.LoginResult();
				scope.Cache.Add(businessItem, serviceItem);
				
            	serviceItem.Success = businessItem.Success;
            	serviceItem.ErrorMessage = businessItem.ErrorMessage;
				return serviceItem;
			}
		}

        public static Business.LoginResult TranslateFromServiceToBusiness(DataContracts.LoginResult serviceItem)
        {
			if(serviceItem == null)
			{
				return null;
			}
	
			using (TranslationScope scope = new TranslationScope())
            {
                if (scope.Cache.Contains(serviceItem))
                    return (Business.LoginResult)scope.Cache.Translate(serviceItem);

            	Business.LoginResult businessItem = new Business.LoginResult();
				scope.Cache.Add(serviceItem, businessItem);
            	businessItem.Success = serviceItem.Success;
            	businessItem.ErrorMessage = serviceItem.ErrorMessage;
				return businessItem;
			}	
		}
	}
}
