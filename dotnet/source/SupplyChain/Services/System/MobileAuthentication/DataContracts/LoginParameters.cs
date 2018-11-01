using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Cdc.SupplyChain.Services.System.MobileAuthentication.DataContracts
{
    [DataContract(Namespace = "http://Cdc.SupplyChain.Services.System.MobileAuthentication.DataContracts/2007/05", IsReference = true)]
    public class LoginParameters
    {
        [DataMember(Order = 1, IsRequired = false)]
        public string Username { get; set; }
		
        [DataMember(Order = 2, IsRequired = false)]
        public string Password { get; set; }
		
        [DataMember(Order = 3, IsRequired = false)]
        public string Domain { get; set; }
		

        public override string ToString()
        {
            StringBuilder result = new StringBuilder(base.ToString());
            result.Append(string.Format("\r\n{0}={1}", "Username", Username));
            result.Append(string.Format("\r\n{0}={1}", "Password", Password));
            result.Append(string.Format("\r\n{0}={1}", "Domain", Domain));
            return result.ToString();
        }	
    }
}
