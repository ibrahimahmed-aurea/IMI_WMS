using System;
using System.Collections.Generic;
using System.Text;

namespace Imi.SupplyChain.Server.Job.Gateway
{
    public class EdiSegment
    {
        public double EdiOutId { get; set; }
        public double NextRowNumber { get; set; }
        public string EdiData { get; set; }
    }
}
