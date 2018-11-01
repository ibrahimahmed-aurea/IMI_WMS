using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "DbQuery")]
    public class Query : IMappableObject, IXmlSerializable
    {
        public Query()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [PropertyStorageHint(Length = 32768, IsMandatory = false)]
        public virtual string SqlStatement { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(IsMandatory = true, ForeignKey = "FK_Query_Schema")]
        public virtual Schema Schema { get; set; }

        [PropertyStorageHint(IsMandatory = false)]
        public virtual QueryType QueryType { get; set; }

        private IList<QueryProperty> properties;

        [CollectionStorageHint(Inverse = true, Cascade = CascadeOperation.AllDeleteOrphan)]
        public virtual IList<QueryProperty> Properties
        {
            get
            {
                if (properties == null)
                {
                    properties = new List<QueryProperty>();
                }
                return properties;
            }
            set { properties = value; }
        }

        [Search(SearchType = SearchTypes.FreeText)]
        [PropertyStorageHint(Length = 100, IsMandatory = true)]
        public virtual string Name { get; set; }

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual ActionMapTarget ObjectType
        {
            get
            {
                return ActionMapTarget.Query;
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
