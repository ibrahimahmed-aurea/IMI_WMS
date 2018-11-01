using System;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public interface IMappedPropertyHelper
    {
        DbProperty GetOrigin(MappedProperty mappedProperty);
    }
}
