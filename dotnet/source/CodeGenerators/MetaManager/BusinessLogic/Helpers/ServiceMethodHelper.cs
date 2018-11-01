using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public class ServiceMethodHelper : Cdc.MetaManager.BusinessLogic.Helpers.IServiceMethodHelper
    {
        private IModelService ModelService { get; set; }

        [Transaction(ReadOnly = false)]
        public ServiceMethod SaveAndSynchronize(ServiceMethod serviceMethod)
        {
            Cdc.MetaManager.DataAccess.Domain.Action action = ModelService.GetDomainObject<Cdc.MetaManager.DataAccess.Domain.Action>(serviceMethod.MappedToAction.Id);

            ModelService.CreateAndSynchronizePropertyMaps(action, serviceMethod);            
            serviceMethod = (DataAccess.Domain.ServiceMethod)ModelService.MergeSaveDomainObject(serviceMethod);

            return serviceMethod;
        }
    }
}
