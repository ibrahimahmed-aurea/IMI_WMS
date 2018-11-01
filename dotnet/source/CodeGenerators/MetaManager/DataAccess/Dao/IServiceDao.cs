using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IServiceDao
    {
        Service FindById(Guid serviceId);
        IList<Service> FindAll(Guid applicationId);
        Service FindByName(Guid applicationId, string serviceName);
        Service Save(Service service);
        Service SaveOrUpdate(Service service);
        Service SaveOrUpdateMerge(Service service);
        void Delete(Service service);
    }
}

