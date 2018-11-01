using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Imi.HbmGenerator.Attributes;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName="Application")]
    public class Application : IXmlSerializable, IVersionControlled
    {
        public Application()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [PropertyStorageHint(Length=50, IsMandatory= true)]
        public virtual string Name { get; set; }

        [PropertyStorageHint(Length = 255, IsMandatory = false)]
        public virtual string Namespace { get; set; }

        [PropertyStorageHint(Length = 11, IsMandatory = false)]
        public virtual string Version { get; set; }

        private bool? isFrontend;

        [PropertyStorageHint(IsMandatory = false)]
        public virtual bool? IsFrontend
        {
            get
            {
                // if not set
                if (isFrontend == null)
                    return false;

                return isFrontend;
            }

            set
            {
                isFrontend = value;
            }
        }

        public virtual string GetPrefix()
        {
            if (IsFrontend.Value)
            {
                return string.Format("FE{0}", Id.ToString());
            }
            else
            {
                return string.Format("BE{0}", Id.ToString());
            }
        }

        private IList<Module> modules;

        [ApplicationOnlyFrontend]
        [DataAccess.DomainXmlIgnore]
        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.None)]
        public virtual IList<Module> Modules
        {
            get
            {
                if (modules == null)
                {
                    modules = new List<Module>();
                }
                return modules;
            }
            set { modules = value; }

        }

        private IList<CustomDialog> customDialogs;

        [ApplicationOnlyFrontend]
        [DataAccess.DomainXmlIgnore]
        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.None)]
        public virtual IList<CustomDialog> CustomDialogs
        {
            get
            {
                if (customDialogs == null)
                {
                    customDialogs = new List<CustomDialog>();
                }
                return customDialogs;
            }
            set { customDialogs = value; }

        }

        private IList<UXAction> uXActions;

        [ApplicationOnlyFrontend]
        [DataAccess.DomainXmlIgnore]
        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.None)]
        public virtual IList<UXAction> UXActions
        {
            get
            {
                if (uXActions == null)
                {
                    uXActions = new List<UXAction>();
                }
                return uXActions;
            }
            set { uXActions = value; }

        }

        private IList<View> views;

        [ApplicationOnlyFrontend]
        [DataAccess.DomainXmlIgnore]
        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.None)]
        public virtual IList<View> Views
        {
            get
            {
                if (views == null)
                {
                    views = new List<View>();
                }
                return views;
            }
            set { views = value; }

        }

        private IList<Service> services;

        [ApplicationOnlyBackend]
        [DataAccess.DomainXmlIgnore]
        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.None)]
        public virtual IList<Service> Services
        {
            get
            {
                if (services == null)
                {
                    services = new List<Service>();
                }
                return services;
            }
            set { services = value; }

        }

        private IList<BusinessEntity> businessEntities;

        [ApplicationOnlyBackend]
        [DataAccess.DomainXmlIgnore]
        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.None)]
        public virtual IList<BusinessEntity> BusinessEntities
        {
            get
            {
                if (businessEntities == null)
                {
                    businessEntities = new List<BusinessEntity>();
                }
                return businessEntities;
            }
            set { businessEntities = value; }

        }

        private IList<Report> reports;

        [ApplicationOnlyBackend]
        [DataAccess.DomainXmlIgnore]
        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.None)]
        public virtual IList<Report> Reports
        {
            get
            {
                if (reports == null)
                {
                    reports = new List<Report>();
                }
                return reports;
            }
            set { reports = value; }
        }

        [ApplicationOnlyFrontend]
        [DataAccess.DomainXmlById]
        [PropertyStorageHint(IsMandatory = false, Cascade = CascadeAssociationOperation.None, Lazy = true)]
        public virtual Menu Menu { get; set; }

        [ApplicationOnlyBackend]
        [DataAccess.DomainXmlById]
        [PropertyStorageHint(IsMandatory = false, Cascade = CascadeAssociationOperation.None, Lazy = true)]
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

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ApplicationOnlyFrontendAttribute : System.Attribute
    {
        public ApplicationOnlyFrontendAttribute() {}
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ApplicationOnlyBackendAttribute : System.Attribute
    {
        public ApplicationOnlyBackendAttribute() { }
    }

    
}
