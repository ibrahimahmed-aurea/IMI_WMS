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
    public class HibernateUXActionDao : HibernateDaoSupport, IUXActionDao
    {
        #region IUXActionDao Members

        [Transaction(ReadOnly = true)]
        public UXAction FindById(Guid actionId)
        {
            return HibernateTemplate.Get<UXAction>(actionId) as UXAction;
        }

        [Transaction(ReadOnly = true)]
        public IList<UXAction> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<UXAction>("UXAction.FindAll", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<UXAction> FindAllUnmapped(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<UXAction>("UXAction.FindAllUnmappedAction", paramNames, paramValues);
        }

        [Transaction(ReadOnly = false)]
        public UXAction Save(UXAction action)
        {
            if (action.Id == Guid.Empty) { action.Id = Guid.NewGuid(); }
            HibernateTemplate.Save(action);
            return action;
        }

        [Transaction(ReadOnly = false)]
        public UXAction SaveOrUpdate(UXAction action)
        {
            UXAction saveAction = null;

            if (action != null && action.Id != Guid.Empty && !action.IsTransient)
                saveAction = Session.Merge(action) as UXAction;
            else
                saveAction = action;

            HibernateTemplate.SaveOrUpdate(saveAction);

            return saveAction;
        }

        [Transaction(ReadOnly = false)]
        public UXAction SaveOrUpdateMerge(UXAction action)
        {
            UXAction saveAction = null;

            if (action != null && action.Id != Guid.Empty)
                saveAction = Session.Merge(action) as UXAction;
            else
                saveAction = action;

            object mergedObj = Session.Merge(action);
            HibernateTemplate.SaveOrUpdate(mergedObj);

            return (UXAction) mergedObj;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(UXAction action)
        {
            HibernateTemplate.Delete(action);
        }
        

        [Transaction(ReadOnly = true)]
        public IList<UXAction> FindAllByDialogId(Guid dialogId)
        {
            string[] paramNames = { "dialogId" };
            object[] paramValues = { dialogId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<UXAction>("UXAction.FindDialogAction", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<UXAction> FindAllByWorkflowId(Guid workflowId)
        {
            string[] paramNames = { "workflowId" };
            object[] paramValues = { workflowId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<UXAction>("UXAction.FindWorkflowAction", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<UXAction> FindAllByCustomDialogId(Guid customDialogId)
        {
            string[] paramNames = { "customDialogId" };
            object[] paramValues = { customDialogId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<UXAction>("UXAction.FindCustomDialogAction", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<UXAction> FindNonUniqueActionsByDialogId(Guid dialogId)
        {
            string[] paramNames = { "dialogId" };
            object[] paramValues = { dialogId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<UXAction>("UXAction.FindNonUniqueActionsByDialogId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<UXAction> FindAllByServiceMethodId(Guid serviceMethodId)
        {
            string[] paramNames = { "serviceMethodId" };
            object[] paramValues = { serviceMethodId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<UXAction>("UXAction.FindAllByServiceMethodId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public UXAction FindByPropertyMapId(Guid propertyMapId)
        {
            string[] paramNames = { "propertyMapId" };
            object[] paramValues = { propertyMapId };

            IList<UXAction> tmp = HibernateTemplate.FindByNamedQueryAndNamedParam<UXAction>("UXAction.FindByPropertyMapId", paramNames, paramValues);

            if (tmp.Count > 0)
                return tmp[0];
            else
                return null;
        }
       
        #endregion
    }
}
