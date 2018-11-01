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
    public class HibernateWorkflowSubworkflowDao : HibernateDaoSupport, IWorkflowSubworkflowDao
    {
        [Transaction(ReadOnly = true)]
        public WorkflowSubworkflow FindById(Guid workflowSubworkflowId)
        {
            return HibernateTemplate.Get<WorkflowSubworkflow>(workflowSubworkflowId);
        }

        [Transaction(ReadOnly = true)]
        public IList<WorkflowSubworkflow> FindAllByWorkflowId(Guid workflowId)
        {
            string[] paramNames = { "workflowId" };
            object[] paramValues = { workflowId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<WorkflowSubworkflow>("WorkflowSubworkflow.FindAllByWorkflowId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = false)]
        public WorkflowSubworkflow SaveOrUpdateMerge(WorkflowSubworkflow workflowSubworkflow)
        {
            object mergedObj = Session.Merge(workflowSubworkflow);
            HibernateTemplate.SaveOrUpdate(mergedObj);
            return (WorkflowSubworkflow) mergedObj;
        }
    }
}
