using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Cdc.Framework.Services;
using Cdc.SupplyChain.Services.System.MobileAuthentication.DataContracts;

namespace Cdc.SupplyChain.Services.System.MobileAuthentication.ServiceContracts
{
    [MessageContract(WrapperName = "LoginResponse", WrapperNamespace = "http://Cdc.SupplyChain.Services.System.MobileAuthentication.ServiceContracts/2007/05")]
    public class LoginResponse
    {
        [MessageBodyMember(Order = 0)]
        public LoginResult LoginResult { get; set; }
		
		public override string ToString()
		{
			return string.Format("{0}\r\n{1}", base.ToString(), LoginResult.ToString());
		}
    }
}
