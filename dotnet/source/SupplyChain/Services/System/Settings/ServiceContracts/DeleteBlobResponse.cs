// Generated from template: .\ServiceContracts\ServiceContractClassTemplate.cst
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Imi.SupplyChain.Services.Settings.DataContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceContracts
{
    [MessageContract(WrapperName = "DeleteBlobResponse", WrapperNamespace = "http://Imi.SupplyChain.Services.Settings.ServiceContracts/2011/09")]
    public class DeleteBlobResponse
    {
        [MessageBodyMember(Order = 0)]
        public DeleteBlobResult DeleteBlobResult { get; set; }
		
		public override string ToString()
		{
			return string.Format("{0}\r\n{1}", base.ToString(), DeleteBlobResult.ToString());
		}
    }
}
