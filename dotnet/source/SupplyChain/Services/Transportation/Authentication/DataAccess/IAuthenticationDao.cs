using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Transportation.Authentication.BusinessEntities;

namespace Imi.SupplyChain.Transportation.Authentication.DataAccess
{
    public interface IAuthenticationDao
    {
        IList<FindUserNodesResult> FindAllNodes();
        IList<FindUserNodesResult> FindUserNodes(FindUserNodesParameters parameters);
        IList<FindUserLogonDetailsResult> FindUserLogonDetails(FindUserLogonDetailsParameters parameters);
        void ModifyUserDetails(ModifyUserDetailsParameters parameters);
        LogonResult Logon(LogonParameters parameters);
    }
}
