using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Business = Imi.SupplyChain.Settings.BusinessEntities;
using Service = Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators
{
    public class ContainerMetaDataTranslator
    {
        public static DataContracts.ContainerMetaData TranslateFromBusinessToService(Business.ContainerMetaData businessItem)
        {
            if (businessItem == null)
            {
                return null;
            }
            
            DataContracts.ContainerMetaData serviceItem = new DataContracts.ContainerMetaData();
            serviceItem.Name = businessItem.Name;
            serviceItem.Value = businessItem.Value;

            return serviceItem;
        }

        public static Business.ContainerMetaData TranslateFromServiceToBusiness(DataContracts.ContainerMetaData serviceItem)
        {
            if (serviceItem == null)
            {
                return null;
            }

            Business.ContainerMetaData businessItem = new Business.ContainerMetaData();
            businessItem.Name = serviceItem.Name;
            businessItem.Value = serviceItem.Value;

            return businessItem;
        }
	}
}
