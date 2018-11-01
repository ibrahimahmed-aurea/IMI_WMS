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
    public class HibernateWorkflowDao : HibernateDaoSupport, IWorkflowDao
    {
        [Transaction(ReadOnly = true)]
        public Workflow FindById(Guid workflowId)
        {
            return HibernateTemplate.Get<Workflow>(workflowId) as Workflow;
        }

        [Transaction(ReadOnly = true)]
        public IList<Workflow> FindAll()
        {
            return HibernateTemplate.LoadAll<Workflow>();
        }

        [Transaction(ReadOnly = true)]
        public IList<Workflow> FindAll(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Workflow>("Workflow.FindAll", paramNames, paramValues);
        }

        [Transaction(ReadOnly = false)]
        public Workflow SaveOrUpdate(Workflow workflow)
        {
            if (workflow.RequestMap != null)
                workflow.RequestMap = Session.Merge(workflow.RequestMap) as PropertyMap;

            HibernateTemplate.SaveOrUpdate(workflow);
            
            return workflow;
        }

        [Transaction(ReadOnly = false)]
        public Workflow SaveOrUpdateMerge(Workflow workflow)
        {
            if (workflow.RequestMap != null)
                workflow.RequestMap = Session.Merge(workflow.RequestMap) as PropertyMap;

            object mergedObj = Session.Merge(workflow);
            HibernateTemplate.SaveOrUpdate(mergedObj);

            return (Workflow) mergedObj;
        }
        
        [Transaction(ReadOnly = false)]
        public void Delete(Workflow workflow)
        {
            HibernateTemplate.Delete(workflow);
        }

        [Transaction(ReadOnly = true)]
        public IList<Workflow> FindWorkflows(Guid applicationId, string moduleName, string workflowName)
        {
            if (string.IsNullOrEmpty(moduleName))
                moduleName = "%";

            if (string.IsNullOrEmpty(workflowName))
                workflowName = "%";

            string[] paramNames = { "applicationId", "moduleName", "workflowName" };
            object[] paramValues = { applicationId, moduleName, workflowName };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Workflow>("Workflow.FindWorkflows", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<Workflow> FindAllByModule(Guid moduleId)
        {
            string[] paramNames = { "moduleId"};
            object[] paramValues = { moduleId};

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Workflow>("Workflow.FindAllByModule", paramNames, paramValues);
        }
    }
}
