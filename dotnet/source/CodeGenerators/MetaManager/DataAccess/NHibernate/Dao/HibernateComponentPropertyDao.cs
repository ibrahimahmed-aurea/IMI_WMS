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
    public class HibernateComponentPropertyDao : HibernateDaoSupport, IComponentPropertyDao
    {

        public ComponentProperty FindById(Guid componentPropertyId)
        {
            return HibernateTemplate.Get<ComponentProperty>(componentPropertyId) as ComponentProperty;
        }

        public IList<ComponentProperty> FindAll()
        {
            return HibernateTemplate.LoadAll<ComponentProperty>();
        }

        [Transaction(ReadOnly = false)]
        public ComponentProperty Save(ComponentProperty componentProperty)
        {
            if (componentProperty.Id == Guid.Empty) { componentProperty.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(componentProperty);
            return componentProperty;
        }

        [Transaction(ReadOnly = false)]
        public ComponentProperty SaveOrUpdate(ComponentProperty componentProperty)
        {
            HibernateTemplate.SaveOrUpdate(componentProperty);
            return componentProperty;
        }

        [Transaction(ReadOnly = false)]
        public ComponentProperty SaveOrUpdateMerge(ComponentProperty componentProperty)
        {
            object mergedObj = Session.Merge(componentProperty);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (ComponentProperty)mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(ComponentProperty componentProperty)
        {
            HibernateTemplate.Delete(componentProperty);
        }

    }
}
