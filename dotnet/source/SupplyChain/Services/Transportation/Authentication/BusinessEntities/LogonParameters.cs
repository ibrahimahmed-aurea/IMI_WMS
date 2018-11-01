using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Transportation.Authentication.BusinessEntities
{
    public class LogonParameters
    {
        public string UserIdentity { get; set; }
        public string NodeIdentity { get; set; }
        public string TerminalIdentity { get; set; }
        public string ApplicationIdentity { get; set; }
    }
}
