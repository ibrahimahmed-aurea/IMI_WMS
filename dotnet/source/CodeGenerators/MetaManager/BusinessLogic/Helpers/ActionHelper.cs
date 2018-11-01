using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess.Domain;
using NHibernate;
using Cdc.MetaManager.DataAccess;
using System.Diagnostics;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using System.Reflection;
using Domain = Cdc.MetaManager.DataAccess.Domain;
using Spring.Data.NHibernate.Support;
using Spring.Data.NHibernate;
using System.Xml.Linq;
using System.Xml;
using System.IO;


namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public class ActionHelper : Cdc.MetaManager.BusinessLogic.Helpers.IActionHelper
    {
        private IModelService ModelService { get; set; }
        private IConfigurationManagementService ConfigurationManagementService { get; set; }

        [Transaction(ReadOnly = false)]
        public Cdc.MetaManager.DataAccess.Domain.Action SaveAndSynchronize(Cdc.MetaManager.DataAccess.Domain.Action action, Dictionary<PropertyMap, List<MappedProperty>> mappedPropertyToDelete)
        {
            List<IDomainObject> objectsToSave = new List<IDomainObject>();
            //Save any new custom properties, since they are not saved by cascade
            BusinessEntity customBE = null; 
            foreach (MappedProperty mappedProperty in action.RequestMap.MappedProperties.Concat(action.ResponseMap.MappedProperties))
            {
                if (mappedProperty.Target.Id == Guid.Empty)
                {
                    if (mappedProperty.Target is Property)
                    {
                        if (customBE == null)
                        {
                            customBE = ModelService.GetInitializedDomainObject<BusinessEntity>(mappedProperty.TargetProperty.BusinessEntity.Id);
                        }

                        if (action.Id == Guid.Empty)
                        {
                            ModelService.SaveDomainObject(mappedProperty.Target);
                        }
                        else
                        {
                            mappedProperty.Target.Id = Guid.NewGuid();
                            objectsToSave.Add(mappedProperty.Target);
                        }
                    }
                }
            }
            
            if (action.Id != Guid.Empty)
            {
                if (action.Query != null)
                {
                    foreach (QueryProperty qp in action.Query.Properties)
                    {
                        if (qp.Id == Guid.Empty)
                        {
                            qp.Id = Guid.NewGuid();
                            objectsToSave.Add(qp);
                        }
                    }
                }
                else if (action.StoredProcedure != null)
                {
                    foreach (ProcedureProperty pp in action.StoredProcedure.Properties)
                    {
                        if (pp.Id == Guid.Empty)
                        {
                            pp.Id = Guid.NewGuid();
                            objectsToSave.Add(pp);
                        }
                    }
                }

                List<IDomainObject> objectsToDelete = new List<IDomainObject>();

                foreach (List<MappedProperty> mpList in mappedPropertyToDelete.Values)
                {
                    objectsToDelete.AddRange(mpList);
                }

                ModelService.StartSynchronizePropertyMapsInObjects(action, objectsToSave, objectsToDelete);

                foreach (PropertyMap map in mappedPropertyToDelete.Keys)
                {
                    foreach (MappedProperty mp in mappedPropertyToDelete[map])
                    {
                        map.MappedProperties.Remove(mp);
                    }
                }
            }
            
            action = (DataAccess.Domain.Action)ModelService.MergeSaveDomainObject(action);

            if (customBE != null)
            {
                ConfigurationManagementService.CheckOutDomainObject(customBE.Id, typeof(BusinessEntity));
            }

            return action;
        }
    }
}
