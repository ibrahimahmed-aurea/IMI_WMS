using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IDynamicDao
    {
        IList<IDomainObject> FindByQuery(string query);
    }
}
