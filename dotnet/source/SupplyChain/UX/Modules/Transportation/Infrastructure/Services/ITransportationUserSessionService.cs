using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Modules.Transportation.Infrastructure.Services
{
    public interface ITransportationUserSessionService : IUserSessionService
    {
        string NodeId  {get; set;}
    }
}
