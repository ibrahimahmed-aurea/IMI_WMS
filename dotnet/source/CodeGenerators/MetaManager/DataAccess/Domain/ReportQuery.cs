using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "ReportQuery")]
    public class ReportQuery : IXmlSerializable, IDomainObject
    {
        public ReportQuery()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual int Sequence { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "ParentId", IsMandatory = false, ForeignKey = "FK_ReportQuery_ReportQuery")]
        public virtual ReportQuery Parent { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(IsMandatory = false, ForeignKey = "FK_ReportQuery_Report")]
        public virtual Report Report { get; set; }

        [PropertyStorageHint(IsMandatory = true, ForeignKey = "FK_ReportQuery_Query", Cascade = CascadeAssociationOperation.All)]
        public virtual Query Query { get; set; }

        private IList<ReportQuery> children;

        [CollectionStorageHint(Column = "ParentId", Inverse = true, Cascade = CascadeOperation.AllDeleteOrphan)]
        public virtual IList<ReportQuery> Children
        {
            get
            {
                if (children == null)
                {
                    children = new List<ReportQuery>();
                }
                return children;
            }
            set { children = value; }

        }

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
    }
}
