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
    public class HibernateUXApplicationDao : HibernateDaoSupport, IUXApplicationDao
    {
        #region IUXApplicationDao Members

        public UXApplication FindById(Guid uxApplicationId)
        {
            return HibernateTemplate.Load<UXApplication>(uxApplicationId);
        }

        public IList<UXApplication> FindAll()
        {
            return HibernateTemplate.LoadAll<UXApplication>();
        }

        [Transaction(ReadOnly = false)]
        public UXApplication Save(UXApplication uxApplication)
        {
            HibernateTemplate.Save(uxApplication);
            return uxApplication;
        }

        [Transaction(ReadOnly = false)]
        public UXApplication SaveOrUpdate(UXApplication uxApplication)
        {
            HibernateTemplate.SaveOrUpdate(uxApplication);
            return uxApplication;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(UXApplication uxApplication)
        {
            HibernateTemplate.Delete(uxApplication);
        }


        #endregion
    }
}
