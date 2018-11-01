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
    public class HibernatePropertyCodeDao : HibernateDaoSupport, IPropertyCodeDao
    {
        [Transaction(ReadOnly = true)]
        public PropertyCode FindById(Guid propertyCodeId)
        {
            return HibernateTemplate.Get<PropertyCode>(propertyCodeId);
        }
    }
}
