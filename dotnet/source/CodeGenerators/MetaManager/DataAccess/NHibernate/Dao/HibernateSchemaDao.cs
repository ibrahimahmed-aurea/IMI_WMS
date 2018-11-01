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
    public class HibernateSchemaDao : HibernateDaoSupport, ISchemaDao
    {
        #region ISchemaDao Members

        [Transaction(ReadOnly = true)]
        public Schema FindById(Guid schemaId)
        {
            return HibernateTemplate.Get<Schema>(schemaId);
        }

        [Transaction(ReadOnly = true)]
        public Schema FindByApplicationId(Guid applicationId)
        {
            string[] paramNames = { "applicationId"};
            object[] paramValues = { applicationId};

            IList<Schema> l = HibernateTemplate.FindByNamedQueryAndNamedParam<Schema>(
                                          "Schema.FindByApplicationId",
                                          paramNames, paramValues);

            if (l.Count == 1)
                return (l[0]);
            else
                return (null);
        }

        [Transaction(ReadOnly = true)]
        public IList<Schema> FindAll()
        {
            return HibernateTemplate.LoadAll<Schema>();
        }

        [Transaction(ReadOnly = false)]
        public Schema Save(Schema schema)
        {
            if (schema.Id == Guid.Empty) { schema.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(schema);
            return schema;
        }

        [Transaction(ReadOnly = false)]
        public Schema SaveOrUpdate(Schema schema)
        {
            HibernateTemplate.SaveOrUpdate(schema);
            return schema;
        }

        [Transaction(ReadOnly = false)]
        public Schema SaveOrUpdateMerge(Schema schema)
        {
            object mergedObj = Session.Merge(schema);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (Schema) mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(Schema schema)
        {
            HibernateTemplate.Delete(schema);
        }


        #endregion
    }
}
