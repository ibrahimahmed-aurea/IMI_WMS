using System;
using System.Threading;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.Framework.DataAccess;
using Cdc.Framework.Services;
using Cdc.SupplyChain.Warehouse.BusinessLogic.Movement.Entities;
//using Cdc.SupplyChain.Warehouse.BusinessLogic.Movement.DataAccess;
          
namespace Cdc.SupplyChain.Warehouse.BusinessLogic.Movement.BusinessLogic
{
    public class FindLiftTruckAction : MarshalByRefObject
    {
        private const string schemaName = "OWUSER";

        private List<LiftTruck> liftTrucks = null;

        public FindLiftTruckResult Execute(FindLiftTruckParameters parameters)
        {
            if (liftTrucks == null)
            {
              liftTrucks = new FindLiftTruckResult()
              {
                  new LiftTruck() { LiftTruckIdentity = "LT01", LiftTruckName = "Lift Truck 01A" },
                  new LiftTruck() { LiftTruckIdentity = "LT02", LiftTruckName = "Lift Truck 02B" },
                  new LiftTruck() { LiftTruckIdentity = "LT03", LiftTruckName = "Lift Truck 03B" },
                  new LiftTruck() { LiftTruckIdentity = "LT04", LiftTruckName = "Lift Truck 04C" },
                  new LiftTruck() { LiftTruckIdentity = "LT05", LiftTruckName = "Lift Truck 05A" },
                  new LiftTruck() { LiftTruckIdentity = "LT06", LiftTruckName = "Lift Truck 06B" },
                  new LiftTruck() { LiftTruckIdentity = "LT07", LiftTruckName = "Lift Truck 07A" },
                  new LiftTruck() { LiftTruckIdentity = "LT08", LiftTruckName = "Lift Truck 08B" },
                  new LiftTruck() { LiftTruckIdentity = "LT09", LiftTruckName = "Lift Truck 09C" },
                  new LiftTruck() { LiftTruckIdentity = "LT10", LiftTruckName = "Lift Truck 10B" },
              };
            }

            var selectResult = from l in liftTrucks
                         where l.LiftTruckIdentity.Contains(parameters.SearchString) ||
                               l.LiftTruckName.Contains(parameters.SearchString)
                         select l;


            var result = new FindLiftTruckResult();

            if ((selectResult != null) && (selectResult.Any()))
            {
                result.AddRange(selectResult.ToArray<LiftTruck>());
            }

            return result; 
            
            //ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            //string connectionString = settings.ConnectionString;

            //FindLiftTruckResult result = null;
                        
            //using (TransactionScope scope = new TransactionScope())
            //{
            //    FindUserLogonDetailsAction findUserLogonDetailsAction = new FindUserLogonDetailsAction();
            //    IList<FindUserLogonDetailsResult> findUserLogonDetailsResult = findUserLogonDetailsAction.Execute(new FindUserLogonDetailsParameters() { UserIdentity = parameters.UserIdentity });

            //    FindUserWarehousesAction findUserWarehousesAction = new FindUserWarehousesAction();
            //    FindUserCompaniesAction findUserCompaniesAction = new FindUserCompaniesAction();
            //    IList<FindUserWarehousesResult> findUserWarehousesResult;
            //    IList<FindUserCompaniesResult> findUserCompaniesResult;

            //    // If the user does not exist (i.e. Windows user without app user account)
            //    // show all nodes for now.
            //    if (findUserLogonDetailsResult.Count == 0)
            //    {
            //        // Special
            //        findUserWarehousesResult = findUserWarehousesAction.Execute(null);
            //        findUserCompaniesResult = findUserCompaniesAction.Execute(null);
            //    }
            //    else
            //    {
            //        findUserWarehousesResult = findUserWarehousesAction.Execute(new FindUserWarehousesParameters() { UserIdentity = parameters.UserIdentity });
            //        findUserCompaniesResult = findUserCompaniesAction.Execute(new FindUserCompaniesParameters() { UserIdentity = parameters.UserIdentity });
            //    }

            //    result = new FindLiftTruckResult() { UserIdentity = parameters.UserIdentity };

            //    if ((findUserLogonDetailsResult != null) && (findUserLogonDetailsResult.Count > 0))
            //    {
            //        result.UserName = findUserLogonDetailsResult[0].UserName;
            //        result.RecentWarehouseIdentity = findUserLogonDetailsResult[0].RecentWarehouseIdentity;
            //        result.RecentCompanyIdentity = findUserLogonDetailsResult[0].RecentCompanyIdentity;
            //        result.LastLogonTime = findUserLogonDetailsResult[0].LastLogonTime;
            //    }
            //    else
            //    {
            //        // Get identity from the current thread
            //        result.UserName = Thread.CurrentPrincipal.Identity.Name;
            //        if (string.IsNullOrEmpty(result.UserName))
            //            result.UserName = parameters.UserIdentity;
            //        result.LastLogonTime = DateTime.Now;
            //    }

            //    result.Warehouses = findUserWarehousesResult;
            //    result.Companies = findUserCompaniesResult;

            //    scope.Complete();

            //    return result;
            //}
        }
    }
}
