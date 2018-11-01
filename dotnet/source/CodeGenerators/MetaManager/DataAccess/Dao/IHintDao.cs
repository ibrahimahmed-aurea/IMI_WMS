using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IHintDao
    {
        Hint FindById(Guid hintId);
        IList<Hint> FindAll();
        IList<Hint> FindAll(Guid applicationId);
        Hint Save(Hint hint);
        Hint SaveOrUpdate(Hint hint);
        Hint SaveOrUpdateMerge(Hint hint);
        void Delete(Hint hint);
    }
}

