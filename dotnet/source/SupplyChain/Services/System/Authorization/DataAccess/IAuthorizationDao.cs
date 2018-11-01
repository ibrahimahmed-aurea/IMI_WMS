using System;
using System.Collections.Generic;
using Imi.SupplyChain.Authorization.BusinessEntities;

namespace Imi.SupplyChain.Authorization.DataAccess
{
    public interface IAuthorizationDao
    {
        void Authorize(IList<AuthOperation> operations, IList<string> roles, string authorizationProvider);
    }
}
