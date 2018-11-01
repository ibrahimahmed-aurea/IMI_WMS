using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Data.NHibernate.Generic.Support;
using Cdc.MetaManager.DataAccess.Dao;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.NHibernate.Dao
{
    public class HibernatePropertyMapDao : HibernateDaoSupport, IPropertyMapDao
    {
        [Transaction(ReadOnly = true)]
        public PropertyMap FindById(Guid propertyMapId)
        {
            return HibernateTemplate.Get<PropertyMap>(propertyMapId);
        }

        [Transaction(ReadOnly = true)]
        public IList<PropertyMap> FindAll()
        {
            return HibernateTemplate.LoadAll<PropertyMap>();
        }

        [Transaction(ReadOnly = false)]
        public PropertyMap Save(PropertyMap propertyMap)
        {
            if (propertyMap.Id == Guid.Empty) { propertyMap.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(propertyMap);
            return propertyMap;
        }

        [Transaction(ReadOnly = false)]
        public PropertyMap SaveOrUpdate(PropertyMap propertyMap)
        {
            HibernateTemplate.SaveOrUpdate(propertyMap);
            return propertyMap;
        }

        [Transaction(ReadOnly = false)]
        public PropertyMap SaveOrUpdateMerge(PropertyMap propertyMap)
        {
            PropertyMap map = Session.Merge(propertyMap) as PropertyMap;            
            HibernateTemplate.SaveOrUpdate(map);
            return map;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(PropertyMap propertyMap)
        {
            HibernateTemplate.Delete(propertyMap);
        }

    }
}
