using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace Imi.SupplyChain.Deployment
{
    public static class IISHelper
    {
        const string IISRegKeyName = "Software\\Microsoft\\InetStp";
        const string IISRegKeyMajorVersionValue = "MajorVersion";
        const string IISRegKeyMinorVersionValue = "MinorVersion";
        const string IISPathWWWRootValue = "PathWWWRoot";

        private static bool GetRegistryValue<T>(RegistryHive hive, string key, string value, RegistryValueKind kind, out T data)
        {
            bool success = false;
            data = default(T);

            using (RegistryKey baseKey = RegistryKey.OpenRemoteBaseKey(hive, String.Empty))
            {
                if (baseKey != null)
                {
                    using (RegistryKey registryKey = baseKey.OpenSubKey(key, RegistryKeyPermissionCheck.ReadSubTree))
                    {
                        if (registryKey != null)
                        {
                            try
                            {
                                // If the key was opened, try to retrieve the value.
                                RegistryValueKind kindFound = registryKey.GetValueKind(value);
                                if (kindFound == kind)
                                {
                                    object regValue = registryKey.GetValue(value, null);
                                    if (regValue != null)
                                    {
                                        data = (T)Convert.ChangeType(regValue, typeof(T), CultureInfo.InvariantCulture);
                                        success = true;
                                    }
                                }
                            }
                            catch (IOException)
                            {
                                // The registry value doesn't exist. Since the
                                // value doesn't exist we have to assume that
                                // the component isn't installed and return
                                // false and leave the data param as the
                                // default value.
                            }
                        }
                    }
                }
            }
            return success;
        }

        /// <summary>
        ///     Get the IIS version.
        ///        4   = Windows NT4 Option Pack.
        ///        5   = Windows 2000 Server.
        ///        5.1 = Windows XP Professional.
        ///        6   = Windows Server 2003.
        ///        7   = Windows Vista / Windows Server 2008.
        ///        7.5 = Windows 7 / Windows Server 2008 R2.
        /// </summary>
        /// <returns></returns>
        public static Version GetVersion()
        {
            int majorVersion = 0;
            int minorVersion = 0;

            if (GetRegistryValue(RegistryHive.LocalMachine, IISRegKeyName, IISRegKeyMajorVersionValue, RegistryValueKind.DWord, out majorVersion) &&
                GetRegistryValue(RegistryHive.LocalMachine, IISRegKeyName, IISRegKeyMinorVersionValue, RegistryValueKind.DWord, out minorVersion))
                return new Version(majorVersion, minorVersion);
            else
                return null;
        }

        /// <summary>
        ///     Gets the local WWW Root Path for the installed IIS, but only if it's installed.
        ///     Returns an empty string if it's not installed.
        /// </summary>
        /// <returns></returns>
        public static string GetWWWRootPath()
        {
            string wwwRootPath = string.Empty;

            if (GetRegistryValue(RegistryHive.LocalMachine, IISRegKeyName, IISPathWWWRootValue, RegistryValueKind.String, out wwwRootPath))
                return wwwRootPath;
            else
                return string.Empty;
        }

    }
}
