using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.Framework.Services;
using Business = Cdc.SupplyChain.Services.System.MobileAuthentication.BusinessEntities;
using Service = Cdc.SupplyChain.Services.System.MobileAuthentication.DataContracts;

namespace Cdc.SupplyChain.Services.System.MobileAuthentication.ServiceImplementation.Translators
{
    public class LoginParametersTranslator
    {
        public static DataContracts.LoginParameters TranslateFromBusinessToService(Business.LoginParameters businessItem)
        {
			if(businessItem == null)
			{
				return null;
			}

			using (TranslationScope scope = new TranslationScope())
            {
                if (scope.Cache.Contains(businessItem))
                    return (DataContracts.LoginParameters)scope.Cache.Translate(businessItem);

            	DataContracts.LoginParameters serviceItem = new DataContracts.LoginParameters();
				scope.Cache.Add(businessItem, serviceItem);
				
            	serviceItem.Username = businessItem.Username;
            	serviceItem.Password = businessItem.Password;
            	serviceItem.Domain = businessItem.Domain;
				return serviceItem;
			}
		}

        public static Business.LoginParameters TranslateFromServiceToBusiness(DataContracts.LoginParameters serviceItem)
        {
			if(serviceItem == null)
			{
				return null;
			}
	
			using (TranslationScope scope = new TranslationScope())
            {
                if (scope.Cache.Contains(serviceItem))
                    return (Business.LoginParameters)scope.Cache.Translate(serviceItem);

            	Business.LoginParameters businessItem = new Business.LoginParameters();
				scope.Cache.Add(serviceItem, businessItem);
            	businessItem.Username = serviceItem.Username;
            	businessItem.Password = serviceItem.Password;
            	businessItem.Domain = serviceItem.Domain;
				return businessItem;
			}	
		}
	}
}
