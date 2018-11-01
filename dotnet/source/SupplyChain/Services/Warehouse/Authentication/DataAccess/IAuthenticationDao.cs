using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Warehouse.Authentication.BusinessEntities;

namespace Imi.SupplyChain.Warehouse.Authentication.DataAccess
{
    public interface IAuthenticationDao
    {
        IList<FindUserWarehousesResult> FindAllWarehouses();
        IList<FindUserCompaniesResult> FindAllCompanies();
        IList<FindUserWarehousesResult> FindUserWarehouses(FindUserWarehousesParameters parameters);
        IList<FindUserCompaniesResult> FindUserCompanies(FindUserCompaniesParameters parameters);
        IList<FindUserLogonDetailsResult> FindUserLogonDetails(FindUserLogonDetailsParameters parameters);
        void ModifyUserDetails(ModifyUserDetailsParameters parameters);
        LogonResult Logon(LogonParameters parameters);
        
    }
}
