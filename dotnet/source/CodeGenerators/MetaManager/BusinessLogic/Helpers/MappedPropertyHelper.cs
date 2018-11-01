using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;
using NHibernate;
using Spring.Transaction.Interceptor;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public class MappedPropertyHelper : Cdc.MetaManager.BusinessLogic.Helpers.IMappedPropertyHelper
    {
        private IModelService ModelService { get; set; }

        public static Hint GetHintForMappedProperty(MappedProperty property)
        {
            if (property != null && !property.IsCustom)
            {
                if (property.Target is Property)
                    return (property.Target as Property).Hint;
                else
                    return GetHintForMappedProperty(property.Source as MappedProperty);
            }

            return null;
        }
        
        [Transaction(ReadOnly = true)]
        public DbProperty GetOrigin(MappedProperty mappedProperty)
        {
            mappedProperty = ModelService.GetDomainObject<MappedProperty>(mappedProperty.Id);
            
            while (mappedProperty.MapProperty != null)
            {
                mappedProperty = mappedProperty.MapProperty;
            }

            DbProperty dbProperty = mappedProperty.Source as DbProperty;

            if (dbProperty != null)
            {
                NHibernateUtil.Initialize(dbProperty);
            }

            return dbProperty;
        }
    }
}
