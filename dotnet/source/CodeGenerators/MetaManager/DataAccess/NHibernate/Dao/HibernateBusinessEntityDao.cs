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
    public class HibernateBusinessEntityDao : HibernateDaoSupport, IBusinessEntityDao
    {
        #region IBusinessEntityDao Members

        [Transaction(ReadOnly = true)]
        public BusinessEntity FindById(Guid businessEntityId)
        {
            return HibernateTemplate.Get<BusinessEntity>(businessEntityId) as BusinessEntity;
        }

        [Transaction(ReadOnly = true)]
        public IList<BusinessEntity> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<BusinessEntity>("BusinessEntity.FindAll", paramNames, paramValues);
        }

        [Transaction(ReadOnly = false)]
        public BusinessEntity Save(BusinessEntity businessEntity)
        {
            if (businessEntity.Id == Guid.Empty) { businessEntity.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(businessEntity);
            return businessEntity;
        }

        [Transaction(ReadOnly = false)]
        public BusinessEntity SaveOrUpdate(BusinessEntity businessEntity)
        {
            HibernateTemplate.SaveOrUpdate(businessEntity);
            return businessEntity;
        }

        [Transaction(ReadOnly = false)]
        public BusinessEntity SaveOrUpdateMerge(BusinessEntity businessEntity)
        {
            object mergedObj = Session.Merge(businessEntity);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (BusinessEntity)mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(BusinessEntity businessEntity)
        {
            HibernateTemplate.Delete(businessEntity);
        }


        #endregion
    }
}
