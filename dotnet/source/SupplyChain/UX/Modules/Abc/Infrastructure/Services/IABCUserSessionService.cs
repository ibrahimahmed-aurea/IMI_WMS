using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Modules.ABC.Infrastructure.Services
{
    public interface IABCUserSessionService : IUserSessionService
    {
        string WarehouseId  {get; set;}
        string ClientId { get; set; }
    }
}
