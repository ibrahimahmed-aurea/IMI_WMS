using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using Imi.SupplyChain.OutputManager.Initialization.BusinessEntities;
using Imi.SupplyChain.OutputManager.Initialization.DataAccess;
using System.Configuration;

namespace Imi.SupplyChain.OutputManager.Initialization.BusinessLogic
{
    public class FindOutputManagersAction : MarshalByRefObject
    {
        private const string schemaName = "OMUSER";
        
        public FindOutputManagersResult Execute()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            string connectionString = settings.ConnectionString;

            FindOutputManagersResult result = new FindOutputManagersResult();


            bool isAdministratorModeEnabled = false;
            bool.TryParse(ConfigurationManager.AppSettings["AdministratorMode"], out isAdministratorModeEnabled);

            IInitializationDao dao = new InitializationDao(connectionString);

            if (isAdministratorModeEnabled)
            {
                result.OutputManagers = dao.FindAllOutputManagers();
            }
            else
            {
                result.OutputManagers = dao.FindAllOutputManagers();
            }

            return result;
        }

    }

}
