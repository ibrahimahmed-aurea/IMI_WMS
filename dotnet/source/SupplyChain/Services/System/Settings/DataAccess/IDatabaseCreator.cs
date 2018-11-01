using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.Settings.DataAccess
{
    public interface IDatabaseCreator
    {
        bool CreateDatabase(bool replaceExisting);
        bool DatabaseExists();
    }
}
