using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Authorization.BusinessEntities;
using Imi.SupplyChain.Authorization.DataAccess;
using System.Configuration;

namespace Imi.SupplyChain.Authorization.BusinessLogic
{
    public class CheckAuthorizationAction : MarshalByRefObject
    {
        private IAuthorizationDao _authorizationDao;
        private bool _isAdministratorModeEnabled;

        public CheckAuthorizationAction()
        {
            _authorizationDao = new AzManAuthorizationDao();

            bool.TryParse(ConfigurationManager.AppSettings["AdministratorMode"], out _isAdministratorModeEnabled);
        }

        public CheckAuthorizationResult Execute(CheckAuthorizationParameters parameters)
        {
            CheckAuthorizationResult result = new CheckAuthorizationResult();

            if (_isAdministratorModeEnabled)
            {
                foreach (AuthOperation operation in parameters.Operations)
                {
                    operation.IsAuthorized = true;
                }
            }
            else
            {
                _authorizationDao.Authorize(parameters.Operations, parameters.Roles, parameters.AuthorizationProvider);
                result.IsAuthorizationEnabled = true;
            }

            result.Operations = parameters.Operations;

            return result;
        }
    }
}
