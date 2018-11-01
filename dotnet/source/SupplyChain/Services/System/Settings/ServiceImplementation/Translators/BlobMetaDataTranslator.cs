using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Business = Imi.SupplyChain.Settings.BusinessEntities;
using Service = Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators
{
    public class BlobMetaDataTranslator
    {
        public static DataContracts.BlobMetaData TranslateFromBusinessToService(Business.BlobMetaData businessItem)
        {
            if (businessItem == null)
            {
                return null;
            }
            
            DataContracts.BlobMetaData serviceItem = new DataContracts.BlobMetaData();
            serviceItem.Name = businessItem.Name;
            serviceItem.Value = businessItem.Value;

            return serviceItem;
        }

        public static Business.BlobMetaData TranslateFromServiceToBusiness(DataContracts.BlobMetaData serviceItem)
        {
            if (serviceItem == null)
            {
                return null;
            }

            Business.BlobMetaData businessItem = new Business.BlobMetaData();
            businessItem.Name = serviceItem.Name;
            businessItem.Value = serviceItem.Value;

            return businessItem;
        }
	}
}
