using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Business = Imi.SupplyChain.Settings.BusinessEntities;
using Service = Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators
{
    public class CreateOrUpdateBlobParametersTranslator
    {
        public static DataContracts.CreateOrUpdateBlobParameters TranslateFromBusinessToService(Business.CreateOrUpdateBlobParameters businessItem)
        {
            if (businessItem == null)
            {
                return null;
            }

            DataContracts.CreateOrUpdateBlobParameters serviceItem = new DataContracts.CreateOrUpdateBlobParameters();
            serviceItem.ContainerName = businessItem.ContainerName;
            serviceItem.Blobs = BlobCollectionTranslator.TranslateFromBusinessToService(businessItem.Blobs);

            return serviceItem;
        }

        public static Business.CreateOrUpdateBlobParameters TranslateFromServiceToBusiness(DataContracts.CreateOrUpdateBlobParameters serviceItem)
        {
            if (serviceItem == null)
            {
                return null;
            }

            Business.CreateOrUpdateBlobParameters businessItem = new Business.CreateOrUpdateBlobParameters();
            businessItem.ContainerName = serviceItem.ContainerName;
            businessItem.Blobs = BlobCollectionTranslator.TranslateFromServiceToBusiness(serviceItem.Blobs);

            return businessItem;
        }
	}
}
