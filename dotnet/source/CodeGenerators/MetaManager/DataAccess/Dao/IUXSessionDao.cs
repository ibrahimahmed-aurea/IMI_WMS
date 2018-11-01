using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IUXSessionDao
    {
        UXSession FindById(Guid uxSessionId);
        UXSession FindByApplicationId(Guid applicationId);
        UXSession Save(UXSession uxSession);
        UXSession SaveOrUpdate(UXSession uxSession);
        UXSession SaveOrUpdateMerge(UXSession uxSession);
        void Delete(UXSession uxSession);
    }
}
