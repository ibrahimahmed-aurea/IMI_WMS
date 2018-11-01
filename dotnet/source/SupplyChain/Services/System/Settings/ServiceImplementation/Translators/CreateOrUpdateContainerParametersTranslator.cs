using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Business = Imi.SupplyChain.Settings.BusinessEntities;
using Service = Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators
{
    public class CreateOrUpdateContainerParametersTranslator
    {
        public static DataContracts.CreateOrUpdateContainerParameters TranslateFromBusinessToService(Business.CreateOrUpdateContainerParameters businessItem)
        {
            if (businessItem == null)
            {
                return null;
            }


            DataContracts.CreateOrUpdateContainerParameters serviceItem = new DataContracts.CreateOrUpdateContainerParameters();
            serviceItem.Container = ContainerTranslator.TranslateFromBusinessToService(businessItem.Container);
            
            return serviceItem;
        }

        public static Business.CreateOrUpdateContainerParameters TranslateFromServiceToBusiness(DataContracts.CreateOrUpdateContainerParameters serviceItem)
        {
            if (serviceItem == null)
            {
                return null;
            }

            Business.CreateOrUpdateContainerParameters businessItem = new Business.CreateOrUpdateContainerParameters();
            businessItem.Container = ContainerTranslator.TranslateFromServiceToBusiness(serviceItem.Container);

            return businessItem;
        }
	}
}
