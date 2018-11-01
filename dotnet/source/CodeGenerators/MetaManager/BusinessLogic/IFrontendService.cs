using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.BusinessLogic
{
    public interface IFrontendService
    {
        IList<T> GetAllDomainObjectsByApplicationId<T>(Guid ApplicationId);
        T GetInitializedDomainObject<T>(Guid domainObjectId);
    }
}
