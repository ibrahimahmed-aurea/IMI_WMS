using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.Wms.Mobile.UI
{
    public class DeviceInfo
    {
        private DeviceInfo()
        {
        }

        public static string GetPlatform()
        {
            return string.Format("{0} ({1})", Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER"), Environment.OSVersion.ToString());
        }
    }
}
