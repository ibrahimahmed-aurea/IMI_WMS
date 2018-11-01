using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    public class UXSessionProperty : IMappableProperty, IXmlSerializable
    {
        public UXSessionProperty()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [PropertyStorageHint(Length = 50, IsMandatory = false)]
        public virtual string Name { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(IsMandatory = true, ForeignKey = "FK_UXSessProp_UXSession")]
        public virtual UXSession UXSession { get; set; }

        [PropertyStorageHint(Length = 255, IsMandatory = false)]
        public virtual string TypeName { get; set; }

        private Type type;

        /// <summary>
        /// Used for type conversion
        /// </summary>
        /// 
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual Type Type
        {
            get
            {
                if ((type == null) && (TypeName != null))
                    type = Type.GetType(TypeName);

                return type;
            }
            set
            {
                type = value;
                if (value != null)
                    TypeName = type.AssemblyQualifiedName;
                else
                    TypeName = null;

            }
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
