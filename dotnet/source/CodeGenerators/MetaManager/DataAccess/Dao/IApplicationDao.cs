using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IApplicationDao
    {
        Application FindById(Guid applicationId);
        IList<Application> FindAll();
        Application Save(Application application);
        Application SaveOrUpdate(Application application);
        Application SaveOrUpdateMerge(Application application);
        void Delete(Application application);
    }
}

