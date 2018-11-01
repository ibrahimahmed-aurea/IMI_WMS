using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.IdentityModel.Claims;

namespace Imi.SupplyChain.Services.OrderManagement.Utilities.ServiceImplementation
{
    public class Authorizer
    {
        static Authorizer instance;

        private Authorizer()
        {
        }

        public static Authorizer GetInstance()
        {
            if (instance == null)
            {
                instance = new Authorizer();
            }
            return instance;
        }

        public bool IsAuthorized(string user, bool useDomainInUserID)
        {
            string loginId = getLoginId();
            bool authorized = false; // TODO: default should be configurable
            if (loginId != null)
            {
                if (!useDomainInUserID)
                {   // strip away domain name
                    int idx;
                    if ((idx = loginId.IndexOf('\\')) != -1)
                    {
                        loginId = loginId.Substring(idx + 1);
                    }
                    else if ((idx = loginId.IndexOf('/')) != -1)
                    {
                        loginId = loginId.Substring(idx + 1);
                    }
                }
                authorized = (user == loginId);
            }
            return authorized;
        }

        public string getLoginId()
        {
            string loginId = null;
            IClaimsIdentity identity = Thread.CurrentPrincipal.Identity as IClaimsIdentity;
            if (identity != null)
            {
                loginId = identity.Claims.FindAll(c => { return c.ClaimType == ClaimTypes.Name; }).First().Value;
            }
            return loginId;
        }
    }
}
