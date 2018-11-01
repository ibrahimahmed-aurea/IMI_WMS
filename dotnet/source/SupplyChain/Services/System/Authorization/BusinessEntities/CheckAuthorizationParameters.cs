using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace Imi.SupplyChain.Authorization.BusinessEntities
{
    public class CheckAuthorizationParameters
    {
        public IList<string> Roles { get; set; }

        public string AuthorizationProvider { get; set; }

        public IList<AuthOperation> Operations { get; set; }
    }
}
