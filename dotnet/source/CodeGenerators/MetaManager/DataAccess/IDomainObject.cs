using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.DataAccess
{
    public interface IDomainObject : System.IEquatable<IDomainObject>
    {
        Guid Id { get; set; }
        bool IsTransient { get; set; }
    }
}
