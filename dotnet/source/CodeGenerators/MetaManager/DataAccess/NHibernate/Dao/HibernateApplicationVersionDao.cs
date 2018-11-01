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
    public class HibernateApplicationVersionDao : HibernateDaoSupport, IApplicationVersionDao
    {
        #region IApplicationVersionDao Members

        public ApplicationVersion FindById(Guid applicationVersionId)
        {
            return HibernateTemplate.Load<ApplicationVersion>(applicationVersionId) as ApplicationVersion;
        }

        public IList<ApplicationVersion> FindAll()
        {
            return HibernateTemplate.LoadAll<ApplicationVersion>();
        }

        [Transaction(ReadOnly = false)]
        public ApplicationVersion Save(ApplicationVersion applicationVersion)
        {
            HibernateTemplate.Save(applicationVersion);
            return applicationVersion;
        }

        [Transaction(ReadOnly = false)]
        public ApplicationVersion SaveOrUpdate(ApplicationVersion applicationVersion)
        {
            HibernateTemplate.SaveOrUpdate(applicationVersion);
            return applicationVersion;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(ApplicationVersion applicationVersion)
        {
            HibernateTemplate.Delete(applicationVersion);
        }


        #endregion
    }
}
