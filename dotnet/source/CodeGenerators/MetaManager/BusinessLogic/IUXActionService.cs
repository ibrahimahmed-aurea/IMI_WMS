using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess;

namespace Cdc.MetaManager.BusinessLogic
{
    public interface IUXActionService
    {
        UXAction GetUXActionByIdWithMap(Guid uxActionId);
        ViewAction AddToView(UXAction action, ViewNode viewNode, ViewActionType viewActionType, MappedProperty mappedProperty);
        IList<UXAction> GetUXActionForMappableObject(IMappableUXObject uxObject);
    }
}
