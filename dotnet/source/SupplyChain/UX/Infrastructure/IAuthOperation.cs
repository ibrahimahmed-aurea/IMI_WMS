using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX.Infrastructure
{
    public interface IAuthOperation
    {
        bool IsAuthorized { get; set; }
        string Operation { get; set; }
    }
}
