using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Warehouse.Authentication.BusinessEntities
{
    public class FindUserDetailsResult : FindUserLogonDetailsResult
    {
        public string UserIdentity { get; set; }
        public IList<FindUserWarehousesResult> Warehouses { get; set; }
        public IList<FindUserCompaniesResult> Companies { get; set; }
    }
}
