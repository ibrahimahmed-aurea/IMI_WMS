using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Business = Imi.SupplyChain.Settings.BusinessEntities;
using Service = Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators
{
    public class DeleteContainerResultTranslator
    {
        public static DataContracts.DeleteContainerResult TranslateFromBusinessToService(Business.DeleteContainerResult businessItem)
        {
            if (businessItem == null)
            {
                return null;
            }

            DataContracts.DeleteContainerResult serviceItem = new DataContracts.DeleteContainerResult();

            return serviceItem;
        }

        public static Business.DeleteContainerResult TranslateFromServiceToBusiness(DataContracts.DeleteContainerResult serviceItem)
        {
            if (serviceItem == null)
            {
                return null;
            }
            
            Business.DeleteContainerResult businessItem = new Business.DeleteContainerResult();

            return businessItem;
        }
	}
}
