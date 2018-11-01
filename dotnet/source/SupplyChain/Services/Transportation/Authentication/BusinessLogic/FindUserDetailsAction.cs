using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using Imi.SupplyChain.Transportation.Authentication.BusinessEntities;
using System.Threading;
using System.Configuration;
using Imi.SupplyChain.Transportation.Authentication.DataAccess;
          
namespace Imi.SupplyChain.Transportation.Authentication.BusinessLogic
{
    public class FindUserDetailsAction : MarshalByRefObject
    {
        private const string schemaName = "RMUSER";

        public FindUserDetailsResult Execute(FindUserDetailsParameters parameters)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            string connectionString = settings.ConnectionString;

            FindUserDetailsResult result = null;
            string _userIdentity = string.Empty;
                        
            using (TransactionScope scope = new TransactionScope())
            {
                FindUserLogonDetailsAction findUserLogonDetailsAction = new FindUserLogonDetailsAction();
                IList<FindUserLogonDetailsResult> findUserLogonDetailsResult = findUserLogonDetailsAction.Execute(new FindUserLogonDetailsParameters() { UserIdentity = parameters.UserIdentity, UserPrincipalIdentity = parameters.UserPrincipalIdentity });

                if ((findUserLogonDetailsResult != null) && (findUserLogonDetailsResult.Count > 0))
                {
                    _userIdentity = findUserLogonDetailsResult[0].UserIdentity;
                }
                else
                {
                    _userIdentity = parameters.UserIdentity;
                }

                FindUserNodesAction findUserNodesAction = new FindUserNodesAction();
                IList<FindUserNodesResult> findUserNodesResult = findUserNodesAction.Execute(new FindUserNodesParameters() { UserIdentity = _userIdentity });

                result = new FindUserDetailsResult() { UserIdentity = _userIdentity };

                if ((findUserLogonDetailsResult != null) && (findUserLogonDetailsResult.Count > 0))
                {
                    result.UserName = findUserLogonDetailsResult[0].UserName;
                    result.RecentNodeIdentity = findUserLogonDetailsResult[0].RecentNodeIdentity;
                    result.LastLogonTime = findUserLogonDetailsResult[0].LastLogonTime;
                }
                else
                {
                    // Get identity from the current thread
                    result.UserName = Thread.CurrentPrincipal.Identity.Name;
                    if (string.IsNullOrEmpty(result.UserName))
                        result.UserName = _userIdentity;
                    result.LastLogonTime = DateTime.Now;
                }

                result.UserNodes = findUserNodesResult;

                scope.Complete();

                return result;
            }

        }
    }
}
