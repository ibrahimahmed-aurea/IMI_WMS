using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IPropertyMapDao
    {
        PropertyMap FindById(Guid propertyMapId);
        IList<PropertyMap> FindAll();
        PropertyMap Save(PropertyMap propertyMap);
        PropertyMap SaveOrUpdate(PropertyMap propertyMap);
        PropertyMap SaveOrUpdateMerge(PropertyMap propertyMap);
        void Delete(PropertyMap propertyMap);
    }
}
