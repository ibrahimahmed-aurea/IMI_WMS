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
    public class HibernateDynamicDao : HibernateDaoSupport, IDynamicDao
    {
        [Transaction(ReadOnly = true)]
        public IList<IDomainObject> FindByQuery(string query)
        {
            return HibernateTemplate.Find<IDomainObject>(query);
        }
    }
}
