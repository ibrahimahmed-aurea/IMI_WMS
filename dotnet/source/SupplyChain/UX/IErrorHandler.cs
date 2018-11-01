using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX
{
    public interface IErrorHandler
    {
        bool HandleError(Exception error);
    }
}
