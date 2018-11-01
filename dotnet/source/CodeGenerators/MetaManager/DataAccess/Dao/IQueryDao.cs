using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IQueryDao
    {
        Query FindById(Guid queryId);
        Query FindByNameAndApplicationId(Guid applicationId, string name);
        IList<Query> FindAllByApplicationId(Guid applicationId);
        IList<Query> FindAllBySchemaId(Guid schemaId);
        IList<Query> FindAllBySchemaIdAndQueryType(Guid schemaId, QueryType queryType);
        Query Save(Query query);
        Query SaveOrUpdate(Query query);
        Query SaveOrUpdateMerge(Query query);
        void Delete(Query query);
    }
}

