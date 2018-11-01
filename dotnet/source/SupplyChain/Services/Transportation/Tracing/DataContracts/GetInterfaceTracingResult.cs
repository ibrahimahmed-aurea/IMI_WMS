using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.Transportation.Services.Tracing.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.Transportation.Services.Tracing.DataContracts/2011/09")]
    public class GetInterfaceTracingResult
    {
        [DataMember(Order = 1, IsRequired = false)]
        public bool LoggIsEnabled { get; set; }

        [DataMember(Order = 2, IsRequired = false)]
        public Nullable<DateTime> LoggStopTime { get; set; }
    }
}
