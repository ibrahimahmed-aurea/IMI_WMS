using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.Transportation.Services.Authentication.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.Transportation.Services.Authentication.DataContracts/2011/09")]
    public class ModifyUserDetailsParameters
    {
        [DataMember(Order = 1, IsRequired = true)]
        public string UserIdentity { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public string RecentNodeIdentity { get; set; }

        [DataMember(Order = 3, IsRequired = true)]
        public Nullable<DateTime> LastLogonTime { get; set; }
    }
}
