using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Business = Imi.SupplyChain.Settings.BusinessEntities;
using Service = Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators
{
    public class FindContainerParametersTranslator
    {
        public static DataContracts.FindContainerParameters TranslateFromBusinessToService(Business.FindContainerParameters businessItem)
        {
            if (businessItem == null)
            {
                return null;
            }

            DataContracts.FindContainerParameters serviceItem = new DataContracts.FindContainerParameters();
            serviceItem.ContainerName = businessItem.ContainerName;

            return serviceItem;
        }

        public static Business.FindContainerParameters TranslateFromServiceToBusiness(DataContracts.FindContainerParameters serviceItem)
        {
            if (serviceItem == null)
            {
                return null;
            }

            Business.FindContainerParameters businessItem = new Business.FindContainerParameters();
            businessItem.ContainerName = serviceItem.ContainerName;

            return businessItem;
        }
	}
}
