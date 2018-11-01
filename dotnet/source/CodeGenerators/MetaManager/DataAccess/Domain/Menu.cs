using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "Menu")]
    public class Menu : IXmlSerializable, IVersionControlled
    {
        public Menu()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [PropertyStorageHint(Column = "TopMenuItemId", IsMandatory = true, ForeignKey = "FK_Menu_MenuItem", Cascade = CascadeAssociationOperation.All)]
        public virtual MenuItem TopMenuItem { get; set; }

        private IList<MenuItem> menuItems;

        [DataAccess.DomainXmlById]
        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.AllDeleteOrphan)]
        public virtual IList<MenuItem> MenuItems
        {
            get
            {
                if (menuItems == null)
                {
                    menuItems = new List<MenuItem>();
                }
                return menuItems;
            }
            set { menuItems = value; }

        }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "ApplicationId", IsMandatory = true, ForeignKey = "FK_Menu_Application")]
        public virtual Application Application { get; set; }

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
