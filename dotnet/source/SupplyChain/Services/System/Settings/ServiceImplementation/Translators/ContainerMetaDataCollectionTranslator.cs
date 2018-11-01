using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Business = Imi.SupplyChain.Settings.BusinessEntities;
using Service = Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators
{
    public class ContainerMetaDataCollectionTranslator
    {
        public static DataContracts.ContainerMetaDataCollection TranslateFromBusinessToService(IList<Business.ContainerMetaData> businessItems)
        {
            if (businessItems == null)
            {
                return null;
            }

            DataContracts.ContainerMetaDataCollection serviceItems = new DataContracts.ContainerMetaDataCollection();

            try
            {
                foreach (Business.ContainerMetaData businessItem in businessItems)
                {
                    serviceItems.Add(ContainerMetaDataTranslator.TranslateFromBusinessToService(businessItem));
                }
            }
            catch (Exception) // Ignory lazy load errors
            {
            }

            return serviceItems;
        }

        public static IList<Business.ContainerMetaData> TranslateFromServiceToBusiness(DataContracts.ContainerMetaDataCollection serviceItems)
        {
            if (serviceItems == null)
            {
                return null;
            }

            IList<Business.ContainerMetaData> businessItems = new List<Business.ContainerMetaData>();

            foreach (DataContracts.ContainerMetaData serviceItem in serviceItems)
            {
                businessItems.Add(ContainerMetaDataTranslator.TranslateFromServiceToBusiness(serviceItem));
            }

            return businessItems;
        }
	}
}
