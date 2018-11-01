using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Dao;
using Spring.Data.NHibernate.Generic.Support;
using Cdc.MetaManager.DataAccess.Domain;
using Spring.Transaction.Interceptor;

namespace Cdc.MetaManager.DataAccess.NHibernate.Dao
{
    public class HibernateQueryPropertyDao : HibernateDaoSupport, IQueryPropertyDao
    {
        #region IQueryPropertyDao Members

        [Transaction(ReadOnly = true)]
        public QueryProperty FindById(Guid propertyId)
        {
            return HibernateTemplate.Get<QueryProperty>(propertyId) as QueryProperty;
        }

        [Transaction(ReadOnly = true)]
        public IList<QueryProperty> FindAll()
        {
            return HibernateTemplate.LoadAll<QueryProperty>();
        }

        [Transaction(ReadOnly = false)]
        public QueryProperty Save(QueryProperty property)
        {
            if (property.Id == Guid.Empty) { property.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(property);
            return property;
        }

        [Transaction(ReadOnly = false)]
        public QueryProperty SaveOrUpdate(QueryProperty property)
        {
            HibernateTemplate.SaveOrUpdate(property);
            return property;
        }

        [Transaction(ReadOnly = false)]
        public QueryProperty SaveOrUpdateMerge(QueryProperty property)
        {
            object mergedObj = Session.Merge(property);
            HibernateTemplate.SaveOrUpdate(property);
            return (QueryProperty) mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(QueryProperty property)
        {
            HibernateTemplate.Delete(property);
        }

        [Transaction(ReadOnly = false)]
        public void DeleteAllForQuery(Guid queryId)
        {
            string[] paramNames = { "queryId" };
            object[] paramValues = { queryId };

            IList<QueryProperty> result = HibernateTemplate.FindByNamedQueryAndNamedParam<QueryProperty>(
                                                        "QueryProperty.FindAllByQueryId",
                                                        paramNames, paramValues);

            foreach (QueryProperty qp in result)
            {
                HibernateTemplate.Delete(qp);
            }
        }


        #endregion
    }
}
