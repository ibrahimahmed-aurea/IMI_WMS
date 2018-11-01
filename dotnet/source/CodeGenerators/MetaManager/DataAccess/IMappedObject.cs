using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess
{
    public interface IMappedObject
    {
        Guid GetRequestMapId(IDomainObject connectedToObject, out SetTargetChoice setTarget);
        Guid GetResponseMapId(IDomainObject connectedToObject, out SetTargetChoice setTarget);
    }
}
