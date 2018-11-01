using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Business = Imi.SupplyChain.Settings.BusinessEntities;
using Service = Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators
{
    public class CreateOrUpdateBlobResultTranslator
    {
        public static DataContracts.CreateOrUpdateBlobResult TranslateFromBusinessToService(Business.CreateOrUpdateBlobResult businessItem)
        {
            if (businessItem == null)
            {
                return null;
            }

            DataContracts.CreateOrUpdateBlobResult serviceItem = new DataContracts.CreateOrUpdateBlobResult();
            
            return serviceItem;
        }

        public static Business.CreateOrUpdateBlobResult TranslateFromServiceToBusiness(DataContracts.CreateOrUpdateBlobResult serviceItem)
        {
            if (serviceItem == null)
            {
                return null;
            }

            Business.CreateOrUpdateBlobResult businessItem = new Business.CreateOrUpdateBlobResult();

            return businessItem;
        }
	}
}
