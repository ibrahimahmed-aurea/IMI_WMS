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
    public class HibernateWorkflowDialogDao : HibernateDaoSupport, IWorkflowDialogDao
    {
        [Transaction(ReadOnly = true)]
        public WorkflowDialog FindById(Guid workflowDialogId)
        {
            return HibernateTemplate.Get<WorkflowDialog>(workflowDialogId) as WorkflowDialog;
        }

        [Transaction(ReadOnly = true)]
        public IList<WorkflowDialog> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<WorkflowDialog>("WorkflowDialog.FindAll", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<WorkflowDialog> FindAllByDialogId(Guid dialogId)
        {
            string[] paramNames = { "dialogId" };
            object[] paramValues = { dialogId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<WorkflowDialog>("WorkflowDialog.FindAllByDialogId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = false)]
        public WorkflowDialog SaveOrUpdateMerge(WorkflowDialog workflowDialog)
        {
            object mergedObj = Session.Merge(workflowDialog);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (WorkflowDialog) mergedObj;
        }
    }
}
