using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Business = Imi.SupplyChain.Settings.BusinessEntities;
using Service = Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators
{
    public class CreateOrUpdateContainerResultTranslator
    {
        public static DataContracts.CreateOrUpdateContainerResult TranslateFromBusinessToService(Business.CreateOrUpdateContainerResult businessItem)
        {
            if (businessItem == null)
            {
                return null;
            }

            DataContracts.CreateOrUpdateContainerResult serviceItem = new DataContracts.CreateOrUpdateContainerResult();
            
            return serviceItem;
        }

        public static Business.CreateOrUpdateContainerResult TranslateFromServiceToBusiness(DataContracts.CreateOrUpdateContainerResult serviceItem)
        {
            if (serviceItem == null)
            {
                return null;
            }

            Business.CreateOrUpdateContainerResult businessItem = new Business.CreateOrUpdateContainerResult();

            return businessItem;
        }
	}
}
