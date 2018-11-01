using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IMenuDao
    {
        Menu FindById(Guid menuId);
        Menu FindByApplicationId(Guid applicationId);
        IList<Menu> FindAll(Guid applicationId);
        Menu Save(Menu menu);
        Menu SaveOrUpdate(Menu menu);
        Menu SaveOrUpdateMerge(Menu menu);
        void Delete(Menu menu);
    }
}

