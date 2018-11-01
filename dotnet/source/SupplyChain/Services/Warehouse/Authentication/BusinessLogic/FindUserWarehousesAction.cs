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
    public class FindUserWarehousesAction : MarshalByRefObject
    {
        private const string schemaName = "OWUSER";
        
        public IList<FindUserWarehousesResult> Execute(FindUserWarehousesParameters parameters)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            string connectionString = settings.ConnectionString;

            bool isAdministratorModeEnabled = false;
            bool.TryParse(ConfigurationManager.AppSettings["AdministratorMode"], out isAdministratorModeEnabled);

            IAuthenticationDao dao = new AuthenticationDao(connectionString);

            if (isAdministratorModeEnabled)
            {
                return dao.FindAllWarehouses();
            }
            else
            {
                return dao.FindUserWarehouses(parameters);
            }
                
        }

    }

}
