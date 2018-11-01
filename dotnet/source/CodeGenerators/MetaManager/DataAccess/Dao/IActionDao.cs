using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DAD = Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IActionDao
    {
        DAD.Action FindById(Guid actionId);
        DAD.Action FindByQueryId(Guid queryId);
        DAD.Action FindByStoredProcedureId(Guid storedProcedureId);
        IList<DAD.Action> FindAll();
        IList<DAD.Action> FindAll(Guid applicationId);
        IList<DAD.Action> FindByEntityAndName(string entityName, string actionName, Guid applicationId);
        IList<DAD.Action> FindAllUnassigned(Guid applicationId);
        IList<DAD.Action> FindByBusinessEntityId(Guid businessEntityId);
        DAD.Action FetchWithMaps(Guid actionId);
        DAD.Action Save(DAD.Action action);
        DAD.Action SaveOrUpdate(DAD.Action action);
        DAD.Action SaveOrUpdateMerge(DAD.Action action);
        void Delete(DAD.Action action);
        DAD.Action FindByPropertyMapId(Guid propertyMapId);
        DAD.Action FindByServiceMethodId(Guid serviceMethodId);
    }
}

