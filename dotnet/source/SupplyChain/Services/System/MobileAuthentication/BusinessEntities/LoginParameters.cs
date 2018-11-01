using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.SupplyChain.Services.System.MobileAuthentication.BusinessEntities
{
    public class LoginParameters
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder(base.ToString());
            result.Append(string.Format("\r\n{0}={1}", "UserName", Username));
            result.Append(string.Format("\r\n{0}={1}", "Password", "*".PadRight(Password.Length)));
            result.Append(string.Format("\r\n{0}={1}", "Domain", Domain));
            return result.ToString();
        }	

    }
}
