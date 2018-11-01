using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Imi.Framework.Services;
using Imi.SupplyChain.Services.Settings.ServiceContracts;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation
{
    [ExceptionShielding("DefaultShieldingPolicy")]
	[ServiceApplicationName("System")]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SettingsService : ISettingsService
    {
        SettingsServiceAdapter adapter;

        public SettingsService()
        {
            adapter = PolicyInjection.Create<SettingsServiceAdapter>();
        }

        public DeleteBlobResponse DeleteBlob(DeleteBlobRequest request)
        {
            return adapter.DeleteBlob(request);
        }

        public CreateOrUpdateBlobResponse CreateOrUpdateBlob(CreateOrUpdateBlobRequest request)
        {
            return adapter.CreateOrUpdateBlob(request);
        }
        
        public FindContainerResponse FindContainer(FindContainerRequest request)
        {
            return adapter.FindContainer(request);
        }
        
        public FindBlobResponse FindBlob(FindBlobRequest request)
        {
            return adapter.FindBlob(request);
        }
        
        public CreateOrUpdateContainerResponse CreateOrUpdateContainer(CreateOrUpdateContainerRequest request)
        {
            return adapter.CreateOrUpdateContainer(request);
        }

        public DeleteContainerResponse DeleteContainer(DeleteContainerRequest request)
        {
            return adapter.DeleteContainer(request);
        }
    }
}
