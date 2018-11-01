using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Warehouse.Authentication.BusinessEntities;
using System.Configuration;
using Imi.SupplyChain.Warehouse.Authentication.DataAccess;

namespace Imi.SupplyChain.Warehouse.Authentication.BusinessLogic
{
    public class FindUserCompaniesAction : MarshalByRefObject
    {
        private const string schemaName = "OWUSER";

        public IList<FindUserCompaniesResult> Execute(FindUserCompaniesParameters parameters)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            string connectionString = settings.ConnectionString;

            bool isAdministratorModeEnabled = false;
            bool.TryParse(ConfigurationManager.AppSettings["AdministratorMode"], out isAdministratorModeEnabled);

            IAuthenticationDao dao = new AuthenticationDao(connectionString);

            if (isAdministratorModeEnabled)
            {
                return dao.FindAllCompanies();
            }
            else
            {
                return dao.FindUserCompanies(parameters);
            }
        }

    }
}
