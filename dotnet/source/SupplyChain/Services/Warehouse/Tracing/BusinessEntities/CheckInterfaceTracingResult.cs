using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Warehouse.Tracing.BusinessEntities
{
    public class CheckInterfaceTracingResult
    {
        public bool LoggOn { get; set; }
        public int LoggInterval { get; set; }
        public DateTime LoggStarted { get; set; }
    }
}
