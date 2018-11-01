using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Cdc.SupplyChain.Services.System.MobileSettings.DataContracts
{
    [DataContract(Namespace = "http://Cdc.SupplyChain.Services.System.MobileSettings.DataContracts/2010/02")]
    public class GetSystemTimeResult
    {
        [DataMember(Order = 1, IsRequired = true)]
        public virtual DateTime SystemTime { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder(base.ToString());
            result.Append(string.Format("\r\n{0}={1}", "SystemTime", SystemTime));
            return result.ToString();
        }	
    }
}