using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Modules.Warehouse.Infrastructure.Services
{
    public interface IWarehouseUserSessionService : IUserSessionService
    {
        string WarehouseId  {get; set;}
        string ClientId { get; set; }
        bool IsClientInterfaceHAPI { get; set; }
        bool IsClientInterfaceWebServices { get; set; }
        bool IsClientInterfaceEDI { get; set; }
    }
}
