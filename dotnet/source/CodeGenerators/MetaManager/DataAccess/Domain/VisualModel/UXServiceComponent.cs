using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    public abstract class UXServiceComponent : UXComponent, IMappedObject
    {
        protected UXServiceComponent()
            : this(null)
        { 
        
        }

        protected UXServiceComponent(string name)
            : base(name)
        { 
            DisplayPropertyNames = new List<string>();
        }

        private ServiceMethod serviceMethod;
        
        [XmlIgnore]
        [DomainReference]
        public ServiceMethod ServiceMethod
        {
            get
            {
                return serviceMethod;
            }
            set
            {
                serviceMethod = value;

                if (serviceMethod == null)
                    serviceMethodId = Guid.Empty;
            }
        }

        private Guid serviceMethodId;

        public Guid ServiceMethodId
        {
            get
            {
                if (ServiceMethod != null)
                    return ServiceMethod.Id;
                else
                    return serviceMethodId;
            }
            set
            {
                serviceMethodId = value;
            }
        }
                
        private PropertyMap componentMap;

        [XmlIgnore]
        [DomainReference]
        public PropertyMap ComponentMap
        {
            get
            {
                return componentMap;
            }
            set
            {
                componentMap = value;

                if (componentMap == null)
                    componentMapId = Guid.Empty;
            }
        }

        private Guid componentMapId;

        public Guid ComponentMapId
        {
            get
            {
                if (ComponentMap != null)
                    return ComponentMap.Id;
                else
                    return componentMapId;
            }
            set
            {
                componentMapId = value;
            }
        }

        public UXServiceComponent MasterComponent { get; set; }

        public string KeyPropertyName { get; set; }
                
        public List<string> DisplayPropertyNames { get; set; }



        #region IMappedObject Members

        public Guid GetRequestMapId(IDomainObject connectedToObject, out SetTargetChoice setTarget)
        {
            setTarget = SetTargetChoice.No;

            if (connectedToObject == null)
            {
                return ComponentMapId;
            }
            else if (NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(connectedToObject) == typeof(ServiceMethod) && connectedToObject.Id == ServiceMethodId)
            {
                return ComponentMapId;
            }
            else
            {
                return Guid.Empty;
            }
        }

        public Guid GetResponseMapId(IDomainObject connectedToObject, out SetTargetChoice setTarget)
        {
            setTarget = SetTargetChoice.No;
            return Guid.Empty;
        }

        #endregion
    }
}
