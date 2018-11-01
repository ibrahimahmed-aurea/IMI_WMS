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
    class HibernateWorkflowServiceMethodDao : HibernateDaoSupport, IWorkflowServiceMethodDao
    {
        [Transaction(ReadOnly = true)]
        public IList<WorkflowServiceMethod> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<WorkflowServiceMethod>("WorkflowServiceMethod.FindAll", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public WorkflowServiceMethod FindById(Guid workflowServiceMethodId)
        {
            return HibernateTemplate.Get<WorkflowServiceMethod>(workflowServiceMethodId);
        }
    }
}
