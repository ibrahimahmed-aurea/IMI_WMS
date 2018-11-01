using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using Imi.SupplyChain.Warehouse.Authentication.BusinessEntities;
using Imi.SupplyChain.Warehouse.Authentication.DataAccess;
using System.Configuration;

namespace Imi.SupplyChain.Warehouse.Authentication.BusinessLogic
{
    public class FindUserLogonDetailsAction : MarshalByRefObject
    {
        private const string schemaName = "OWUSER";
        
        public IList<FindUserLogonDetailsResult> Execute(FindUserLogonDetailsParameters parameters)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            string connectionString = settings.ConnectionString;
                        
            IAuthenticationDao dao = new AuthenticationDao(connectionString);

            return dao.FindUserLogonDetails(parameters);
        }

    }

}
