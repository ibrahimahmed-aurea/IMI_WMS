using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Spring.Data.NHibernate.Generic.Support;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess.Domain;
using NHibernate;

namespace Cdc.MetaManager.DataAccess.NHibernate.Dao
{
    public class HibernateStoredProcedureDao : HibernateDaoSupport, IStoredProcedureDao
    {
        #region IStoredProcedureDao Members

        [Transaction(ReadOnly = true)]
        public StoredProcedure FindById(Guid storedProcedureId)
        {
            return HibernateTemplate.Get<StoredProcedure>(storedProcedureId);
        }

        [Transaction(ReadOnly = true)]
        public IList<StoredProcedure> FindAllByPackageId(Guid packageId)
        {
            string[] paramNames = { "packageId" };
            object[] paramValues = { packageId };

            IList<StoredProcedure> storedProcedures = HibernateTemplate.FindByNamedQueryAndNamedParam<StoredProcedure>(
                                                                             "StoredProcedure.FindAllByPackageId",
                                                                             paramNames, paramValues);

            return storedProcedures;
        }

        [Transaction(ReadOnly = true)]
        public StoredProcedure FindByProcedurePropertyId(Guid procedurePropertyId)
        {
            string[] paramNames = { "procedurePropertyId" };
            object[] paramValues = { procedurePropertyId };

            IList<StoredProcedure> list = HibernateTemplate.FindByNamedQueryAndNamedParam<StoredProcedure>(
                                                 "StoredProcedure.FindByProcedurePropertyId",
                                                 paramNames, paramValues);

            if (list.Count == 1)
                return list[0];
            else
                return null;
        }

        [Transaction(ReadOnly = false)]
        public StoredProcedure Save(StoredProcedure storedProcedure)
        {
            if (storedProcedure.Id == Guid.Empty) { storedProcedure.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(storedProcedure);
            return storedProcedure;
        }

        [Transaction(ReadOnly = false)]
        public StoredProcedure SaveOrUpdate(StoredProcedure storedProcedure)
        {
            HibernateTemplate.SaveOrUpdate(storedProcedure);
            return storedProcedure;
        }

        [Transaction(ReadOnly = false)]
        public StoredProcedure SaveOrUpdateMerge(StoredProcedure storedProcedure)
        {
            object mergedObj = Session.Merge(storedProcedure);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (StoredProcedure) mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(StoredProcedure storedProcedure)
        {
            HibernateTemplate.Delete(storedProcedure);
        }


        #endregion
    }
}
