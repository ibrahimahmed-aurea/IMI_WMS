using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ModelDesigner(IsMovable = true)]
    [ClassStorageHint(TableName = "ServiceMethod")]
    public class ServiceMethod : IMappableUXObject, IXmlSerializable, IVersionControlled, IMappedObject
    {
        public ServiceMethod()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [PropertyStorageHint(IsMandatory = true, Length = 50)]
        public virtual string Name { get; set; }

        [PropertyMapAttribute(Type = PropertyMapType.Request)]
        [PropertyStorageHint(Column = "RequestMapId", IsMandatory = true, ForeignKey = "FK_ServMethod_ReqPropMap", Cascade = CascadeAssociationOperation.All)]
        public virtual PropertyMap RequestMap { get; set; }

        [PropertyMapAttribute(Type = PropertyMapType.Response)]
        [PropertyStorageHint(Column = "ResponseMapId", IsMandatory = true, ForeignKey = "FK_ServMethod_RespPropMap", Cascade = CascadeAssociationOperation.All)]
        public virtual PropertyMap ResponseMap { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(IsMandatory = true, ForeignKey = "FK_ServMethod_Action", Cascade = CascadeAssociationOperation.None)]
        public virtual Action MappedToAction { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(IsMandatory = true, ForeignKey = "FK_ServMethod_Service")]
        public virtual Service Service { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual UXActionType ActionType
        {
            get
            {
                return UXActionType.ServiceMethod;
            }
        }

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

        #region IVersionControlled Members

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(IsMandatory = false)]
        public virtual bool IsLocked { get; set; }

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Length = 40, IsMandatory = false)]
        public virtual string LockedBy { get; set; }

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(IsMandatory = false)]
        public virtual DateTime? LockedDate { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual string RepositoryFileName
        {
            get { return this.GetType().Name + "_" + this.Id.ToString(); }
        }

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(IsMandatory = true)]
        public virtual VersionControlledObjectStat State { get; set; }

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
            object[] propertyMapAttributes = this.GetType().GetProperty("RequestMap").GetCustomAttributes(typeof(PropertyMapAttribute), true);
            setTarget = ((PropertyMapAttribute)propertyMapAttributes[0]).SetTarget;

            if (connectedToObject == null)
            {
                return RequestMap == null ? Guid.Empty : RequestMap.Id;
            }
            else if (NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(connectedToObject) == typeof(Action))
            {
                if (MappedToAction.Id == connectedToObject.Id)
                {
                    return RequestMap == null ? Guid.Empty : RequestMap.Id;
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
            object[] propertyMapAttributes = this.GetType().GetProperty("ResponseMap").GetCustomAttributes(typeof(PropertyMapAttribute), true);
            setTarget = ((PropertyMapAttribute)propertyMapAttributes[0]).SetTarget;

            if (connectedToObject == null)
            {
                return ResponseMap == null ? Guid.Empty : ResponseMap.Id;
            }
            else if (NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(connectedToObject) == typeof(Action))
            {
                if (MappedToAction.Id == connectedToObject.Id)
                {
                    return ResponseMap == null ? Guid.Empty : ResponseMap.Id;
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

        #endregion

    }
}
