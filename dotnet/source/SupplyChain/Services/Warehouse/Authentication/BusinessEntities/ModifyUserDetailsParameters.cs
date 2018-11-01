using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Warehouse.Authentication.BusinessEntities
{
    public class ModifyUserDetailsParameters
    {
        public string UserIdentity { get; set; }
        public string RecentWarehouseIdentity { get; set; }
        public string RecentCompanyIdentity { get; set; }
        public Nullable<DateTime> LastLogonTime { get; set; }
    }
}
