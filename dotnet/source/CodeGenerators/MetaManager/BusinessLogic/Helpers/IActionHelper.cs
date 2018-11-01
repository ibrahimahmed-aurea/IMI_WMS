using System;
using System.Collections.Generic;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public interface IActionHelper
    {
        Cdc.MetaManager.DataAccess.Domain.Action SaveAndSynchronize(Cdc.MetaManager.DataAccess.Domain.Action action, Dictionary<PropertyMap, List<MappedProperty>> mappedPropertyToDelete);
    }
}
