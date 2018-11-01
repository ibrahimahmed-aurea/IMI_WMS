using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.BusinessLogic
{
    public interface IBackendService
    {
        IList<T> GetAllDomainObjectsByApplicationId<T>(Guid ApplicationId);
    }
}
