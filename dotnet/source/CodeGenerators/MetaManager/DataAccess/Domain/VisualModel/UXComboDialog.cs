using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    [VisualDesigner("ComboDialog")]
    public class UXComboDialog : UXComponent, IBindable, IMappedObject
    {
        public UXComboDialog()
            : this(null)
        {
        }

        public UXComboDialog(string name)
            :base(name)
        {
            IsEditable = true;
        }

        private MappedProperty mappedProperty;

        [XmlIgnore]
        [DomainReference]
        public MappedProperty MappedProperty
        {
            get
            {
                return mappedProperty;
            }
            set
            {
                if (mappedProperty != null && value != mappedProperty)
                    Hint = null;

                mappedProperty = value;

                if (mappedProperty == null)
                    mappedPropertyId = Guid.Empty;
            }
        }

        public bool IsEditable { get; set; }

        private Guid mappedPropertyId;

        public Guid MappedPropertyId
        {
            get
            {
                if (MappedProperty != null)
                    return MappedProperty.Id;
                else
                    return mappedPropertyId;
            }
            set
            {
                mappedPropertyId = value;
            }
        }

        private MappedProperty keyMappedProperty;

        [XmlIgnore]
        [DomainReference]
        public MappedProperty KeyMappedProperty
        {
            get
            {
                return keyMappedProperty;
            }
            set
            {
                if (keyMappedProperty != null && value != keyMappedProperty)
                {
                    Hint = null;
                }

                keyMappedProperty = value;

                if (keyMappedProperty == null)
                {
                    keyMappedPropertyId = Guid.Empty;
                }
            }
        }

        private Guid keyMappedPropertyId;

        public Guid KeyMappedPropertyId
        {
            get
            {
                if (keyMappedProperty != null)
                {
                    return keyMappedProperty.Id;
                }
                else
                {
                    return keyMappedPropertyId;
                }
            }
            set
            {
                keyMappedPropertyId = value;
            }
        }

        private Dialog dialog;

        [XmlIgnore]
        [DomainReference]
        public Dialog Dialog
        {
            get
            {
                return dialog;
            }
            set
            {
                dialog = value;

                if (dialog == null)
                    dialogId = Guid.Empty;
            }
        }

        private Guid dialogId;

        public Guid DialogId
        {
            get
            {
                if (Dialog != null)
                    return Dialog.Id;
                else
                    return dialogId;
            }
            set
            {
                dialogId = value;
            }
        }

        private DataSource dataSource;

        [XmlIgnore]
        [DomainReference]
        public DataSource DataSource
        {
            get
            {
                return dataSource;
            }
            set
            {
                dataSource = value;

                if (dataSource == null)
                    dataSourceId = Guid.Empty;
            }
        }

        private Guid dataSourceId;

        public Guid DataSourceId
        {
            get
            {
                if (DataSource != null)
                    return DataSource.Id;
                else
                    return dataSourceId;
            }
            set
            {
                dataSourceId = value;
            }
        }

        private PropertyMap resultMap;

        [XmlIgnore]
        [DomainReference]
        public PropertyMap ResultMap
        {
            get
            {
                return resultMap;
            }
            set
            {
                resultMap = value;

                if (resultMap == null)
                    resultMapId = Guid.Empty;
            }
        }

        private Guid resultMapId;

        public Guid ResultMapId
        {
            get
            {
                if (ResultMap != null)
                    return ResultMap.Id;
                else
                    return resultMapId;
            }
            set
            {
                resultMapId = value;
            }
        }

        private PropertyMap viewMap;

        [XmlIgnore]
        [DomainReference]
        public PropertyMap ViewMap
        {
            get
            {
                return viewMap;
            }
            set
            {
                viewMap = value;

                if (viewMap == null)
                    viewMapId = Guid.Empty;
            }
        }

        private Guid viewMapId;

        public Guid ViewMapId
        {
            get
            {
                if (ViewMap != null)
                    return ViewMap.Id;
                else
                    return viewMapId;
            }
            set
            {
                viewMapId = value;
            }
        }

        #region IMappedObject Members

        public Guid GetRequestMapId(IDomainObject connectedToObject, out SetTargetChoice setTarget)
        {
            setTarget = SetTargetChoice.No;

            if (connectedToObject == null)
            {
                return ViewMapId;
            }
            else if (NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(connectedToObject) == typeof(Dialog) && connectedToObject.Id == DialogId)
            {
                return ViewMapId;
            }
            else
            {
                return Guid.Empty;
            }

        }

        public Guid GetResponseMapId(IDomainObject connectedToObject, out SetTargetChoice setTarget)
        {
            setTarget = SetTargetChoice.No;

            if (NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(connectedToObject) == typeof(View))
            {
                return resultMapId;
            }
            else
            {
                return Guid.Empty;
            }
        }

        #endregion
    }
}
