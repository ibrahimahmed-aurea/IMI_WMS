using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;

namespace Imi.SupplyChain.Deployment
{
    public static class AccessControlList
    {
        static AccessControlList() { }

        public static bool AccountExist(string accountName)
        {
            try
            {
                if (new NTAccount(accountName).Translate(typeof(SecurityIdentifier)) != null)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static SecurityIdentifier GetAccount(string accountName)
        {
            try
            {
                return (SecurityIdentifier)(new NTAccount(accountName).Translate(typeof(SecurityIdentifier)));
            }
            catch
            {
                return null;
            }
        }

    }
}
