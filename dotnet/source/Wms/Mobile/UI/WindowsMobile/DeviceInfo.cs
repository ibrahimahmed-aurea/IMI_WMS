using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Imi.Wms.Mobile.UI
{
    public class DeviceInfo
    {
        private DeviceInfo()
        { 
        }

        [DllImport("Coredll.dll", EntryPoint = "SystemParametersInfoW", CharSet = CharSet.Unicode)]
        static extern int SystemParametersInfoW(uint uiAction, uint uiParam, StringBuilder pvParam, uint fWinIni);

        public enum SystemParametersInfoActions : uint
        {
            SPI_GETPLATFORMTYPE = 257,
            SPI_GETOEMINFO = 258,
        }

        public static string GetPlatform()
        { 
            return string.Format("{0} ({1})", GetDeviceInfo(), Environment.OSVersion.ToString());
        }

        private static string GetDeviceInfo()
        {
            try
            {
                StringBuilder oemInfo = new StringBuilder(256);
                if (SystemParametersInfoW((uint)SystemParametersInfoActions.SPI_GETOEMINFO,
                    (uint)oemInfo.Capacity, oemInfo, 0) != 0)
                {
                    return oemInfo.ToString();
                }
            }
            catch (Exception)
            { 
            }

            return "Unknown Device";
        }
    }
}
