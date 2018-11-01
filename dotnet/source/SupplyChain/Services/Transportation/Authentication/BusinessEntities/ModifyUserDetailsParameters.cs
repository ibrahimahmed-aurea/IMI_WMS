using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Transportation.Authentication.BusinessEntities
{
    public class ModifyUserDetailsParameters
    {
        public string UserIdentity { get; set; }
        public string RecentNodeIdentity { get; set; }
        public Nullable<DateTime> LastLogonTime { get; set; }
    }
}
