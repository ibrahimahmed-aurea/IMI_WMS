using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "DataSource")]
    public class DataSource : IXmlSerializable , IDomainObject, IMappedObject
    {
        public DataSource()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [PropertyStorageHint(IsMandatory = true, Length = 100, UniqueKey = "UNQ_DataSource_View_Name")]
        public virtual string Name { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "ViewId", IsMandatory = true, ForeignKey = "FK_DataSource_View", UniqueKey = "UNQ_DataSource_View_Name")]
        public virtual View View { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "ServiceMethodId", IsMandatory = true, ForeignKey = "FK_DataSource_ServiceMethod")]
        public virtual ServiceMethod ServiceMethod { get; set; }

        [PropertyMapAttribute(Type = PropertyMapType.Request, SetTarget = SetTargetChoice.No)]
        [PropertyStorageHint(Column = "RequestMapId", IsMandatory = true, ForeignKey = "FK_DataSource_ReqPropMap", Cascade = CascadeAssociationOperation.All)]
        public virtual PropertyMap RequestMap { get; set; }

        [PropertyMapAttribute(Type = PropertyMapType.Response, SetTarget = SetTargetChoice.No)]
        [PropertyStorageHint(Column = "ResponseMapId", IsMandatory = true, ForeignKey = "FK_DataSource_RespPropMap", Cascade = CascadeAssociationOperation.All)]
        public virtual PropertyMap ResponseMap { get; set; }

        /// <summary>
        /// Calls from client to server will only run if one or more of the mapped resultvalues are null.
        /// </summary>
        [PropertyStorageHint(Column = "OnlyRunIfValuesAreNull", IsMandatory = true)]
        public virtual bool OnlyRunIfValuesAreNull { get; set; }

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
            object[] propertyMapAttributes = this.GetType().GetProperty("RequestMap").GetCustomAttributes(typeof(PropertyMapAttribute), true);
            setTarget = ((PropertyMapAttribute)propertyMapAttributes[0]).SetTarget;
            
            if (connectedToObject == null)
            {
                return RequestMap == null ? Guid.Empty : RequestMap.Id;
            }
            else if (NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(connectedToObject) == typeof(ServiceMethod))
            {
                if (ServiceMethod.Id == connectedToObject.Id)
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
            else if (NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(connectedToObject) == typeof(View))
            {
                if (View.Id == connectedToObject.Id)
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
