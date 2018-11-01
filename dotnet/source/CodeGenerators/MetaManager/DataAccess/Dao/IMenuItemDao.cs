using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IMenuItemDao
    {
        MenuItem FindById(Guid menuItemId);
        IList<MenuItem> FindAll(Guid applicationId);
        IList<MenuItem> FindAllByActionId(Guid actionId);
        MenuItem Save(MenuItem menuItem);
        MenuItem SaveOrUpdate(MenuItem menuItem);
        MenuItem SaveOrUpdateMerge(MenuItem menuItem);
        void Delete(MenuItem menuItem);
    }
}

