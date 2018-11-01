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
    public class HibernateUXActionInterfacePropertyDao : HibernateDaoSupport, IUXActionInterfacePropertyDao
    {
        #region IUXActionInterfacePropertyDao Members

        public ActionInterfaceProperty FindById(int actionInterfacePropertyId)
        {
            return HibernateTemplate.Load<ActionInterfaceProperty>(actionInterfacePropertyId) as ActionInterfaceProperty;
        }

        public IList<ActionInterfaceProperty> FindAll()
        {
            return HibernateTemplate.LoadAll<ActionInterfaceProperty>();
        }

        [Transaction(ReadOnly = false)]
        public ActionInterfaceProperty Save(ActionInterfaceProperty actionInterfaceProperty)
        {
            HibernateTemplate.Save(actionInterfaceProperty);
            return actionInterfaceProperty;
        }

        [Transaction(ReadOnly = false)]
        public ActionInterfaceProperty SaveOrUpdate(ActionInterfaceProperty actionInterfaceProperty)
        {
            HibernateTemplate.SaveOrUpdate(actionInterfaceProperty);
            return actionInterfaceProperty;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(ActionInterfaceProperty actionInterfaceProperty)
        {
            HibernateTemplate.Delete(actionInterfaceProperty);
        }

        #endregion
    }
}
