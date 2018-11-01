using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Business = Imi.SupplyChain.Settings.BusinessEntities;
using Service = Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators
{
    public class BlobTranslator
    {
        public static DataContracts.Blob TranslateFromBusinessToService(Business.Blob businessItem)
        {
            if (businessItem == null)
            {
                return null;
            }

            DataContracts.Blob serviceItem = new DataContracts.Blob();
            serviceItem.Name = businessItem.Name;
            serviceItem.LastModified = businessItem.LastModified;
            serviceItem.MetaData = BlobMetaDataCollectionTranslator.TranslateFromBusinessToService(businessItem.MetaData);
            serviceItem.Data = businessItem.Data;

            return serviceItem;
        }

        public static Business.Blob TranslateFromServiceToBusiness(DataContracts.Blob serviceItem)
        {
            if (serviceItem == null)
            {
                return null;
            }

            Business.Blob businessItem = new Business.Blob();
            businessItem.Name = serviceItem.Name;
            businessItem.LastModified = serviceItem.LastModified;
            businessItem.MetaData = BlobMetaDataCollectionTranslator.TranslateFromServiceToBusiness(serviceItem.MetaData);
            businessItem.Data = serviceItem.Data;

            return businessItem;
        }
	}
}
