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
    public class HibernateViewComponentPropertyReferenceDao : HibernateDaoSupport, IViewComponentPropertyReferenceDao
    {
        #region IViewComponentPropertyReferenceDao Members

        public ViewComponentPropertyReference FindById(int viewComponentPropertyReferenceId)
        {
            return HibernateTemplate.Load<ViewComponentPropertyReference>(viewComponentPropertyReferenceId);
        }

        public ViewComponentPropertyReference FindByViewComponentId(int viewComponentId)
        {
            string[] paramNames = { "viewComponentId" };
            object[] paramValues = { viewComponentId };

            IList<ViewComponentPropertyReference> l = HibernateTemplate.FindByNamedQueryAndNamedParam<ViewComponentPropertyReference>("ViewComponentPropertyReference.FindByViewComponentId", paramNames, paramValues);

            if (l.Count == 1)
                return (l[0]);
            else
                return (null);
        }

        public void DeleteByViewId(int viewId)
        {
            string[] paramNames = { "viewId" };
            object[] paramValues = { viewId };

            IList<ViewComponentPropertyReference> l = HibernateTemplate.FindByNamedQueryAndNamedParam <ViewComponentPropertyReference>("ViewComponentPropertyReference.FindByViewId", paramNames, paramValues);

            foreach (ViewComponentPropertyReference vpr in l)
            {
                HibernateTemplate.Delete(vpr);
            }
        }

        public IList<ViewComponentPropertyReference> FindAll()
        {
            return HibernateTemplate.LoadAll<ViewComponentPropertyReference>();
        }

        [Transaction(ReadOnly = false)]
        public ViewComponentPropertyReference Save(ViewComponentPropertyReference viewComponentPropertyReference)
        {
            HibernateTemplate.Save(viewComponentPropertyReference);
            return viewComponentPropertyReference;
        }

        [Transaction(ReadOnly = false)]
        public ViewComponentPropertyReference SaveOrUpdate(ViewComponentPropertyReference viewComponentPropertyReference)
        {
            HibernateTemplate.SaveOrUpdate(viewComponentPropertyReference);
            return viewComponentPropertyReference;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(ViewComponentPropertyReference viewComponentPropertyReference)
        {
            HibernateTemplate.Delete(viewComponentPropertyReference);
        }

        #endregion
    }
}
