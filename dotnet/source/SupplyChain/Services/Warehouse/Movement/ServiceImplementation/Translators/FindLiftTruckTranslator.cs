using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service = Cdc.SupplyChain.Warehouse.Services.Movement.DataContracts;
using Business = Cdc.SupplyChain.Warehouse.BusinessLogic.Movement.Entities;
using Cdc.Framework.Services;

namespace Cdc.SupplyChain.Warehouse.Services.Movement.ServiceImplementation.Translators
{
    public class FindLiftTruckTranslator
    {
        public static Business.FindLiftTruckParameters TranslateFromServiceToBusiness(Service.FindLiftTruckParameters serviceEntity)
        {
            return GenericMapper.MapNew< Business.FindLiftTruckParameters>(serviceEntity);
        }


        public static Service.FindLiftTruckResult TranslateFromBusinessToService(Business.FindLiftTruckResult businessEntity)
        {
            return GenericMapper.MapListNew<DataContracts.FindLiftTruckResult, Business.LiftTruck, DataContracts.LiftTruck>(businessEntity, LiftTruckTranslator.TranslateFromBusinessToService);
        }
    }

    public class LiftTruckTranslator
    {
        public static Business.LiftTruck TranslateFromServiceToBusiness(Service.LiftTruck serviceEntity)
        {
            return GenericMapper.MapNew<Business.LiftTruck>(serviceEntity);
        }

        public static Service.LiftTruck TranslateFromBusinessToService(Business.LiftTruck businessEntity)
        {
            return GenericMapper.MapNew<Service.LiftTruck>(businessEntity);
        }

    }
}
