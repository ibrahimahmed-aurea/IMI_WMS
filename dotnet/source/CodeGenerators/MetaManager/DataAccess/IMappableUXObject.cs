using System;
using System.Collections.Generic;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess
{
    public interface IMappableUXObject : IDomainObject
    {
        string Name { get; set; }
        UXActionType ActionType { get; }
    }
}
