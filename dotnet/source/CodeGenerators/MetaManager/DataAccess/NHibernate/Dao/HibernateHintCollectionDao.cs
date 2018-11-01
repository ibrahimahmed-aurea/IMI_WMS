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
    class HibernateHintCollectionDao : HibernateDaoSupport, IHintCollectionDao
    {

        #region IHintCollectionDao Members

        [Transaction(ReadOnly = true)]
        public HintCollection FindById(Guid hintCollectionId)
        {
            return HibernateTemplate.Get<HintCollection>(hintCollectionId) as HintCollection;
        }

        [Transaction(ReadOnly = true)]
        public IList<HintCollection> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<HintCollection>("HintCollection.FindAll", paramNames, paramValues);
        }

        [Transaction(ReadOnly = false)]
        public HintCollection Save(HintCollection hintCollection)
        {
            if (hintCollection.Id == Guid.Empty) { hintCollection.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(hintCollection);
            return hintCollection;
        }

        [Transaction(ReadOnly = false)]
        public HintCollection SaveOrUpdate(HintCollection hintCollection)
        {
            HibernateTemplate.SaveOrUpdate(hintCollection);
            return hintCollection;
        }

        [Transaction(ReadOnly = false)]
        public HintCollection SaveOrUpdateMerge(HintCollection hintCollection)
        {
            object mergedObj = Session.Merge(hintCollection);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (HintCollection)mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(HintCollection hintCollection)
        {
            HibernateTemplate.Delete(hintCollection);
        }

        #endregion
    }
}
