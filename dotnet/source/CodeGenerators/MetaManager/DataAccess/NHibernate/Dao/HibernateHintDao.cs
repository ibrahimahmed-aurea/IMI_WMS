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
    public class HibernateHintDao : HibernateDaoSupport, IHintDao
    {
        #region IHintDao Members

        [Transaction(ReadOnly = true)]
        public Hint FindById(Guid hintId)
        {
            return HibernateTemplate.Get<Hint>(hintId) as Hint;
        }
        
        [Transaction(ReadOnly = true)]
        public IList<Hint> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Hint>("Hint.FindAll", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<Hint> FindAll()
        {
            return HibernateTemplate.LoadAll<Hint>();
        }
        
        [Transaction(ReadOnly = false)]
        public Hint Save(Hint hint)
        {
            if (hint.Id == Guid.Empty) { hint.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(hint);
            return hint;
        }

        [Transaction(ReadOnly = false)]
        public Hint SaveOrUpdate(Hint hint)
        {
            HibernateTemplate.SaveOrUpdate(hint);
            return hint;
        }

        [Transaction(ReadOnly = false)]
        public Hint SaveOrUpdateMerge(Hint hint)
        {
            object mergedObj = Session.Merge(hint);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (Hint)mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(Hint hint)
        {
            HibernateTemplate.Delete(hint);
        }


        #endregion
    }
}
