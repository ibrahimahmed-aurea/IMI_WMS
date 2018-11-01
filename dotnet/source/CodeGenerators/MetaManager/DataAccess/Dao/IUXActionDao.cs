using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IUXActionDao
    {
        UXAction FindById(Guid actionId);
        IList<UXAction> FindAll(Guid applicationId);
        IList<UXAction> FindAllUnmapped(Guid applicationId);
        UXAction Save(UXAction action);
        UXAction SaveOrUpdate(UXAction action);
        UXAction SaveOrUpdateMerge(UXAction action);
        void Delete(UXAction action);
        IList<UXAction> FindAllByDialogId(Guid dialogId);
        IList<UXAction> FindAllByWorkflowId(Guid workflowId);
        IList<UXAction> FindAllByCustomDialogId(Guid customDialogId);
        IList<UXAction> FindAllByServiceMethodId(Guid serviceMethodId);
        IList<UXAction> FindNonUniqueActionsByDialogId(Guid dialogId);
        UXAction FindByPropertyMapId(Guid propertyMapId);
    }
}

