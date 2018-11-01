using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Imi.Framework.Services;
using Imi.SupplyChain.OutputManager.Services.Tracing.DataContracts;

namespace Imi.SupplyChain.OutputManager.Services.Tracing.ServiceContracts
{
    [MessageContract(WrapperName = "EnableTracingRequest", WrapperNamespace = "http://Imi.SupplyChain.OutputManager.Services.Tracing.ServiceContracts/2011/09")]
    public class EnableDatabaseTracingRequest : RequestMessageBase
    {
        private EnableDatabaseTracingParameters enableTracingParams;

        [MessageBodyMember(Order = 0)]
        public EnableDatabaseTracingParameters EnableTracingParams
        {
            get { return enableTracingParams; }
            set { enableTracingParams = value; }
        }
    }
}
