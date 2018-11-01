using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Services;
using Business = Imi.SupplyChain.Settings.BusinessEntities;
using Service = Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators
{
    public class ContainerTranslator
    {
        public static DataContracts.Container TranslateFromBusinessToService(Business.Container businessItem)
        {
            if (businessItem == null)
            {
                return null;
            }


            DataContracts.Container serviceItem = new DataContracts.Container();
            serviceItem.Name = businessItem.Name;
            serviceItem.LastModified = businessItem.LastModified;
            serviceItem.MetaData = ContainerMetaDataCollectionTranslator.TranslateFromBusinessToService(businessItem.MetaData);

            return serviceItem;
        }

        public static Business.Container TranslateFromServiceToBusiness(DataContracts.Container serviceItem)
        {
            if (serviceItem == null)
            {
                return null;
            }
            
            Business.Container businessItem = new Business.Container();
            businessItem.Name = serviceItem.Name;
            businessItem.LastModified = serviceItem.LastModified;
            businessItem.MetaData = ContainerMetaDataCollectionTranslator.TranslateFromServiceToBusiness(serviceItem.MetaData);

            return businessItem;
        }
	}
}
