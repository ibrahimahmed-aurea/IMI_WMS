using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.Framework.DataAccess
{
    public interface ITransactionScope : IDisposable
    {
        void Complete();
    }
}
