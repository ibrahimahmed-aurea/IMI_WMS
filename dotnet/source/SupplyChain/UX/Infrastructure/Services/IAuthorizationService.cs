using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX.Infrastructure.Services
{
    /// <summary>
    ///     Authorization Service Interface - Used for authorizing a list of operations.
    /// </summary>
    public interface IAuthorizationService
    {
        bool IsAuthorized(string operation);

        void CheckAuthorization(ICollection<IAuthOperation> operations);
        bool CheckAuthorization(string applicationName, IEnumerable<IAuthOperation> operations);
    }
}
