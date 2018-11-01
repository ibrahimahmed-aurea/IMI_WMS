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
    public class HibernateViewDao : HibernateDaoSupport, IViewDao
    {
        #region IViewDao Members

        [Transaction(ReadOnly = true)]
        public View FindById(Guid viewId)
        {            
            return HibernateTemplate.Get<View>(viewId);
        }
        

        [Transaction(ReadOnly = true)]
        public View FindByPropertyMapId(Guid propertyMapId)
        {
            string[] paramNames = { "propertyMapId" };
            object[] paramValues = { propertyMapId };

            IList<View> tmp = HibernateTemplate.FindByNamedQueryAndNamedParam<View>("View.FindByPropertyMapId", paramNames, paramValues);

            if (tmp.Count > 0)
                return tmp[0];
            else
                return null;
        }

        [Transaction(ReadOnly = true)]
        public IList<View> FindAll()
        {
            return HibernateTemplate.LoadAll<View>();
        }

        [Transaction(ReadOnly = false)]
        public View Save(View view)
        {
            if (view.Id == Guid.Empty) { view.Id = Guid.NewGuid(); }
            view.FlushVisualTree();
            HibernateTemplate.Save(view);
            return view;
        }

        [Transaction(ReadOnly = false)]
        public View SaveOrUpdate(View view)
        {
            view.FlushVisualTree();
            HibernateTemplate.SaveOrUpdate(view);
            
            return view;
        }

        [Transaction(ReadOnly = false)]
        public View SaveOrUpdateMerge(View view)
        {
            view.FlushVisualTree();
            object mergedObj = Session.Merge(view);
            HibernateTemplate.SaveOrUpdate(mergedObj);

            return (View) mergedObj;
        }
        
        [Transaction(ReadOnly = false)]
        public void Delete(View view)
        {
            HibernateTemplate.Delete(view);
        }

        [Transaction(ReadOnly = true)]
        public IList<View> FindByEntityAndName(string entityName, string viewName, Guid applicationId)
        {
            if (string.IsNullOrEmpty(entityName))
                entityName = "%";

            if (string.IsNullOrEmpty(viewName))
                viewName = "%";

            string[] paramNames = { "entityName", "viewName", "applicationId" };
            object[] paramValues = { entityName, viewName, applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<View>("View.FindByEntityAndName", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<View> FindViews(string entityName, string viewName, string title, FindViewTypes findViewTypes, Guid applicationId)
        {
            if (string.IsNullOrEmpty(entityName))
                entityName = "%";

            if (string.IsNullOrEmpty(viewName))
                viewName = "%";

            if (string.IsNullOrEmpty(title))
                title = "%";

            if (findViewTypes == FindViewTypes.All)
            {
                string[] paramNames = { "entityName", "viewName", "title", "applicationId" };
                object[] paramValues = { entityName, viewName, title, applicationId };

                return HibernateTemplate.FindByNamedQueryAndNamedParam<View>("View.FindView", paramNames, paramValues);
            }
            else if (findViewTypes == FindViewTypes.Custom)
            {
                string[] paramNames = { "entityName", "viewName", "title", "applicationId" };
                object[] paramValues = { entityName, viewName, title, applicationId };

                return HibernateTemplate.FindByNamedQueryAndNamedParam<View>("View.FindViewCustom", paramNames, paramValues);
            }
            else if (findViewTypes == FindViewTypes.Drilldowns)
            {
                string[] paramNames = { "entityName", "viewName", "title", "applicationId", "dialogType" };
                object[] paramValues = { entityName, viewName, title, applicationId, (int)DialogType.Drilldown};

                return HibernateTemplate.FindByNamedQueryAndNamedParam<View>("View.FindViewOverviews", paramNames, paramValues);
            }
            return null;
        }

        [Transaction(ReadOnly = true)]
        public IList<string> FindAllUniqueCustomDLLNames(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            IList<string> foundViews = HibernateTemplate.FindByNamedQueryAndNamedParam<string>("View.FindAllUniqueCustomDLLNames", paramNames, paramValues);

            // Remove null values in found viewlist
            foundViews.Remove(null);
            
            return foundViews;
        }

        [Transaction(ReadOnly = true)]
        public IList<View> FindAllByServiceMethodId(Guid serviceMethodId)
        {
            string[] paramNames = { "serviceMethodId" };
            object[] paramValues = { serviceMethodId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<View>("View.FindAllByServiceMethodId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<View> FindByBusinessEntityId(Guid businessEntityId)
        {
            string[] paramNames = { "businessEntityId" };
            object[] paramValues = { businessEntityId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<View>("View.FindByBusinessEntityId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<View> FindByNameAndApplicationId(string viewName, Guid applicationId)
        {
            string[] paramNames = { "viewName", "applicationId" };
            object[] paramValues = { viewName, applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<View>("View.FindByNameAndApplicationId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<View> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<View>("View.FindAllByApplication", paramNames, paramValues);
        }

        #endregion
    }
}
