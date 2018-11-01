using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IStoredProcedurePropertyDao
    {
        ProcedureProperty FindById(Guid propertyId);
        IList<ProcedureProperty> FindAll();
        ProcedureProperty Save(ProcedureProperty property);
        ProcedureProperty SaveOrUpdate(ProcedureProperty property);
        ProcedureProperty SaveOrUpdateMerge(ProcedureProperty property);
        void Delete(ProcedureProperty property);
    }
}
