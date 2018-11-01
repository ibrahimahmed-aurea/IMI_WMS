using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using NHibernate;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "ViewAction")]
    public class ViewAction : IXmlSerializable, IDomainObject, IMappedObject
    {
        public ViewAction()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "UXActionId", IsMandatory = true, Cascade = CascadeAssociationOperation.None, ForeignKey = "FK_ViewAction_UXAction")]
        public virtual UXAction Action { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "ViewNodeId", IsMandatory = true, ForeignKey = "FK_ViewAction_ViewNode")]
        public virtual ViewNode ViewNode { get; set; }

        [PropertyMapAttribute(Type = PropertyMapType.Request, SetTarget = SetTargetChoice.No)]
        [PropertyStorageHint(Column = "ViewToActionMapId", IsMandatory = false, Cascade = CascadeAssociationOperation.All, ForeignKey = "FK_ViewAction_PropertyMap")]
        public virtual PropertyMap ViewToActionMap { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual int Sequence { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual ViewActionType Type { get; set; }

        // Mapped property of the Dialog Actions ResponseMap of the View that is the Interface View.
        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "DrilldownFieldMappedPropertyId", IsMandatory = false, Cascade = CascadeAssociationOperation.None, ForeignKey = "FK_ViewAction_MappedProperty")]
        public virtual MappedProperty DrilldownFieldMappedProperty { get; set; }

        // Mapped property of the view responsemap that contains the file data. Used for ViewActiontype Open File.
        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "FileContentMappedPropertyId", IsMandatory = false, Cascade = CascadeAssociationOperation.None, ForeignKey = "FK_ViewAction_MappedProperty")]
        public virtual MappedProperty FileContentMappedProperty { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", GetType().Name, Id);
        }

        #region IXmlSerializable Members

        public virtual System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(System.Xml.XmlReader reader)
        {
            DataAccess.DomainXmlSerializationHelper.ReadXML(this, reader);
        }

        public virtual void WriteXml(System.Xml.XmlWriter writer)
        {
            DataAccess.DomainXmlSerializationHelper.WriteXML(this, writer);
        }

        #endregion

        #region IEquatable<IDomainObject> Members

        public virtual bool Equals(IDomainObject other)
        {
            if (other == null) { return false; }

            if (NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(this) == NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(other))
            {
                if (this.Id == other.Id)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region IMappedObject Members

        public virtual Guid GetRequestMapId(IDomainObject connectedToObject, out SetTargetChoice setTarget)
        {
            object[] propertyMapAttributes = this.GetType().GetProperty("ViewToActionMap").GetCustomAttributes(typeof(PropertyMapAttribute), true);
            setTarget = ((PropertyMapAttribute)propertyMapAttributes[0]).SetTarget;

            if (connectedToObject == null)
            {
                return ViewToActionMap == null ? Guid.Empty : ViewToActionMap.Id;
            }
            else if (NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(connectedToObject) == typeof(UXAction))
            {
                if (Action.Id == connectedToObject.Id)
                {
                    return ViewToActionMap == null ? Guid.Empty : ViewToActionMap.Id;
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

        public virtual Guid GetResponseMapId(IDomainObject connectedToObject, out SetTargetChoice setTarget)
        {
            setTarget = SetTargetChoice.No;
            return Guid.Empty;
        }

        #endregion
    }
}
