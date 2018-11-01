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
    public class HibernateUXSessionDao : HibernateDaoSupport, IUXSessionDao
    {
        #region IUXSessionDao Members

        [Transaction(ReadOnly = true)]
        public UXSession FindById(Guid uxSessionId)
        {
            return HibernateTemplate.Get<UXSession>(uxSessionId);
        }

        [Transaction(ReadOnly = true)]
        public UXSession FindByApplicationId(Guid applicationId)
        {
            var s = from a in HibernateTemplate.LoadAll<UXSession>()
                    where a.Application.Id == applicationId
                    select a;
            if (s.Count() > 0)
                return s.First();
            else
                return null;
        }

        [Transaction(ReadOnly = false)]
        public UXSession Save(UXSession uxSession)
        {
            if (uxSession.Id == Guid.Empty) { uxSession.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(uxSession);
            return uxSession;
        }

        [Transaction(ReadOnly = false)]
        public UXSession SaveOrUpdate(UXSession uxSession)
        {
            HibernateTemplate.SaveOrUpdate(uxSession);
            return uxSession;
        }

        [Transaction(ReadOnly = false)]
        public UXSession SaveOrUpdateMerge(UXSession uxSession)
        {
            object mergedObj = Session.Merge(uxSession);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (UXSession) mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(UXSession uxSession)
        {
            HibernateTemplate.Delete(uxSession);
        }

        #endregion
    }
}
