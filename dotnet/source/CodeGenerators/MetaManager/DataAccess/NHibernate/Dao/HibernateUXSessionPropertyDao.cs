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
    public class HibernateUXSessionPropertyDao : HibernateDaoSupport, IUXSessionPropertyDao
    {
        #region IUXSessionPropertyDao Members

        [Transaction(ReadOnly = true)]
        public UXSessionProperty FindById(Guid uxSessionPropertyId)
        {
            return HibernateTemplate.Get<UXSessionProperty>(uxSessionPropertyId);
        }
        #endregion
    }
}
