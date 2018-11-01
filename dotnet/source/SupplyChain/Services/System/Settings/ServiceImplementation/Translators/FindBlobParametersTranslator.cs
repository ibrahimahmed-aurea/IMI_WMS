using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Business = Imi.SupplyChain.Settings.BusinessEntities;
using Service = Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators
{
    public class FindBlobParametersTranslator
    {
        public static DataContracts.FindBlobParameters TranslateFromBusinessToService(Business.FindBlobParameters businessItem)
        {
            if (businessItem == null)
            {
                return null;
            }

            DataContracts.FindBlobParameters serviceItem = new DataContracts.FindBlobParameters();
            serviceItem.ContainerName = businessItem.ContainerName;
            serviceItem.BlobName = businessItem.BlobName;

            return serviceItem;
        }

        public static Business.FindBlobParameters TranslateFromServiceToBusiness(DataContracts.FindBlobParameters serviceItem)
        {
            if (serviceItem == null)
            {
                return null;
            }

            Business.FindBlobParameters businessItem = new Business.FindBlobParameters();
            businessItem.ContainerName = serviceItem.ContainerName;
            businessItem.BlobName = serviceItem.BlobName;

            return businessItem;
        }
	}
}
