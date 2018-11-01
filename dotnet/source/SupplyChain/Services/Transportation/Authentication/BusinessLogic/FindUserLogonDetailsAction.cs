using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using Imi.SupplyChain.Transportation.Authentication.BusinessEntities;
using Imi.SupplyChain.Transportation.Authentication.DataAccess;
using System.Configuration;

namespace Imi.SupplyChain.Transportation.Authentication.BusinessLogic
{
    public class FindUserLogonDetailsAction : MarshalByRefObject
    {
        private const string schemaName = "RMUSER";
        
        public IList<FindUserLogonDetailsResult> Execute(FindUserLogonDetailsParameters parameters)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            string connectionString = settings.ConnectionString;
                        
            IAuthenticationDao dao = new AuthenticationDao(connectionString);

            return dao.FindUserLogonDetails(parameters);
        }

    }

}
