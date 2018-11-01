using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Modules.OutputManager.Infrastructure.Services
{
    public interface IOutputManagerUserSessionService : IUserSessionService
    {
        string OutputManagerId  {get; set;}
    }
}
