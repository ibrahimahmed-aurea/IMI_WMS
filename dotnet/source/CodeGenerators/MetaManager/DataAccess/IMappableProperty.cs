using System;
using System.Collections.Generic;
using System.Text;

namespace Cdc.MetaManager.DataAccess
{
    public interface IMappableProperty : IDomainObject
    {
        string Name { get; set; }
        Type Type { get; }
        
    }
}
