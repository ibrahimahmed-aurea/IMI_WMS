using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Cdc.SupplyChain.Services.System.MobileAuthentication.DataContracts
{
    [DataContract(Namespace = "http://Cdc.SupplyChain.Services.System.MobileAuthentication.DataContracts/2007/05", IsReference = true)]
    public class LoginResult
    {
        [DataMember(Order = 1, IsRequired = false)]
        public Boolean Success { get; set; }
		
        [DataMember(Order = 2, IsRequired = false)]
        public string ErrorMessage { get; set; }
		

        public override string ToString()
        {
            StringBuilder result = new StringBuilder(base.ToString());
            result.Append(string.Format("\r\n{0}={1}", "Success", Success));
            result.Append(string.Format("\r\n{0}={1}", "ErrorMessage", ErrorMessage));
            return result.ToString();
        }	
    }
}
