using System;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public interface IPropertyMapHelper
    {
        void RemoveMappedProperty(MappedProperty mappedProperty);
    }
}
