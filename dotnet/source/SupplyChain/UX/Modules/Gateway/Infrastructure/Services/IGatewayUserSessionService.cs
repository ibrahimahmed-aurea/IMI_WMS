using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Modules.Gateway.Infrastructure.Services
{
    public interface IGatewayUserSessionService : IUserSessionService
    {
        string WarehouseId  {get; set;}
    }
}
