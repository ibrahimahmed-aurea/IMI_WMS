using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using NHibernate;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "MenuItem")]
    public class MenuItem : ILocalizable, IXmlSerializable, IDomainObject
    {
        public MenuItem()
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
        [PropertyStorageHint(Column = "MenuId", IsMandatory = false, ForeignKey = "FK_MenuItem_Menu")]
        public virtual Menu Menu { get; set; }

        private IList<MenuItem> childrens;

        [CollectionStorageHint(Column = "ParentId", Inverse = true, Cascade = CascadeOperation.All)]
        public virtual IList<MenuItem> Children
        {
            get
            {
                if (childrens == null)
                {
                    childrens = new List<MenuItem>();
                }
                return childrens;
            }
            set { childrens = value; }
        }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "ParentId", IsMandatory = false, ForeignKey = "FK_MenuItem_MenuItem")]
        public virtual MenuItem Parent { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "UXActionId", IsMandatory = false, ForeignKey = "FK_MenuItem_UXAction")]
        public virtual UXAction Action { get; set; }

        [PropertyStorageHint(IsMandatory = true, Length = 50)]
        public virtual string Name { get; set; }

        [PropertyStorageHint(IsMandatory = true, Length = 50)]
        [Localizable]
        public virtual string Caption { get; set; }

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual string AuthorizationId
        {
            get
            {
                return Id.ToString();
            }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", GetType().Name, Id);
        }

        #region ILocalizable Members
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual string MetaId
        {
            get
            {
                return Id.ToString();
            }
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
