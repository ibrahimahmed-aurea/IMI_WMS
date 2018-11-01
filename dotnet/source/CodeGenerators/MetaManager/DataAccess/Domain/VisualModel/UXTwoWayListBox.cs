using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    [VisualDesigner("TwoWayListBox")]
    public class UXTwoWayListBox : UXComponent, ILocalizable, IMappedObject
    {
        public UXTwoWayListBox(string name)
            : base(name)
        {
            Width = 300;
            Height = 500;
        }

        public UXTwoWayListBox()
            : this(null)
        {

        }

        [Localizable]
        public string LeftCaption { get; set; }

        [Localizable]
        public string RightCaption { get; set; }

        private ServiceMethod leftFindServiceMethod;

        [XmlIgnore]
        [DomainReference]
        public ServiceMethod LeftFindServiceMethod
        {
            get
            {
                return leftFindServiceMethod;
            }
            set
            {
                leftFindServiceMethod = value;

                if (leftFindServiceMethod == null)
                    leftFindServiceMethodId = Guid.Empty;
            }
        }

        private Guid leftFindServiceMethodId;

        public Guid LeftFindServiceMethodId
        {
            get
            {
                if (LeftFindServiceMethod != null)
                    return LeftFindServiceMethod.Id;
                else
                    return leftFindServiceMethodId;
            }
            set
            {
                leftFindServiceMethodId = value;
            }
        }

        private PropertyMap leftFindServiceMethodMap;

        [XmlIgnore]
        [DomainReference]
        public PropertyMap LeftFindServiceMethodMap
        {
            get
            {
                return leftFindServiceMethodMap;
            }
            set
            {
                leftFindServiceMethodMap = value;

                if (leftFindServiceMethodMap == null)
                    leftFindServiceMethodMapId = Guid.Empty;
            }
        }

        private Guid leftFindServiceMethodMapId;

        public Guid LeftFindServiceMethodMapId
        {
            get
            {
                if (LeftFindServiceMethodMap != null)
                    return LeftFindServiceMethodMap.Id;
                else
                    return leftFindServiceMethodMapId;
            }
            set
            {
                leftFindServiceMethodMapId = value;
            }
        }

        private ServiceMethod rightFindServiceMethod;

        [XmlIgnore]
        [DomainReference]
        public ServiceMethod RightFindServiceMethod
        {
            get
            {
                return rightFindServiceMethod;
            }
            set
            {
                rightFindServiceMethod = value;

                if (rightFindServiceMethod == null)
                    rightFindServiceMethodId = Guid.Empty;
            }
        }

        private Guid rightFindServiceMethodId;

        public Guid RightFindServiceMethodId
        {
            get
            {
                if (RightFindServiceMethod != null)
                    return RightFindServiceMethod.Id;
                else
                    return rightFindServiceMethodId;
            }
            set
            {
                rightFindServiceMethodId = value;
            }
        }


        private PropertyMap rightFindServiceMethodMap;

        [XmlIgnore]
        [DomainReference]
        public PropertyMap RightFindServiceMethodMap
        {
            get
            {
                return rightFindServiceMethodMap;
            }
            set
            {
                rightFindServiceMethodMap = value;

                if (rightFindServiceMethodMap == null)
                    rightFindServiceMethodMapId = Guid.Empty;
            }
        }

        private Guid rightFindServiceMethodMapId;

        public Guid RightFindServiceMethodMapId
        {
            get
            {
                if (RightFindServiceMethodMap != null)
                    return RightFindServiceMethodMap.Id;
                else
                    return rightFindServiceMethodMapId;
            }
            set
            {
                rightFindServiceMethodMapId = value;
            }
        }

        private ServiceMethod addServiceMethod;

        [XmlIgnore]
        [DomainReference]
        public ServiceMethod AddServiceMethod
        {
            get
            {
                return addServiceMethod;
            }
            set
            {
                addServiceMethod = value;

                if (addServiceMethod == null)
                    addServiceMethodId = Guid.Empty;
            }
        }

        private Guid addServiceMethodId;

        public Guid AddServiceMethodId
        {
            get
            {
                if (AddServiceMethod != null)
                    return AddServiceMethod.Id;
                else
                    return addServiceMethodId;
            }
            set
            {
                addServiceMethodId = value;
            }
        }

        private PropertyMap addServiceMethodMap;

        [XmlIgnore]
        [DomainReference]
        public PropertyMap AddServiceMethodMap
        {
            get
            {
                return addServiceMethodMap;
            }
            set
            {
                addServiceMethodMap = value;

                if (addServiceMethodMap == null)
                    addServiceMethodMapId = Guid.Empty;
            }
        }

        private Guid addServiceMethodMapId;

        public Guid AddServiceMethodMapId
        {
            get
            {
                if (AddServiceMethodMap != null)
                    return AddServiceMethodMap.Id;
                else
                    return addServiceMethodMapId;
            }
            set
            {
                addServiceMethodMapId = value;
            }
        }

        private ServiceMethod removeServiceMethod;

        [XmlIgnore]
        [DomainReference]
        public ServiceMethod RemoveServiceMethod
        {
            get
            {
                return removeServiceMethod;
            }
            set
            {
                removeServiceMethod = value;

                if (removeServiceMethod == null)
                    removeServiceMethodId = Guid.Empty;
            }
        }

        private Guid removeServiceMethodId;

        public Guid RemoveServiceMethodId
        {
            get
            {
                if (RemoveServiceMethod != null)
                    return RemoveServiceMethod.Id;
                else
                    return removeServiceMethodId;
            }
            set
            {
                removeServiceMethodId = value;
            }
        }

        private PropertyMap removeServiceMethodMap;

        [XmlIgnore]
        [DomainReference]
        public PropertyMap RemoveServiceMethodMap
        {
            get
            {
                return removeServiceMethodMap;
            }
            set
            {
                removeServiceMethodMap = value;

                if (removeServiceMethodMap == null)
                    removeServiceMethodMapId = Guid.Empty;
            }
        }

        private Guid removeServiceMethodMapId;

        public Guid RemoveServiceMethodMapId
        {
            get
            {
                if (RemoveServiceMethodMap != null)
                    return RemoveServiceMethodMap.Id;
                else
                    return removeServiceMethodMapId;
            }
            set
            {
                removeServiceMethodMapId = value;
            }
        }

        public List<string> LeftDisplayPropertyNames { get; set; }

        public List<string> RightDisplayPropertyNames { get; set; }

        #region ILocalizable Members

        private string metaId;

        public string MetaId
        {
            get
            {
                if (string.IsNullOrEmpty(metaId))
                    metaId = Guid.NewGuid().ToString();

                return metaId;
            }
            set
            {
                metaId = value;
            }
        }

        #endregion


        #region IMappedObject Members

        public Guid GetRequestMapId(IDomainObject connectedToObject, out SetTargetChoice setTarget)
        {
            setTarget = SetTargetChoice.No;

            if (NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(connectedToObject) == typeof(ServiceMethod))
            {
                if (connectedToObject.Id == AddServiceMethodId)
                {
                    return AddServiceMethodMapId;
                }
                else if (connectedToObject.Id == RemoveServiceMethodId)
                {
                    return RemoveServiceMethodMapId;
                }
                else if (connectedToObject.Id == LeftFindServiceMethodId)
                {
                    return LeftFindServiceMethodMapId;
                }
                else if (connectedToObject.Id == RightFindServiceMethodId)
                {
                    return RightFindServiceMethodMapId;
                }
                else
                {
                    return Guid.Empty;
                }
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
