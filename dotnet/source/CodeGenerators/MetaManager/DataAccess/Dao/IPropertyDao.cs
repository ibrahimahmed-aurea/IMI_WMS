using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IPropertyDao
    {
        Property FindById(Guid propertyId);
        IList<Property> FindAll();
        IList<Property> FindAll(Guid applicationId);
        IList<Property> FindByStorageInfoId(Guid storageInfoId);
        IList<Property> FindByBusinessEntityId(Guid businessEntityId);
        IList<Property> FindAllByTableAndColumn(string tableName, string columnName, Guid applicationId);
        Property Save(Property property);
        Property SaveOrUpdate(Property property);
        Property SaveOrUpdateMerge(Property property);
        void Delete(Property property);
    }
}

