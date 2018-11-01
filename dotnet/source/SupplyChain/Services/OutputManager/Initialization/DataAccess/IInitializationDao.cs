using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.OutputManager.Initialization.BusinessEntities;

namespace Imi.SupplyChain.OutputManager.Initialization.DataAccess
{
    public interface IInitializationDao
    {
        IList<FindOutputManagerResult> FindAllOutputManagers();
        
        
    }
}
