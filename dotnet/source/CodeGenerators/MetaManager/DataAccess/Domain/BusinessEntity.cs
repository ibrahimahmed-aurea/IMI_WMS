using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using NHibernate;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "BusinessEntity")]
    public class BusinessEntity : IXmlSerializable, IVersionControlled
    {
        public BusinessEntity()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [PropertyStorageHint(Length = 50, IsMandatory = true, UniqueKey = "UNQ_BusinessEntity_Name_App")]
        public virtual string Name { get; set; }

        [PropertyStorageHint(IsMandatory = false, Length = 100)]
        public virtual string TableName { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(IsMandatory = true, UniqueKey = "UNQ_BusinessEntity_Name_App", ForeignKey = "FK_BusinessEnt_Application")]
        public virtual Application Application { get; set; }

        private IList<Property> properties;

        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.AllDeleteOrphan)]
        public virtual IList<Property> Properties
        {
            get
            {
                if (properties == null)
                {
                    properties = new List<Property>();
                }
                return properties;
            }
            set { properties = value; }
        }

        private IList<Action> actions;

        [DataAccess.DomainXmlIgnore]
        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.None)]
        public virtual IList<Action> Actions
        {
            get
            {
                if (actions == null)
                {
                    actions = new List<Action>();
                }
                return actions;
            }
            set { actions = value; }
        }

        [PropertyStorageHint(Length = 4000, IsMandatory = false)]
        public virtual string Description { get; set; }

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
    }
}
