using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IStoredProcedureDao
    {
        StoredProcedure FindById(Guid storedProcedureId);
        IList<StoredProcedure> FindAllByPackageId(Guid packageId);
        StoredProcedure Save(StoredProcedure storedProcedure);
        StoredProcedure SaveOrUpdate(StoredProcedure storedProcedure);
        StoredProcedure SaveOrUpdateMerge(StoredProcedure storedProcedure);
        void Delete(StoredProcedure storedProcedure);
        StoredProcedure FindByProcedurePropertyId(Guid procedurePropertyId);
    }
}

