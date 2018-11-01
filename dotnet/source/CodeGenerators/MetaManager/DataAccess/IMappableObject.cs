using System;
using System.Collections.Generic;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess
{
    public interface IMappableObject : IDomainObject
    {
        ActionMapTarget ObjectType { get; }
    }
}
