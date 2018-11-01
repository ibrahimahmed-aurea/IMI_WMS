using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.Warehouse.Services.Authentication.DataContracts/2011/09")]
    public class ModifyUserDetailsParameters
    {
        [DataMember(Order = 1, IsRequired = true)]
        public string UserIdentity { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public string RecentWarehouseIdentity { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public string RecentCompanyIdentity { get; set; }

        [DataMember(Order = 3, IsRequired = true)]
        public Nullable<DateTime> LastLogonTime { get; set; }
    }
}
