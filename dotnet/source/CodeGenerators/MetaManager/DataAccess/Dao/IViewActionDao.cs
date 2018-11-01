using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IViewActionDao
    {
        ViewAction FindById(Guid viewActionId);
        IList<ViewAction> FindAll(Guid applicationId);
        ViewAction Save(ViewAction viewAction);
        ViewAction SaveOrUpdate(ViewAction viewAction);
        ViewAction SaveOrUpdateMerge(ViewAction viewAction);
        void Delete(ViewAction viewAction);
        IList<ViewAction> FindAllByActionId(Guid actionId);
        IList<ViewAction> FindAllByViewNodeId(Guid viewNodeId);
        IList<ViewAction> FindAllByDialogId(Guid dialogId);
        IList<ViewAction> FindAllByActionIdAndViewNodeId(Guid actionId, Guid viewNodeId);
        IList<ViewAction> FindByMappedPropertyId(Guid mappedPropertyId);
        ViewAction FindByPropertyMapId(Guid propertyMapId);
    }
}
