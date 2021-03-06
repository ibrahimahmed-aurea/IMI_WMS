﻿using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "DbSchema")]
    public class Schema : IXmlSerializable, IVersionControlled
    {
        public Schema()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [PropertyStorageHint(Length = 50, IsMandatory = true)]
        public virtual string Name { get; set; }

        [PropertyStorageHint(Length = 255, IsMandatory = true)]
        public virtual string ConnectionString { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(IsMandatory = true, ForeignKey = "FK_Schema_Application")]
        public virtual Application Application { get; set; }

        private IList<Package> packages;

        [DataAccess.DomainXmlIgnore]
        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.None)]
        public virtual IList<Package> Packages
        {
            get
            {
                if (packages == null)
                {
                    packages = new List<Package>();
                }
                return packages;
            }
            set { packages = value; }

        }

        private IList<Query> queries;

        [DataAccess.DomainXmlIgnore]
        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.None)]
        public virtual IList<Query> Queries
        {
            get
            {
                if (queries == null)
                {
                    queries = new List<Query>();
                }
                return queries;
            }
            set { queries = value; }

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
