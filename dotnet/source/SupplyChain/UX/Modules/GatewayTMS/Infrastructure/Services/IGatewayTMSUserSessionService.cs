using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Modules.GatewayTMS.Infrastructure.Services
{
    public interface IGatewayTMSUserSessionService : IUserSessionService
    {
        string NodeId  {get; set;}
    }
}
