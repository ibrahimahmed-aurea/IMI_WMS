using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Business = Imi.SupplyChain.Settings.BusinessEntities;
using Service = Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators
{
    public class FindContainerResultTranslator
    {
        public static DataContracts.FindContainerResult TranslateFromBusinessToService(IList<Business.Container> businessItems)
        {
            if (businessItems == null)
            {
                return null;
            }
            
            DataContracts.FindContainerResult serviceItems = new DataContracts.FindContainerResult();

            try
            {
                foreach (Business.Container businessItem in businessItems)
                {
                    serviceItems.Add(ContainerTranslator.TranslateFromBusinessToService(businessItem));
                }
            }
            catch (Exception) // Ignory lazy load errors
            {
            }

            return serviceItems;
        }

        public static IList<Business.Container> TranslateFromServiceToBusiness(DataContracts.FindContainerResult serviceItems)
        {
            if (serviceItems == null)
            {
                return null;
            }

            IList<Business.Container> businessItems = new List<Business.Container>();

            foreach (DataContracts.Container serviceItem in serviceItems)
            {
                businessItems.Add(ContainerTranslator.TranslateFromServiceToBusiness(serviceItem));
            }

            return businessItems;
        }
	}
}
