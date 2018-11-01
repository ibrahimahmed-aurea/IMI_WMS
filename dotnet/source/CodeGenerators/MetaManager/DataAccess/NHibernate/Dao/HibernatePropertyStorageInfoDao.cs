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
    public class HibernatePropertyStorageInfoDao : HibernateDaoSupport, IPropertyStorageInfoDao
    {
        #region IPropertyStorageInfoDao Members

        [Transaction(ReadOnly = true)]
        public PropertyStorageInfo FindById(Guid propertyStorageInfoId)
        {
            return HibernateTemplate.Get<PropertyStorageInfo>(propertyStorageInfoId) as PropertyStorageInfo;
        }

        [Transaction(ReadOnly = true)]
        public IList<PropertyStorageInfo> FindByTableNameColumnNameApplicationId(string tableName, string columnName, Guid applicationId)
        {
            string[] paramNames = { "tableName", "columnName", "applicationId" };
            object[] paramValues = { tableName, columnName, applicationId };

            IList<PropertyStorageInfo> result = HibernateTemplate.FindByNamedQueryAndNamedParam<PropertyStorageInfo>(
                                 "PropertyStorageInfo.FindByTableNameColumnNameAppId",
                                 paramNames, paramValues);

            return result;
        }

        [Transaction(ReadOnly = false)]
        public PropertyStorageInfo Save(PropertyStorageInfo propertyStorageInfo)
        {
            if (propertyStorageInfo.Id == Guid.Empty) { propertyStorageInfo.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(propertyStorageInfo);
            return propertyStorageInfo;
        }

        [Transaction(ReadOnly = false)]
        public PropertyStorageInfo SaveOrUpdate(PropertyStorageInfo propertyStorageInfo)
        {
            HibernateTemplate.SaveOrUpdate(propertyStorageInfo);
            return propertyStorageInfo;
        }

        [Transaction(ReadOnly = false)]
        public PropertyStorageInfo SaveOrUpdateMerge(PropertyStorageInfo propertyStorageInfo)
        {
            object mergedObj = Session.Merge(propertyStorageInfo);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (PropertyStorageInfo) mergedObj;
        }

        

        [Transaction(ReadOnly = false)]
        public void Delete(PropertyStorageInfo propertyStorageInfo)
        {
            HibernateTemplate.Delete(propertyStorageInfo);
        }


        #endregion


    }
}
