using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Dao;
using Spring.Data.NHibernate.Generic.Support;
using Cdc.MetaManager.DataAccess.Domain;
using Spring.Transaction.Interceptor;

namespace Cdc.MetaManager.DataAccess.NHibernate.Dao
{
    public class HibernateStoredProcedurePropertyDao : HibernateDaoSupport, IStoredProcedurePropertyDao
    {
        #region IStoredProcedurePropertyDao Members

        [Transaction(ReadOnly = true)]
        public ProcedureProperty FindById(Guid propertyId)
        {
            return HibernateTemplate.Get<ProcedureProperty>(propertyId) as ProcedureProperty;
        }

        [Transaction(ReadOnly = true)]
        public IList<ProcedureProperty> FindAll()
        {
            return HibernateTemplate.LoadAll<ProcedureProperty>();
        }

        [Transaction(ReadOnly = false)]
        public ProcedureProperty Save(ProcedureProperty property)
        {
            if (property.Id == Guid.Empty) { property.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(property);
            return property;
        }

        [Transaction(ReadOnly = false)]
        public ProcedureProperty SaveOrUpdate(ProcedureProperty property)
        {
            HibernateTemplate.SaveOrUpdate(property);
            return property;
        }

        [Transaction(ReadOnly = false)]
        public ProcedureProperty SaveOrUpdateMerge(ProcedureProperty property)
        {
            object mergedObj = Session.Merge(property);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (ProcedureProperty) mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(ProcedureProperty property)
        {
            HibernateTemplate.Delete(property);
        }

        #endregion
    }
}
