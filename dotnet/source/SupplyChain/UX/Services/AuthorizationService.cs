using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Practices.CompositeUI;
using FrontendServices = Imi.SupplyChain.UX.Infrastructure.Services;
using BackendAuthDataContracts = Imi.SupplyChain.Services.Authorization.DataContracts;
using BackendAuthService = Imi.SupplyChain.Services.Authorization.ServiceContracts;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX;
using Imi.Framework.Services;
using Imi.SupplyChain.UX.Services;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Services
{
    public class AuthorizationService : FrontendServices.IAuthorizationService
    {
        private BackendAuthService.IAuthorizationService _authService;

        [ServiceDependency]
        public IUserSessionService UserSessionService { get; set; }

        [WcfServiceDependency]
        public BackendAuthService.IAuthorizationService AuthService
        {
            get
            {
                return _authService;
            }
            set
            {
                _authService = value;
            }
        }

        #region IAuthorizationService Members

        public bool IsAuthorized(string operation)
        {
            throw new NotImplementedException();
        }

        public void CheckAuthorization(ICollection<IAuthOperation> operations)
        {
            CheckAuthorization(null, operations);
        }

        public bool CheckAuthorization(string applicationName, IEnumerable<IAuthOperation> operations)
        {
            Dictionary<string, IList<IAuthOperation>> operationDictionary = new Dictionary<string, IList<IAuthOperation>>();
                        
            foreach (IAuthOperation operation in operations)
            {
                if (!operationDictionary.ContainsKey(operation.Operation))
                {
                    operationDictionary.Add(operation.Operation, new List<IAuthOperation>());
                }

                operationDictionary[operation.Operation].Add(operation);
            }

            BackendAuthService.CheckAuthorizationRequest checkAuthRequest = new BackendAuthService.CheckAuthorizationRequest();

            checkAuthRequest.CheckAuthorizationParameters = new BackendAuthDataContracts.CheckAuthorizationParameters();
            checkAuthRequest.CheckAuthorizationParameters.AuthorizationProvider = applicationName;

            BackendAuthDataContracts.AuthOperationCollection authOperations = new BackendAuthDataContracts.AuthOperationCollection();
                        
            foreach (string operation in operationDictionary.Keys)
            {
                BackendAuthDataContracts.AuthOperation authOperation = new BackendAuthDataContracts.AuthOperation();
                authOperation.Operation = operation;
                authOperations.Add(authOperation);
            }

            checkAuthRequest.CheckAuthorizationParameters.Operations = authOperations;

            BackendAuthService.CheckAuthorizationResponse checkAuthResponse = AuthService.CheckAuthorization(checkAuthRequest);
                        
            foreach (BackendAuthDataContracts.AuthOperation operationResponse in checkAuthResponse.CheckAuthorizationResult.Operations)
            {
                IList<IAuthOperation> operationList = operationDictionary[operationResponse.Operation];

                foreach (IAuthOperation operation in operationList)
                {
                    operation.IsAuthorized = operationResponse.IsAuthorized;
                }
            }

            return checkAuthResponse.CheckAuthorizationResult.IsAuthorizationEnabled;
        }

        #endregion
    }
}
