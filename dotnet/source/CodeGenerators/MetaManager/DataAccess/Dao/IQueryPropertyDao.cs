using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IQueryPropertyDao
    {
        QueryProperty FindById(Guid propertyId);
        IList<QueryProperty> FindAll();
        QueryProperty Save(QueryProperty property);
        QueryProperty SaveOrUpdate(QueryProperty property);
        QueryProperty SaveOrUpdateMerge(QueryProperty property);
        void Delete(QueryProperty property);
        void DeleteAllForQuery(Guid queryId);
    }
}
