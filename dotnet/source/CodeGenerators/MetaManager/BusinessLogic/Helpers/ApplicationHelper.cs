using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public class ApplicationHelper : Cdc.MetaManager.BusinessLogic.Helpers.IApplicationHelper
    {
        private IModelService ModelService { get; set; }

        [Transaction(ReadOnly = false)]
        public void UpdateBackendDataModel(DataModelChanges detectedChanges, Application backendApplication)
        {
            backendApplication = ModelService.GetDomainObject<Application>(backendApplication.Id);

            MetaManagerServices.Helpers.HintCollectionHelper.UpdateHints(detectedChanges, backendApplication.HintCollection);
            MetaManagerServices.Helpers.BusinessEntityHelper.UpdateBuisinessEntities(detectedChanges, backendApplication);
        }
    }
}
