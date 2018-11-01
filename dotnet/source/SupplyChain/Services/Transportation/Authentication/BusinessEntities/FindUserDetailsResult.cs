using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Transportation.Authentication.BusinessEntities
{
    public class FindUserDetailsResult : FindUserLogonDetailsResult
    {
        public string UserIdentity { get; set; }
        public IList<FindUserNodesResult> UserNodes { get; set; }
    }
}
