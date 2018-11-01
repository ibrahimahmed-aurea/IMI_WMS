using System;
using System.Collections.Generic;
using System.Text;
using Imi.Framework.Services;
using Service = Imi.SupplyChain.Services.Authorization.DataContracts;
using Business = Imi.SupplyChain.Authorization.BusinessEntities;

namespace Imi.SupplyChain.Services.Authorization.ServiceImplementation.Translators
{
    static class CheckAuthorizationTranslator
    {
        public static Service.CheckAuthorizationResult TranslateFromBusinessToService(Business.CheckAuthorizationResult businessEntity)
        {
            Service.CheckAuthorizationResult result = new Service.CheckAuthorizationResult();

            result.IsAuthorizationEnabled = businessEntity.IsAuthorizationEnabled;
            result.Operations = new Service.AuthOperationCollection();

            foreach(Business.AuthOperation businessOperation in businessEntity.Operations) 
            {
                Service.AuthOperation serviceOperation = new Service.AuthOperation() 
                { 
                    Operation = businessOperation.Operation,
                    IsAuthorized = businessOperation.IsAuthorized 
                };

                result.Operations.Add(serviceOperation);
            }

            return result;
        }

        public static Business.CheckAuthorizationParameters TranslateFromServiceToBusiness(Service.CheckAuthorizationParameters serviceEntity)
        {
            Business.CheckAuthorizationParameters parameters = new Business.CheckAuthorizationParameters();

            parameters.Operations = new List<Business.AuthOperation>();
            parameters.AuthorizationProvider = serviceEntity.AuthorizationProvider;

            foreach(Service.AuthOperation serviceOperation in serviceEntity.Operations) 
            {
                Business.AuthOperation businnessOperation = new Business.AuthOperation()
                {
                    Operation = serviceOperation.Operation,
                    IsAuthorized = serviceOperation.IsAuthorized
                };

                parameters.Operations.Add(businnessOperation);
            }

            return parameters;
        }
    }
}
