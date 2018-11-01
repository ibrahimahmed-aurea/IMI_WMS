using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Warehouse.Authentication.BusinessEntities
{
    public class FindUserLogonDetailsParameters
    {
        public string UserIdentity { get; set; }
        public string UserPrincipalIdentity { get; set; }
    }
}
