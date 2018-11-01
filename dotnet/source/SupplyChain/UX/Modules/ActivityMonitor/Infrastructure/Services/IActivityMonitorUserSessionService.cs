using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Modules.ActivityMonitor.Infrastructure.Services
{
    public interface IActivityMonitorUserSessionService : IUserSessionService
    {
        string WarehouseId  {get; set;}
        string ClientId { get; set; }
    }
}
