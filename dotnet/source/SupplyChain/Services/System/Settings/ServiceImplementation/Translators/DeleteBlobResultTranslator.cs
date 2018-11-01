using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Business = Imi.SupplyChain.Settings.BusinessEntities;
using Service = Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators
{
    public class DeleteBlobResultTranslator
    {
        public static DataContracts.DeleteBlobResult TranslateFromBusinessToService(Business.DeleteBlobResult businessItem)
        {
            if (businessItem == null)
            {
                return null;
            }

            DataContracts.DeleteBlobResult serviceItem = new DataContracts.DeleteBlobResult();
            
            return serviceItem;
        }

        public static Business.DeleteBlobResult TranslateFromServiceToBusiness(DataContracts.DeleteBlobResult serviceItem)
        {
            if (serviceItem == null)
            {
                return null;
            }

            Business.DeleteBlobResult businessItem = new Business.DeleteBlobResult();

            return businessItem;
        }
	}
}
