using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Transportation.Tracing.BusinessEntities
{
    public class StartDatabaseTracingParameters
    {
        public string TerminalId { get; set; }
        public string UserId { get; set; }
        public bool WriteHeader { get; set; }
    }
}
