using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Business = Imi.SupplyChain.Settings.BusinessEntities;
using Service = Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators
{
    public class FindBlobResultTranslator
    {
        public static DataContracts.FindBlobResult TranslateFromBusinessToService(IList<Business.Blob> businessItems)
        {
            if (businessItems == null)
            {
                return null;
            }

            DataContracts.FindBlobResult serviceItems = new DataContracts.FindBlobResult();

            try
            {
                foreach (Business.Blob businessItem in businessItems)
                {
                    serviceItems.Add(BlobTranslator.TranslateFromBusinessToService(businessItem));
                }
            }
            catch (Exception) // Ignory lazy load errors
            {
            }

            return serviceItems;
        }

        public static IList<Business.Blob> TranslateFromServiceToBusiness(DataContracts.FindBlobResult serviceItems)
        {
            if (serviceItems == null)
            {
                return null;
            }

            IList<Business.Blob> businessItems = new List<Business.Blob>();

            foreach (DataContracts.Blob serviceItem in serviceItems)
            {
                businessItems.Add(BlobTranslator.TranslateFromServiceToBusiness(serviceItem));
            }

            return businessItems;
        }
	}
}
