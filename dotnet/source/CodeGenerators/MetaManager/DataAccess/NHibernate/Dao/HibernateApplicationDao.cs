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
    public class HibernateApplicationDao : HibernateDaoSupport, IApplicationDao
    {
        #region IApplicationDao Members

        [Transaction(ReadOnly = true)]
        public Application FindById(Guid applicationId)
        {
            return HibernateTemplate.Get<Application>(applicationId);
        }

        [Transaction(ReadOnly = true)]
        public IList<Application> FindAll()
        {
            return HibernateTemplate.LoadAll<Application>();
        }

        [Transaction(ReadOnly = false)]
        public Application Save(Application application)
        {
            if (application.Id == Guid.Empty) { application.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(application);
            return application;
        }

        [Transaction(ReadOnly = false)]
        public Application SaveOrUpdate(Application application)
        {
            HibernateTemplate.SaveOrUpdate(application);
            return application;
        }

        [Transaction(ReadOnly = false)]
        public Application SaveOrUpdateMerge(Application application)
        {
            object mergedObj = Session.Merge(application);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (Application)mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(Application application)
        {
            HibernateTemplate.Delete(application);
        }


        #endregion
    }
}
