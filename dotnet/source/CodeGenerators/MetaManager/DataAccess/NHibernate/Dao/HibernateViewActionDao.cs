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
    public class HibernateViewActionDao : HibernateDaoSupport, IViewActionDao
    {

        [Transaction(ReadOnly = true)]
        public ViewAction FindById(Guid viewActionId)
        {
            return HibernateTemplate.Get<ViewAction>(viewActionId);
        }

        [Transaction(ReadOnly = true)]
        public IList<ViewAction> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<ViewAction>("ViewAction.FindAll", paramNames, paramValues);
        }

        [Transaction(ReadOnly = false)]
        public ViewAction Save(ViewAction viewAction)
        {
            if (viewAction.Id == Guid.Empty) { viewAction.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(viewAction);
            return viewAction;
        }

        [Transaction(ReadOnly = false)]
        public ViewAction SaveOrUpdate(ViewAction viewAction)
        {
            HibernateTemplate.SaveOrUpdate(viewAction);
            return viewAction;
        }

        [Transaction(ReadOnly = false)]
        public ViewAction SaveOrUpdateMerge(ViewAction viewAction)
        {
            object mergedObj = Session.Merge(viewAction);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (ViewAction) mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(ViewAction viewAction)
        {
            HibernateTemplate.Delete(viewAction);
        }

        [Transaction(ReadOnly = true)]
        public IList<ViewAction> FindAllByActionId(Guid actionId)
        {
            string[] paramNames = { "actionId" };
            object[] paramValues = { actionId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<ViewAction>("ViewAction.FindAllByActionId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<ViewAction> FindAllByViewNodeId(Guid viewNodeId)
        {
            string[] paramNames = { "viewNodeId" };
            object[] paramValues = { viewNodeId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<ViewAction>("ViewAction.FindAllByViewNodeId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<ViewAction> FindAllByDialogId(Guid dialogId)
        {
            string[] paramNames = { "dialogId" };
            object[] paramValues = { dialogId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<ViewAction>("ViewAction.FindAllByDialogId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<ViewAction> FindByMappedPropertyId(Guid mappedPropertyId)
        {
            string[] paramNames = { "mappedPropertyId" };
            object[] paramValues = { mappedPropertyId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<ViewAction>("ViewAction.FindByMappedPropertyId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<ViewAction> FindAllByActionIdAndViewNodeId(Guid actionId, Guid viewNodeId)
        {
            string[] paramNames = { "actionId", "viewNodeId" };
            object[] paramValues = { actionId, viewNodeId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<ViewAction>("ViewAction.FindAllByActionIdAndViewNodeId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public ViewAction FindByPropertyMapId(Guid propertyMapId)
        {
            string[] paramNames = { "propertyMapId" };
            object[] paramValues = { propertyMapId };

            IList<ViewAction> tmp = HibernateTemplate.FindByNamedQueryAndNamedParam<ViewAction>("ViewAction.FindByPropertyMapId", paramNames, paramValues);

            if (tmp.Count > 0)
                return tmp[0];
            else
                return null;
        }

    }
}
