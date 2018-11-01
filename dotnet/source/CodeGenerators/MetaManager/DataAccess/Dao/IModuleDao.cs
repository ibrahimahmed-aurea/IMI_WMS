using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IModuleDao
    {
        Module FindById(Guid moduleId);
        IList<Module> FindAll();
        IList<Module> FindAll(Guid applicationId);
        Module Save(Module module);
        Module SaveOrUpdate(Module module);
        Module SaveOrUpdateMerge(Module module);
        void Delete(Module module);
    }
}
