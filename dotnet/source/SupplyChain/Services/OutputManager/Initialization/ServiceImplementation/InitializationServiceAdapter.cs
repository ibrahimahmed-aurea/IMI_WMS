using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.IdentityModel.Claims;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Imi.SupplyChain.OutputManager.Services.Initialization.ServiceContracts;
using InitializationActions = Imi.SupplyChain.OutputManager.Initialization.BusinessLogic;
using InitializationEntities = Imi.SupplyChain.OutputManager.Initialization.BusinessEntities;
using DataContracts = Imi.SupplyChain.OutputManager.Services.Initialization.DataContracts;
using Imi.Framework.Services;

namespace Imi.SupplyChain.OutputManager.Services.Initialization.ServiceImplementation
{
    public class InitializationServiceAdapter : MarshalByRefObject, IInitializationService
    {
        public FindOutputManagersResponse FindOutputManagers(FindOutputManagersRequest request)
        {
            IClaimsIdentity identity = (IClaimsIdentity)Thread.CurrentPrincipal.Identity;
            string upn = identity.Claims.FindAll(c => { return c.ClaimType == ClaimTypes.Upn; }).First().Value;

            InitializationActions.FindOutputManagersAction action = PolicyInjection.Create<InitializationActions.FindOutputManagersAction>();

            InitializationEntities.FindOutputManagersParameters parameters = Translators.FindOutputManagersTranslator.TranslateFromServiceToBusiness(request.FindOutputManagersParameters);
            
            //parameters.UserPrincipalIdentity = upn; /* user@domain */
            //parameters.UserIdentity = upn.Split('@')[0];

            
            InitializationEntities.FindOutputManagersResult r = action.Execute();

            FindOutputManagersResponse response = new FindOutputManagersResponse();

            response.FindOutputManagerResult = GenericMapper.MapNew<DataContracts.FindOutputManagerResult>(r);

            response.FindOutputManagerResult.OutputManagers = 
                GenericMapper.MapListNew<DataContracts.OutputManagerCollection, InitializationEntities.FindOutputManagerResult, DataContracts.OutputManager>(
                r.OutputManagers, Translators.FindOutputManagersTranslator.TranslateFromBusinessToService);


            return response;
        }

        

    }
}
