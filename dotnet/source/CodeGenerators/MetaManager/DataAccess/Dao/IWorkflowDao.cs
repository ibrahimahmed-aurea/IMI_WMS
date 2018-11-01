using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IWorkflowDao
    {
        Workflow FindById(Guid workflowId);
        IList<Workflow> FindAll();
        IList<Workflow> FindAll(Guid applicationId);
        Workflow SaveOrUpdate(Workflow workflow);
        Workflow SaveOrUpdateMerge(Workflow workflow);
        void Delete(Workflow workflow);
        IList<Workflow> FindWorkflows(Guid applicationId, string moduleName, string workflowName);
        IList<Workflow> FindAllByModule(Guid moduleID);
    }
}
