using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Spring.Data.NHibernate.Generic.Support;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.NHibernate.Dao
{
    public class HibernateQueryDao : HibernateDaoSupport, IQueryDao
    {
        #region IQueryDao Members

        [Transaction(ReadOnly = true)]
        public Query FindById(Guid queryId)
        {
            return HibernateTemplate.Get<Query>(queryId) as Query;
        }

        [Transaction(ReadOnly = true)]
        public Query FindByNameAndApplicationId(Guid applicationId, string name)
        {
            string[] paramNames = { "applicationId", "name" };
            object[] paramValues = { applicationId, name };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Query>(
                                                    "Query.FindByNameAndApplicationId",
                                                    paramNames, paramValues).FirstOrDefault();
        }

        [Transaction(ReadOnly = true)]
        public IList<Query> FindAllByApplicationId(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            IList<Query> result = HibernateTemplate.FindByNamedQueryAndNamedParam<Query>(
                                                    "Query.FindAllByApplicationId",
                                                    paramNames, paramValues);

            return result;
        }

        [Transaction(ReadOnly = true)]
        public IList<Query> FindAllBySchemaId(Guid schemaId)
        {
            string[] paramNames = { "schemaId" };
            object[] paramValues = { schemaId };

            IList<Query> result = HibernateTemplate.FindByNamedQueryAndNamedParam<Query>(
                                                    "Query.FindAllBySchemaId",
                                                    paramNames, paramValues);

            return result;
        }

        [Transaction(ReadOnly = true)]
        public IList<Query> FindAllBySchemaIdAndQueryType(Guid schemaId, QueryType queryType)
        {
            string[] paramNames = { "schemaId", "queryType" };
            object[] paramValues = { schemaId, queryType };

            IList<Query> result = HibernateTemplate.FindByNamedQueryAndNamedParam<Query>(
                                                    "Query.FindAllBySchemaIdAndQueryType",
                                                    paramNames, paramValues);

            return result;
        }

        [Transaction(ReadOnly = false)]
        public Query Save(Query query)
        {
            if (query.Id == Guid.Empty) { query.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(query);
            return query;
        }

        [Transaction(ReadOnly = false)]
        public Query SaveOrUpdate(Query query)
        {
            HibernateTemplate.SaveOrUpdate(query);
            return query;
        }

        [Transaction(ReadOnly = false)]
        public Query SaveOrUpdateMerge(Query query)
        {
            object mergedObj = Session.Merge(query);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (Query) mergedObj;
        }

        

        [Transaction(ReadOnly = false)]
        public void Delete(Query query)
        {
            HibernateTemplate.Delete(query);
        }


        #endregion
    }
}
