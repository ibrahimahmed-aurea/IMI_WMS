using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.DataAccess.Domain;
using Spring.Transaction.Interceptor;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public class BusinessEntityHelper : Cdc.MetaManager.BusinessLogic.Helpers.IBusinessEntityHelper
    {
        private IModelService ModelService { get; set; }
        private IConfigurationManagementService ConfigurationManagementService { get; set; }
        
        [Transaction(ReadOnly = false)]
        public void UpdateBuisinessEntities(DataModelChanges detectedChanges, Application backendApplication)
        {
            backendApplication = ModelService.GetDomainObject<Application>(backendApplication.Id);

            BusinessEntity lastSavedEntity = null;

            foreach (KeyValuePair<object, DataModelChange> keyValue in detectedChanges)
            {
                // Check if new or changed
                if ( (keyValue.Value.ContainDataModelChangeType(DataModelChangeType.Modified) ||
                    keyValue.Value.ContainDataModelChangeType(DataModelChangeType.New))
                    && keyValue.Value.Apply )
                {

                    BusinessEntity entity = null;

                    if (keyValue.Key is BusinessEntity)
                    {
                        entity = (BusinessEntity)keyValue.Key;

                    }
                    else if (keyValue.Key is Property)
                    {
                        Property property = (Property)keyValue.Key;
                        entity = (property).BusinessEntity;
                    }

                    if (entity != null && lastSavedEntity != entity)
                    {
                        // Save changes
                        entity = (BusinessEntity)ModelService.MergeSaveDomainObject(entity);

                        //CheckOut Business Entity
                        CheckOutInObject(entity, true, backendApplication);

                        // Add the view to the the application list
                        backendApplication.BusinessEntities.Add(entity);

                        // Save application
                        ModelService.SaveDomainObject(backendApplication);

                        lastSavedEntity = entity;
                    }
                }
            }

            foreach (KeyValuePair<object, DataModelChange> keyValue in detectedChanges)
            {
                if (keyValue.Value.ContainDataModelChangeType(DataModelChangeType.Deleted) &&
                    keyValue.Value.Apply )
                {
                    if (keyValue.Key is BusinessEntity)
                    {
                        BusinessEntity entity = (BusinessEntity)keyValue.Key;

                        ModelService.DeleteDomainObject(entity);
                    }
                    else if (keyValue.Key is Property)
                    {
                        Property property = (Property)keyValue.Key;

                        ModelService.DeleteDomainObject(property);
                    }
                }
            }
        }

        private void CheckOutInObject(DataAccess.IVersionControlled checkOutObject, bool trueCheckOut_falseCheckIn, Application backendApplication)
        {
            DataAccess.IVersionControlled domainObject = null;
            domainObject = checkOutObject;

            if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(domainObject.GetType()))
            {
                if (trueCheckOut_falseCheckIn)
                {
                        MetaManagerServices.GetConfigurationManagementService().CheckOutDomainObject(domainObject.Id, domainObject.GetType());
                }
                else
                {
                        MetaManagerServices.GetConfigurationManagementService().CheckInDomainObject(domainObject.Id, domainObject.GetType(), backendApplication);
                }
            }
        }
    }
}
