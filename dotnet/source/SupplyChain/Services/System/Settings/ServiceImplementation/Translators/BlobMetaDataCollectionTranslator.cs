using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Business = Imi.SupplyChain.Settings.BusinessEntities;
using Service = Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators
{
    public class BlobMetaDataCollectionTranslator
    {
        public static DataContracts.BlobMetaDataCollection TranslateFromBusinessToService(IList<Business.BlobMetaData> businessItems)
        {
            if (businessItems == null)
            {
                return null;
            }

            DataContracts.BlobMetaDataCollection serviceItems = new DataContracts.BlobMetaDataCollection();

            try
            {
                foreach (Business.BlobMetaData businessItem in businessItems)
                {
                    serviceItems.Add(BlobMetaDataTranslator.TranslateFromBusinessToService(businessItem));
                }
            }
            catch (Exception) // Ignory lazy load errors
            {
            }

            return serviceItems;
        }

        public static IList<Business.BlobMetaData> TranslateFromServiceToBusiness(DataContracts.BlobMetaDataCollection serviceItems)
        {
            if (serviceItems == null)
            {
                return null;
            }

            IList<Business.BlobMetaData> businessItems = new List<Business.BlobMetaData>();

            foreach (DataContracts.BlobMetaData serviceItem in serviceItems)
            {
                businessItems.Add(BlobMetaDataTranslator.TranslateFromServiceToBusiness(serviceItem));
            }

            return businessItems;
        }
	}
}
