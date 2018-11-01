using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Modules.GatewayMHS.Infrastructure.Services
{
    public interface IGatewayMHSUserSessionService : IUserSessionService
    {
        string WarehouseId  {get; set;}
    }
}
