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
    public class HibernateViewInterfacePropertyDao : HibernateDaoSupport, IViewInterfacePropertyDao
    {
        #region IViewInterfacePropertyDao Members

        public ViewInterfaceProperty FindById(int viewInterfacePropertyId)
        {
            return HibernateTemplate.Load<ViewInterfaceProperty>(viewInterfacePropertyId);
        }

        public IList<ViewInterfaceProperty> FindAll()
        {
            return HibernateTemplate.LoadAll<ViewInterfaceProperty>();
        }

        [Transaction(ReadOnly = false)]
        public ViewInterfaceProperty Save(ViewInterfaceProperty viewInterfaceProperty)
        {
            HibernateTemplate.Save(viewInterfaceProperty);
            return viewInterfaceProperty;
        }

        [Transaction(ReadOnly = false)]
        public ViewInterfaceProperty SaveOrUpdate(ViewInterfaceProperty viewInterfaceProperty)
        {
            HibernateTemplate.SaveOrUpdate(viewInterfaceProperty);
            return viewInterfaceProperty;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(ViewInterfaceProperty viewInterfaceProperty)
        {
            HibernateTemplate.Delete(viewInterfaceProperty);
        }

        #endregion
    }
}
