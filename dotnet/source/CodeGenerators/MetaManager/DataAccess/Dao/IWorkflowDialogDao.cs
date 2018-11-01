using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IWorkflowDialogDao
    {
        WorkflowDialog FindById(Guid workflowDialogId);
        IList<WorkflowDialog> FindAllByDialogId(Guid dialogId);
        IList<WorkflowDialog> FindAll(Guid applicationId);
        WorkflowDialog SaveOrUpdateMerge(WorkflowDialog workflowDialog);
    }
}
