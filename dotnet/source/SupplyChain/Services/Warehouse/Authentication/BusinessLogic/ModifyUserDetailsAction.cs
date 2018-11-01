using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Warehouse.Authentication.BusinessEntities;
using Imi.Framework.DataAccess;
using System.Configuration;
using Imi.SupplyChain.Warehouse.Authentication.DataAccess;

namespace Imi.SupplyChain.Warehouse.Authentication.BusinessLogic
{
    public class ModifyUserDetailsAction : MarshalByRefObject
    {
        private const string schemaName = "OWUSER";

        public void Execute(ModifyUserDetailsParameters parameters)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            string connectionString = settings.ConnectionString;

            IAuthenticationDao dao = new AuthenticationDao(connectionString);
            
            dao.ModifyUserDetails(parameters);
        }

    }
}
