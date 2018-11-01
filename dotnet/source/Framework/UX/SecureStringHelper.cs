using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

namespace Imi.Framework.UX
{
    public class SecureStringHelper
    {
        private SecureStringHelper()
        { 
        }

        public static SecureString GetSecureString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            SecureString spwd = new SecureString();
            foreach (char c in value)
            {
                spwd.AppendChar(c);
            }

            spwd.MakeReadOnly();
            return spwd;
        }

        public static IntPtr GetString(SecureString value)
        {
            return Marshal.SecureStringToBSTR(value);
        }

        public static void FreeString(IntPtr value)
        {
            Marshal.ZeroFreeBSTR(value);
        }
    }
}
