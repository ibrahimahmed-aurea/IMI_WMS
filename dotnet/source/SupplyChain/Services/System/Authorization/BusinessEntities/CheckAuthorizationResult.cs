using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Authorization.BusinessEntities
{
    public class CheckAuthorizationResult
    {
        public IList<AuthOperation> Operations { get; set; }
        public bool IsAuthorizationEnabled { get; set; }
    }
}
