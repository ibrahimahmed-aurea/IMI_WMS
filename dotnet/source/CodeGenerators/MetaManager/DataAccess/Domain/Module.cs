using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "Module")]
    public class Module : IXmlSerializable, IVersionControlled
    {
        public Module()
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

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "ApplicationId", IsMandatory = true, ForeignKey = "FK_Module_Application")]
        public virtual Application Application { get; set; }

        private IList<Dialog> dialogs;

        [DataAccess.DomainXmlIgnore]
        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade=CascadeOperation.None)]
        public virtual IList<Dialog> Dialogs
        {
            get
            {
                if (dialogs == null)
                {
                    dialogs = new List<Dialog>();
                }
                return dialogs;
            }
            set { dialogs = value; }

        }

        private IList<Workflow> workflows;

        [DataAccess.DomainXmlIgnore]
        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.None)]
        public virtual IList<Workflow> Workflows
        {
            get
            {
                if (workflows == null)
                {
                    workflows = new List<Workflow>();
                }
                return workflows;
            }
            set { workflows = value; }

        }

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool CodeGenerationOverride { get; set; }

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
