using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Business = Imi.SupplyChain.Settings.BusinessEntities;
using Service = Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators
{
    public class DeleteBlobParametersTranslator
    {
        public static DataContracts.DeleteBlobParameters TranslateFromBusinessToService(Business.DeleteBlobParameters businessItem)
        {
            if (businessItem == null)
            {
                return null;
            }

            DataContracts.DeleteBlobParameters serviceItem = new DataContracts.DeleteBlobParameters();
            serviceItem.ContainerName = businessItem.ContainerName;
            serviceItem.BlobName = businessItem.BlobName;

            return serviceItem;
        }

        public static Business.DeleteBlobParameters TranslateFromServiceToBusiness(DataContracts.DeleteBlobParameters serviceItem)
        {
            if (serviceItem == null)
            {
                return null;
            }
            
            Business.DeleteBlobParameters businessItem = new Business.DeleteBlobParameters();

            businessItem.ContainerName = serviceItem.ContainerName;
            businessItem.BlobName = serviceItem.BlobName;

            return businessItem;
        }
	}
}
