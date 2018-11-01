using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.SupplyChain.Services.System.MobileSettings.BusinessEntities
{
    public class GetSystemTimeResult
    {
        public DateTime SystemTime { get; set; }

        public GetSystemTimeResult()
        {
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder(base.ToString());
            result.Append(string.Format("\r\n{0}={1}", "SystemTime", SystemTime.ToLocalTime()));
            return result.ToString();
        }	
    }
}
