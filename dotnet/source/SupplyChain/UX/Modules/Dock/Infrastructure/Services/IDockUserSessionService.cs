using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Modules.Dock.Infrastructure.Services
{
    public interface IDockUserSessionService : IUserSessionService
    {
        string WarehouseId  {get; set;}
    }
}
