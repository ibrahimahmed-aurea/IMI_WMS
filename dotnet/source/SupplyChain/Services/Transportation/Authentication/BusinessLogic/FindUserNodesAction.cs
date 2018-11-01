﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.DataAccess;
using Imi.SupplyChain.Transportation.Authentication.BusinessEntities;
using Imi.SupplyChain.Transportation.Authentication.DataAccess;
using System.Configuration;

namespace Imi.SupplyChain.Transportation.Authentication.BusinessLogic
{
    public class FindUserNodesAction : MarshalByRefObject
    {
        private const string schemaName = "RMUSER";
        
        public IList<FindUserNodesResult> Execute(FindUserNodesParameters parameters)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[schemaName];
            string connectionString = settings.ConnectionString;

            bool isAdministratorModeEnabled = false;
            bool.TryParse(ConfigurationManager.AppSettings["AdministratorMode"], out isAdministratorModeEnabled);

            IAuthenticationDao dao = new AuthenticationDao(connectionString);

            if (isAdministratorModeEnabled)
            {
                return dao.FindAllNodes();
            }
            else
            {
                return dao.FindUserNodes(parameters);
            }
        }

    }

}