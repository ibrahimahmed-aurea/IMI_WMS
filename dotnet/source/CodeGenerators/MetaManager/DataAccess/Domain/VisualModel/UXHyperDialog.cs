using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    [VisualDesigner("HyperDialog")]
    public class UXHyperDialog : UXComponent, ILocalizable, IMappedObject
    {
        public UXHyperDialog()
            : this(null)
        {
        }

        public UXHyperDialog(string name)
            :base(name)
        {
        }

        [Localizable]
        public string Caption { get; set; }
                
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
