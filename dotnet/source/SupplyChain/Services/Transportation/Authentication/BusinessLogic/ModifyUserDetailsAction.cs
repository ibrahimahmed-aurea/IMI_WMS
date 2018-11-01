using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Transportation.Authentication.BusinessEntities;
using Imi.Framework.DataAccess;
using System.Configuration;
using Imi.SupplyChain.Transportation.Authentication.DataAccess;

namespace Imi.SupplyChain.Transportation.Authentication.BusinessLogic
{
    public class ModifyUserDetailsAction : MarshalByRefObject
    {
        private const string schemaName = "RMUSER";

        public void Execute(ModifyUserDetailsParameters parameters)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            string connectionString = settings.ConnectionString;

            IAuthenticationDao dao = new AuthenticationDao(connectionString);
            
            dao.ModifyUserDetails(parameters);
        }

    }
}
