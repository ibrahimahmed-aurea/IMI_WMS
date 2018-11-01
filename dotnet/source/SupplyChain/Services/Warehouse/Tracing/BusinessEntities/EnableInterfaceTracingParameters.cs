using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Warehouse.Tracing.BusinessEntities
{
    public class EnableInterfaceTracingParameters
    {
        public bool IsTracingEnabled { get; set; }
        public int DurationInSeconds {get; set;}
    }
}
