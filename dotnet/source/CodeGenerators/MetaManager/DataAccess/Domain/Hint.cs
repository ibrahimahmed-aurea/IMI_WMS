using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "Hint")]
    public class Hint : IXmlSerializable , IDomainObject
    {
        public Hint()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [PropertyStorageHint(Length = 4000, IsMandatory = false)]
        public virtual string Text { get; set; }

        [PropertyStorageHint(IsMandatory = false)]
        public virtual int? ODRId { get; set; }

        [PropertyStorageHint(IsMandatory = false)]
        public virtual int? BaseODRId { get; set; }

        [PropertyStorageHint(Length=255, IsMandatory = false)]
        public virtual string ODRObjectId { get; set; }
                        
        public override string ToString()
        {
            return string.Format("{0} ({1})", GetType().Name, Id);
        }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "HintCollectionId", IsMandatory = false, ForeignKey = "FK_Hint_HintCollection")]
        public virtual HintCollection HintCollection { get; set; }

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
