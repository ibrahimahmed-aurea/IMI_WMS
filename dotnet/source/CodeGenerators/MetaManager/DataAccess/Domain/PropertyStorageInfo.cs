using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "PropertyStorageInfo")]
    public class PropertyStorageInfo : IXmlSerializable, IDomainObject
    {
        public PropertyStorageInfo()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }
        
        [PropertyStorageHint(IsMandatory = false)]
        public virtual int Length { get; set; }

        [PropertyStorageHint(IsMandatory = false)]
        public virtual int Precision { get; set; }

        [PropertyStorageHint(IsMandatory = false)]
        public virtual int Scale { get; set; }

        [PropertyStorageHint(IsMandatory = true, Length = 255)]
        public virtual string StorageType { get; set; }

        [PropertyStorageHint(IsMandatory = true, Length = 50)]
        public virtual string TableName { get; set; }

        [PropertyStorageHint(IsMandatory = true, Length = 50)]
        public virtual string ColumnName { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(IsMandatory = true, ForeignKey = "FK_PropStorInfo_Schema")]
        public virtual Schema Schema { get; set; }

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
    }
}
