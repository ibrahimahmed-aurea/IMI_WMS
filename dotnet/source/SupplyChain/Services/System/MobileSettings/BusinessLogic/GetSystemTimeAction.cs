using System;
using Cdc.SupplyChain.Services.System.MobileSettings.BusinessEntities;

namespace Cdc.SupplyChain.Services.System.MobileSettings.BusinessLogic
{
    public class GetSystemTimeAction : MarshalByRefObject
    {
        public GetSystemTimeAction()
        {
        }

        public GetSystemTimeResult Execute()
        {
            return new GetSystemTimeResult{SystemTime = DateTime.UtcNow};
        }
    }
}
