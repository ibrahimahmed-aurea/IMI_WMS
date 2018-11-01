using System;
using System.Threading;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using Imi.SupplyChain.Warehouse.Authentication.BusinessEntities;
using Imi.SupplyChain.Warehouse.Authentication.DataAccess;
          
namespace Imi.SupplyChain.Warehouse.Authentication.BusinessLogic
{
    public class FindUserDetailsAction : MarshalByRefObject
    {
        private const string schemaName = "OWUSER";

        public FindUserDetailsResult Execute(FindUserDetailsParameters parameters)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            string connectionString = settings.ConnectionString;

            FindUserDetailsResult result = null;
            string _userIdentity = string.Empty;
                        
            using (TransactionScope scope = new TransactionScope())
            {
                FindUserLogonDetailsAction findUserLogonDetailsAction = new FindUserLogonDetailsAction();
                IList<FindUserLogonDetailsResult> findUserLogonDetailsResult = findUserLogonDetailsAction.Execute(new FindUserLogonDetailsParameters() { UserIdentity = parameters.UserIdentity, UserPrincipalIdentity = parameters.UserPrincipalIdentity });
                
                if ((findUserLogonDetailsResult != null) && (findUserLogonDetailsResult.Count > 0))
                {
                    _userIdentity = findUserLogonDetailsResult[0].UserIdentity;
                }
                else
                {
                    _userIdentity = parameters.UserIdentity;
                }

                FindUserWarehousesAction findUserWarehousesAction = new FindUserWarehousesAction();
                FindUserCompaniesAction findUserCompaniesAction = new FindUserCompaniesAction();

                IList<FindUserWarehousesResult> findUserWarehousesResult = findUserWarehousesAction.Execute(new FindUserWarehousesParameters() { UserIdentity = _userIdentity });
                IList<FindUserCompaniesResult> findUserCompaniesResult = findUserCompaniesAction.Execute(new FindUserCompaniesParameters() { UserIdentity = _userIdentity });

                result = new FindUserDetailsResult() { UserIdentity = _userIdentity };

                if ((findUserLogonDetailsResult != null) && (findUserLogonDetailsResult.Count > 0))
                {
                    result.UserName = findUserLogonDetailsResult[0].UserName;
                    result.RecentWarehouseIdentity = findUserLogonDetailsResult[0].RecentWarehouseIdentity;
                    result.RecentCompanyIdentity = findUserLogonDetailsResult[0].RecentCompanyIdentity;
                    result.LastLogonTime = findUserLogonDetailsResult[0].LastLogonTime;
                }
                else
                {
                    // Get identity from the current thread
                    result.UserName = Thread.CurrentPrincipal.Identity.Name;
                    if (string.IsNullOrEmpty(result.UserName))
                        result.UserName = _userIdentity;
                    result.LastLogonTime = DateTime.Now;
                }

                result.Warehouses = findUserWarehousesResult;
                result.Companies = findUserCompaniesResult;

                scope.Complete();

                return result;
            }
        }
    }
}
