using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.Transportation.Services.Authentication.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.Transportation.Services.Authentication.DataContracts/2011/09")]
    public class UserNode
    {
        [DataMember(Order = 1, IsRequired = false)]
        public string NodeIdentity { get; set; }

        [DataMember(Order = 2, IsRequired = false)]
        public string NodeName { get; set; }
    }

    [CollectionDataContract(Namespace = "http://Imi.SupplyChain.Transportation.Services.Authentication.DataContracts/2011/09")]
    public class UserNodeCollection : List<UserNode>
    {
        public UserNodeCollection()
        {
        }

        public UserNodeCollection(IEnumerable<UserNode> collection)
            : base(collection)
        {
        }
    }


    [DataContract(Namespace = "http://Imi.SupplyChain.Transportation.Services.Authentication.DataContracts/2011/09")]
    public class FindUserDetailsResult
    {
        [DataMember(Order = 1, IsRequired = false)]
        public string UserIdentity { get; set; }

        [DataMember(Order = 2, IsRequired = false)]
        public string UserName { get; set; }

        [DataMember(Order = 3, IsRequired = false)]
        public string RecentNodeIdentity { get; set; }

        [DataMember(Order = 4, IsRequired = false)]
        public Nullable<DateTime> LastLogonTime { get; set; }

        [DataMember(Order = 5, IsRequired = false)]
        public UserNodeCollection UserNodes { get; set; }
    }
}
