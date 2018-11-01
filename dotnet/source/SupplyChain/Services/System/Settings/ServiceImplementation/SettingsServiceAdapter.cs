using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.IdentityModel.Claims;
using Imi.Framework.Services;
using Imi.SupplyChain.Services.Settings.ServiceContracts;
using Imi.SupplyChain.Services.Settings.ServiceImplementation.Translators;
using BusinessEntities = Imi.SupplyChain.Settings.BusinessEntities;
using BusinessActions = Imi.SupplyChain.Settings.BusinessLogic;

namespace Imi.SupplyChain.Services.Settings.ServiceImplementation
{
    public class SettingsServiceAdapter : MarshalByRefObject, ISettingsService
    {
        private BusinessActions.DeleteBlobAction deleteBlobAction;
        private object deleteBlobActionLock = new object();
        private BusinessActions.CreateOrUpdateBlobAction createOrUpdateBlobAction;
        private object createOrUpdateBlobActionLock = new object();
        private BusinessActions.FindContainerAction findContainerAction;
        private object findContainerActionLock = new object();
        private BusinessActions.FindBlobAction findBlobAction;
        private object findBlobActionLock = new object();
        private BusinessActions.CreateOrUpdateContainerAction createOrUpdateContainerAction;
        private object createOrUpdateContainerActionLock = new object();
        private BusinessActions.DeleteContainerAction deleteContainerAction;
        private object deleteContainerActionLock = new object();
        private const string ShellContainerName = "/Imi.SupplyChain.UX.Shell/Configuration/Files";

        public SettingsServiceAdapter()
        { 
        }

        private string GetUserContainerName(string containerName)
        {
            IClaimsIdentity identity = (IClaimsIdentity)Thread.CurrentPrincipal.Identity;
            string upn = identity.Claims.FindAll(c => { return c.ClaimType == ClaimTypes.Upn; }).First().Value;

            if (containerName.StartsWith(upn) || containerName == ShellContainerName)
            {
                return containerName;
            }
            else
            {
                if (!containerName.StartsWith("/"))
                {
                    upn += "/";
                }
                
                return upn + containerName;
            }
        }

        public DeleteBlobResponse DeleteBlob(DeleteBlobRequest request)
        {
            lock (deleteBlobActionLock)
            {
                if (deleteBlobAction == null)
                {
                    deleteBlobAction = PolicyInjection.Create<BusinessActions.DeleteBlobAction>();
                }
            }

            request.DeleteBlobParameters.ContainerName = GetUserContainerName(request.DeleteBlobParameters.ContainerName);

            BusinessEntities.DeleteBlobParameters parameters = DeleteBlobParametersTranslator.TranslateFromServiceToBusiness(request.DeleteBlobParameters);
            BusinessEntities.DeleteBlobResult result = deleteBlobAction.Execute(parameters);

            DeleteBlobResponse response = new DeleteBlobResponse();
            response.DeleteBlobResult = DeleteBlobResultTranslator.TranslateFromBusinessToService(result);

            return response;
        }

        public CreateOrUpdateBlobResponse CreateOrUpdateBlob(CreateOrUpdateBlobRequest request)
        {
            lock (createOrUpdateBlobActionLock)
            {
                if (createOrUpdateBlobAction == null)
                {
                    createOrUpdateBlobAction = PolicyInjection.Create<BusinessActions.CreateOrUpdateBlobAction>();
                }
            }

            request.CreateOrUpdateBlobParameters.ContainerName = GetUserContainerName(request.CreateOrUpdateBlobParameters.ContainerName);
            
            BusinessEntities.CreateOrUpdateBlobParameters parameters = CreateOrUpdateBlobParametersTranslator.TranslateFromServiceToBusiness(request.CreateOrUpdateBlobParameters);
            BusinessEntities.CreateOrUpdateBlobResult result = createOrUpdateBlobAction.Execute(parameters);

            CreateOrUpdateBlobResponse response = new CreateOrUpdateBlobResponse();
            response.CreateOrUpdateBlobResult = CreateOrUpdateBlobResultTranslator.TranslateFromBusinessToService(result);

            return response;
        }

        public FindContainerResponse FindContainer(FindContainerRequest request)
        {
            lock (findContainerActionLock)
            {
                if (findContainerAction == null)
                {
                    findContainerAction = PolicyInjection.Create<BusinessActions.FindContainerAction>();
                }
            }

            request.FindContainerParameters.ContainerName = GetUserContainerName(request.FindContainerParameters.ContainerName);
                        
            BusinessEntities.FindContainerParameters parameters = FindContainerParametersTranslator.TranslateFromServiceToBusiness(request.FindContainerParameters);
            BusinessEntities.FindContainerResult result = findContainerAction.Execute(parameters);

            FindContainerResponse response = new FindContainerResponse();
            response.FindContainerResult = FindContainerResultTranslator.TranslateFromBusinessToService(result);

            return response;
        }

        public FindBlobResponse FindBlob(FindBlobRequest request)
        {
            lock (findBlobActionLock)
            {
                if (findBlobAction == null)
                {
                    findBlobAction = PolicyInjection.Create<BusinessActions.FindBlobAction>();
                }
            }

            request.FindBlobParameters.ContainerName = GetUserContainerName(request.FindBlobParameters.ContainerName);
                        
            BusinessEntities.FindBlobParameters parameters = FindBlobParametersTranslator.TranslateFromServiceToBusiness(request.FindBlobParameters);
            BusinessEntities.FindBlobResult result = findBlobAction.Execute(parameters);

            FindBlobResponse response = new FindBlobResponse();
            response.FindBlobResult = FindBlobResultTranslator.TranslateFromBusinessToService(result);

            return response;
        }

        public CreateOrUpdateContainerResponse CreateOrUpdateContainer(CreateOrUpdateContainerRequest request)
        {
            lock (createOrUpdateContainerActionLock)
            {
                if (createOrUpdateContainerAction == null)
                {
                    createOrUpdateContainerAction = PolicyInjection.Create<BusinessActions.CreateOrUpdateContainerAction>();
                }
            }

            request.CreateOrUpdateContainerParameters.Container.Name = GetUserContainerName(request.CreateOrUpdateContainerParameters.Container.Name);
                        
            BusinessEntities.CreateOrUpdateContainerParameters parameters = CreateOrUpdateContainerParametersTranslator.TranslateFromServiceToBusiness(request.CreateOrUpdateContainerParameters);
            BusinessEntities.CreateOrUpdateContainerResult result = createOrUpdateContainerAction.Execute(parameters);

            CreateOrUpdateContainerResponse response = new CreateOrUpdateContainerResponse();
            response.CreateOrUpdateContainerResult = CreateOrUpdateContainerResultTranslator.TranslateFromBusinessToService(result);

            return response;
        }

        public DeleteContainerResponse DeleteContainer(DeleteContainerRequest request)
        {
            lock (deleteContainerActionLock)
            {
                if (deleteContainerAction == null)
                {
                    deleteContainerAction = PolicyInjection.Create<BusinessActions.DeleteContainerAction>();
                }
            }

            request.DeleteContainerParameters.ContainerName = GetUserContainerName(request.DeleteContainerParameters.ContainerName);
                        
            BusinessEntities.DeleteContainerParameters parameters = DeleteContainerParametersTranslator.TranslateFromServiceToBusiness(request.DeleteContainerParameters);
            BusinessEntities.DeleteContainerResult result = deleteContainerAction.Execute(parameters);

            DeleteContainerResponse response = new DeleteContainerResponse();
            response.DeleteContainerResult = DeleteContainerResultTranslator.TranslateFromBusinessToService(result);

            return response;
        }
    }
}
