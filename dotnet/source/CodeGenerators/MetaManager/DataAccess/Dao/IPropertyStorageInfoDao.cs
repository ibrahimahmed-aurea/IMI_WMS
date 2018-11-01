using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IPropertyStorageInfoDao
    {
        PropertyStorageInfo FindById(Guid propertyStorageInfoId);
        IList<PropertyStorageInfo> FindByTableNameColumnNameApplicationId(string tableName, string columnName, Guid applicationId);
        PropertyStorageInfo Save(PropertyStorageInfo propertyStorageInfo);
        PropertyStorageInfo SaveOrUpdate(PropertyStorageInfo propertyStorageInfo);
        PropertyStorageInfo SaveOrUpdateMerge(PropertyStorageInfo propertyStorageInfo);
        void Delete(PropertyStorageInfo propertyStorageInfo);
    }
}

