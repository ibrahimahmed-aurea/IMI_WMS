using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IUXApplicationDao
    {
        UXApplication FindById(Guid uxApplicationId);
        IList<UXApplication> FindAll();
        UXApplication Save(UXApplication uxApplication);
        UXApplication SaveOrUpdate(UXApplication uxApplication);
        void Delete(UXApplication uxApplication);
    }
}

