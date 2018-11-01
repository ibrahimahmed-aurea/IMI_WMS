using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Data.NHibernate.Generic.Support;
using Cdc.MetaManager.DataAccess.Dao;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.NHibernate.Dao
{
    public class HibernateDataSourceDao : HibernateDaoSupport, IDataSourceDao
    {
        #region IDataSourceDao Members

        [Transaction(ReadOnly = true)]
        public DataSource FindById(Guid dataSourceId)
        {
            return HibernateTemplate.Get<DataSource>(dataSourceId);
        }

        [Transaction(ReadOnly = true)]
        public IList<DataSource> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<DataSource>("DataSource.FindAll", paramNames, paramValues);
        }

        [Transaction(ReadOnly = false)]
        public DataSource SaveOrUpdate(DataSource dataSource)
        {
            HibernateTemplate.SaveOrUpdate(dataSource);
            return dataSource;
        }

        [Transaction(ReadOnly = false)]
        public DataSource SaveOrUpdateMerge(DataSource dataSource)
        {
            object mergedObj = Session.Merge(dataSource);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (DataSource)mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(DataSource dataSource)
        {
            HibernateTemplate.Delete(dataSource);
        }


        [Transaction(ReadOnly = true)]
        public DataSource FindByPropertyMapId(Guid propertyMapId)
        {
            string[] paramNames = { "propertyMapId" };
            object[] paramValues = { propertyMapId };

            IList<DataSource> tmp = HibernateTemplate.FindByNamedQueryAndNamedParam<DataSource>("DataSource.FindByPropertyMapId", paramNames, paramValues);

            if (tmp.Count > 0)
                return tmp[0];
            else
                return null;
        }

        [Transaction(ReadOnly = true)]
        public IList<DataSource> FindAllByServiceMethodId(Guid serviceMethodId)
        {
            string[] paramNames = { "serviceMethodId" };
            object[] paramValues = { serviceMethodId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<DataSource>("DataSource.FindAllByServiceMethodId", paramNames, paramValues);
        }

        #endregion
    }
}
