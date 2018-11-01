using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;
using NHibernate;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using System.Reflection;
using System.Collections;

namespace Cdc.MetaManager.BusinessLogic
{
    public class MetaManagerUtil
    {
        
        public static PropertyMap InitializePropertyMap(PropertyMap map)
        {
            if (map != null)
            {
                foreach (MappedProperty property in map.MappedProperties)
                {
                    WalkMappedProperty(property);
                }
            }

            return map;
        }

        public static void WalkMappedProperty(MappedProperty mappedProperty)
        {
            NHibernateUtil.Initialize(mappedProperty.Target);
            NHibernateUtil.Initialize(mappedProperty.Source);

            if (mappedProperty.RequestMappedProperty != null)
                NHibernateUtil.Initialize(mappedProperty.RequestMappedProperty);

            if (mappedProperty.Source != null)
            {
                NHibernateUtil.Initialize(mappedProperty.Source.Name);
                NHibernateUtil.Initialize(mappedProperty.Source.Type);
            }

            NHibernateUtil.Initialize(mappedProperty.DefaultSessionProperty);

            if (mappedProperty.Target is MappedProperty)
                WalkMappedProperty(mappedProperty.Target as MappedProperty);
        }

        
        
    }
}
