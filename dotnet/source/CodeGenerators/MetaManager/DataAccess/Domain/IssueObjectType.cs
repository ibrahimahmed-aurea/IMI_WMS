using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.DataAccess.Domain
{
    public enum IssueObjectType
    {
        ServiceMethod,
        Dialog,
        StoredProcedure,
        Query,
        DeleteBusinessEntity,
        DeleteProperty,
        DeleteProcedureProperty,
        Bug
    }
}
