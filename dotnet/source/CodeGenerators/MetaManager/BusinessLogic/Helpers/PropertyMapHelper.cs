using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public class PropertyMapHelper : IPropertyMapHelper
    {
        private IModelService ModelService { get; set; }
        private IConfigurationManagementService ConfigurationManagementService { get; set; }

        [Transaction(ReadOnly = false)]
        public void RemoveMappedProperty(MappedProperty mappedProperty)
        {
            mappedProperty = ModelService.GetDomainObject<MappedProperty>(mappedProperty.Id);
                        
            PropertyMap map = new PropertyMap(); 
            map.Id = mappedProperty.PropertyMap.Id;
            map.IsCollection = mappedProperty.PropertyMap.IsCollection;
            map.MappedProperties = new List<MappedProperty>(mappedProperty.PropertyMap.MappedProperties);
            map.MappedProperties.Remove(mappedProperty);
            List<IDomainObject> mpToDelete = new List<IDomainObject>();
            mpToDelete.Add(mappedProperty);
            ModelService.SynchronizePropertyMapChain( map , mpToDelete);

            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("RequestMappedProperty.Id", mappedProperty.Id);

            IList<MappedProperty> requestMappedProperties = ModelService.GetAllDomainObjectsByPropertyValues<MappedProperty>(criteria);

            foreach (MappedProperty refObj in requestMappedProperties)
            {
                List<IDomainObject> parents;
                IVersionControlled vcParent = refObj as IVersionControlled;

                if (vcParent == null)
                {
                    vcParent = ModelService.GetVersionControlledParent(refObj, out parents).First();
                }

                ConfigurationManagementService.CheckOutDomainObject(vcParent.Id, ModelService.GetDomainObjectType(vcParent));

                refObj.RequestMappedProperty = null;

                ModelService.SaveDomainObject(refObj);
            }
                        
            mappedProperty.PropertyMap = null;
            ModelService.MergeSaveDomainObject(map);
        }
    }
}
