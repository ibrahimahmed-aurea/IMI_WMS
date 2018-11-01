using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IServiceMethodDao
    {
        ServiceMethod FindById(Guid serviceMethodId);
        IList<ServiceMethod> FindAll();
        IList<ServiceMethod> FindAllByApplicationId(Guid applicationId);
        IList<ServiceMethod> FindAllQueriesByApplicationId(Guid applicationId);
        IList<ServiceMethod> FindAllRefCursorProcsByApplicationId(Guid applicationId);
        IList<ServiceMethod> FindAllByService(Guid serviceId);
        IList<ServiceMethod> FindByApplicationIdNameAndService(Guid applicationId, string name, string serviceName);
        ServiceMethod Save(ServiceMethod serviceMethod);
        ServiceMethod SaveOrUpdate(ServiceMethod serviceMethod);
        ServiceMethod SaveOrUpdateMerge(ServiceMethod serviceMethod);
        void Delete(ServiceMethod serviceMethod);
        ServiceMethod FindByPropertyMapId(Guid propertyMapId);
    }
}

