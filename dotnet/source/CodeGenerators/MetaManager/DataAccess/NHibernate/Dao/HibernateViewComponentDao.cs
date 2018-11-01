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
    public class HibernateViewComponentDao : HibernateDaoSupport, IViewComponentDao
    {
        #region IViewComponentDao Members

        public ViewComponent FindById(Guid viewComponentId)
        {
            return HibernateTemplate.Get<ViewComponent>(viewComponentId);
        }

        public IList<ViewComponent> FindAll()
        {
            return HibernateTemplate.LoadAll<ViewComponent>();
        }

        [Transaction(ReadOnly = false)]
        public ViewComponent Save(ViewComponent viewComponent)
        {
            if (viewComponent.Id == Guid.Empty) { viewComponent.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(viewComponent);
            return viewComponent;
        }

        [Transaction(ReadOnly = false)]
        public ViewComponent SaveOrUpdate(ViewComponent viewComponent)
        {
            HibernateTemplate.SaveOrUpdate(viewComponent);
            
            return viewComponent;
        }

        [Transaction(ReadOnly = false)]
        public ViewComponent SaveOrUpdateMerge(ViewComponent viewComponent)
        {
            object mergedObj = Session.Merge(viewComponent);
            HibernateTemplate.SaveOrUpdate(mergedObj);

            return (ViewComponent) mergedObj;
        }
        
        [Transaction(ReadOnly = false)]
        public void Delete(ViewComponent viewComponent)
        {
            HibernateTemplate.Delete(viewComponent);
        }

        #endregion
    }
}
