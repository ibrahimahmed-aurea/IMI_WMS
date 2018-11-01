using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Business = Imi.SupplyChain.Settings.BusinessEntities;
using Service = Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators
{
    public class DeleteContainerParametersTranslator
    {
        public static DataContracts.DeleteContainerParameters TranslateFromBusinessToService(Business.DeleteContainerParameters businessItem)
        {
            if (businessItem == null)
            {
                return null;
            }

            DataContracts.DeleteContainerParameters serviceItem = new DataContracts.DeleteContainerParameters();
            serviceItem.ContainerName = businessItem.ContainerName;

            return serviceItem;
        }

        public static Business.DeleteContainerParameters TranslateFromServiceToBusiness(DataContracts.DeleteContainerParameters serviceItem)
        {
            if (serviceItem == null)
            {
                return null;
            }

            Business.DeleteContainerParameters businessItem = new Business.DeleteContainerParameters();
            businessItem.ContainerName = serviceItem.ContainerName;

            return businessItem;
        }
	}
}
