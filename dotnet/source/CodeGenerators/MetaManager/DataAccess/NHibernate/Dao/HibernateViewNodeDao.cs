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
    public class HibernateViewNodeDao : HibernateDaoSupport, IViewNodeDao
    {

        [Transaction(ReadOnly = true)]
        public ViewNode FindById(Guid viewNodeId)
        {
            return HibernateTemplate.Get<ViewNode>(viewNodeId);
        }

        [Transaction(ReadOnly = true)]
        public IList<ViewNode> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<ViewNode>("ViewNode.FindAll", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<ViewNode> FindAllByViewId(Guid viewId)
        {
            string[] paramNames = { "viewId" };
            object[] paramValues = { viewId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<ViewNode>("ViewNode.FindAllByViewId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<ViewNode> FindAllByDialogId(Guid dialogId)
        {
            string[] paramNames = { "dialogId" };
            object[] paramValues = { dialogId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<ViewNode>("ViewNode.FindAllByDialogId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public long CountByViewId(Guid viewId)
        {
            string[] paramNames = { "viewId" };
            object[] paramValues = { viewId };

            IList<long> l = HibernateTemplate.FindByNamedQueryAndNamedParam<long>("ViewNode.CountByViewId", paramNames, paramValues);

            return l[0];
        }


        [Transaction(ReadOnly = false)]
        public ViewNode Save(ViewNode viewNode)
        {
            if (viewNode.Id == Guid.Empty) { viewNode.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(viewNode);
            return viewNode;
        }

        [Transaction(ReadOnly = false)]
        public ViewNode SaveOrUpdate(ViewNode viewNode)
        {
            HibernateTemplate.SaveOrUpdate(viewNode);
            return viewNode;
        }

        [Transaction(ReadOnly = false)]
        public ViewNode SaveOrUpdateMerge(ViewNode viewNode)
        {
            object mergedObj = Session.Merge(viewNode);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (ViewNode) mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(ViewNode viewNode)
        {
            HibernateTemplate.Delete(viewNode);
        }

        [Transaction(ReadOnly = true)]
        public ViewNode FindByPropertyMapId(Guid propertyMapId)
        {
            string[] paramNames = { "propertyMapId" };
            object[] paramValues = { propertyMapId };

            IList<ViewNode> tmp = HibernateTemplate.FindByNamedQueryAndNamedParam<ViewNode>("ViewNode.FindByPropertyMapId", paramNames, paramValues);

            if (tmp.Count > 0)
                return tmp[0];
            else
                return null;
        }

    }
}
