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
    public class HibernatePropertyDao : HibernateDaoSupport, IPropertyDao
    {

        #region IPropertyDao Members

        [Transaction(ReadOnly = true)]
        Property IPropertyDao.FindById(Guid propertyId)
        {
            return HibernateTemplate.Get<Property>(propertyId);
        }

        [Transaction(ReadOnly = true)]
        IList<Property> IPropertyDao.FindAll()
        {
            return HibernateTemplate.LoadAll<Property>();
        }

        [Transaction(ReadOnly = true)]
        public IList<Property> FindByStorageInfoId(Guid storageInfoId)
        {
            string[] paramNames = { "storageInfoId" };
            object[] paramValues = { storageInfoId };

            IList<Property> result = HibernateTemplate.FindByNamedQueryAndNamedParam<Property>(
                                 "Property.FindByStorageInfoId",
                                 paramNames, paramValues);

            return result;
        }

        [Transaction(ReadOnly = true)]
        public IList<Property> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Property>(
                                 "Property.FindAll",
                                 paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<Property> FindByBusinessEntityId(Guid businessEntityId)
        {
            string[] paramNames = { "businessEntityId" };
            object[] paramValues = { businessEntityId };

            IList<Property> result = HibernateTemplate.FindByNamedQueryAndNamedParam<Property>(
                                 "Property.FindByBusinessEntityId",
                                 paramNames, paramValues);

            return result;
        }

        [Transaction(ReadOnly = false)]
        public Property Save(Property property)
        {
            if (property.Id == Guid.Empty) { property.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(property);
            return property;
        }

        [Transaction(ReadOnly = false)]
        public Property SaveOrUpdate(Property property)
        {
            HibernateTemplate.SaveOrUpdate(property);
            return property;
        }

        [Transaction(ReadOnly = false)]
        public Property SaveOrUpdateMerge(Property property)
        {
            object mergedObj = Session.Merge(property);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (Property)mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(Property property)
        {
            HibernateTemplate.Delete(property);
        }

        #endregion

        #region IPropertyDao Members

        [Transaction(ReadOnly = true)]
        public IList<Property> FindAllByTableAndColumn(string tableName, string columnName, Guid applicationId)
        {
            if (string.IsNullOrEmpty(tableName))
                tableName = "%";

            if (string.IsNullOrEmpty(columnName))
                columnName = "%";

            string[] paramNames = { "tableName", "columnName", "applicationId" };
            object[] paramValues = { tableName, columnName, applicationId };

            IList<Property> result = HibernateTemplate.FindByNamedQueryAndNamedParam<Property>(
                                 "Property.FindAllByTableAndColumn",
                                 paramNames, paramValues);

            return result;
        }

        #endregion
    }
}
