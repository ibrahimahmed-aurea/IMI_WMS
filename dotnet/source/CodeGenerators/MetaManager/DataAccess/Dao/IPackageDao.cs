using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IPackageDao
    {
        Package FindById(Guid packageId);
        Package FindByNameAndSchemaId(string packageName, Guid schemaId);
        Package FindByStoredProcedureId(Guid storedProcedureId);
        IList<Package> FindAllBySchemaId(Guid schemaId);
        IList<Package> FindAllByApplicationId(Guid applicationId);
        Package Save(Package package);
        Package SaveOrUpdate(Package package);
        Package SaveOrUpdateMerge(Package package);
        void Delete(Package package);
    }
}

