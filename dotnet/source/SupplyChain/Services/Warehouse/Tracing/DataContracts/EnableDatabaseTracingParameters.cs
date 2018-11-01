using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.Warehouse.Services.Tracing.DataContracts
{
    [DataContract(Namespace = "http://Imi.SupplyChain.Warehouse.Services.Tracing.DataContracts/2011/09")]
    public class EnableDatabaseTracingParameters
    {
        [DataMember(Order = 1, IsRequired = true)]
        public bool IsTracingEnabled { get; set; }
    }
}
