using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IWorkflowSubworkflowDao
    {
        WorkflowSubworkflow FindById(Guid workflowSubworkflowId);
        IList<WorkflowSubworkflow> FindAllByWorkflowId(Guid workflowId);
        WorkflowSubworkflow SaveOrUpdateMerge(WorkflowSubworkflow workflowSubworkflow);
    }
}
