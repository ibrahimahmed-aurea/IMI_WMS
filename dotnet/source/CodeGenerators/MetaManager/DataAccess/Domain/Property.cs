using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "Property")]
    public class Property : IMappableProperty, IXmlSerializable
    {
        public Property()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }
        
        // TODO fix all the ignores here

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(IsMandatory = true, ForeignKey = "FK_Prop_BusinessEnt")]
        public virtual BusinessEntity BusinessEntity { get; set; }

        /// <summary>
        /// Enhanced name of StoredProcedure i.e. schemaobject StoredProcedure is ARTID
        /// Field._Name is ProductIdentity
        /// </summary>
        [PropertyStorageHint(IsMandatory = true, Length = 50)]
        public virtual string Name { get; set; }

        [PropertyStorageHint(Length = 255, IsMandatory = false)]
        public virtual string TypeName { get; set; }

        private Type type;

        /// <summary>
        /// Used for type conversion, see DbDatatype
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

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "HintId", IsMandatory = false, ForeignKey = "FK_Property_Hint")]
        public virtual Hint Hint { get; set; }

        private IList<PropertyCode> codes;

        [CollectionStorageHint(Inverse = true, Lazy = true, Cascade = CascadeOperation.AllDeleteOrphan)]
        public virtual IList<PropertyCode> Codes
        {
            get
            {
                if (codes == null)
                {
                    codes = new List<PropertyCode>();
                }
                return codes;
            }
            set { codes = value; }
        }

        private IList<PropertyCaption> captionVariations;

        [PropertyStorageHint(Ignore = true)]
        public virtual IList<PropertyCaption> CaptionVariations
        {
            get
            {
                if (captionVariations == null)
                {
                    captionVariations = new List<PropertyCaption>();
                }
                return captionVariations;
            }
            set { captionVariations = value; }

        }

        [PropertyStorageHint(IsMandatory = false, Lazy = false, ForeignKey = "FK_Prop_PropStorInfo", Cascade = CascadeAssociationOperation.All) ]
        public virtual Cdc.MetaManager.DataAccess.Domain.PropertyStorageInfo StorageInfo { get; set; }
                
        [PropertyStorageHint(Length = 255, IsMandatory = false)]
        public virtual string DisplayFormat { get; set; }

        private string visualComponentXml;

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Length = 4000, IsMandatory = false)]
        public virtual string VisualComponentXml 
        {
            get
            {
                if (visualComponent != null)
                {
                    StringBuilder builder = new StringBuilder();

                    XmlSerializer xml = new XmlSerializer(Type.GetType(VisualComponentTypeName));

                    XmlWriter writer = XmlWriter.Create(builder);
                    
                    xml.Serialize(writer, visualComponent);
                                        
                    return builder.ToString();
                }
                else
                    return visualComponentXml;
            }
            set
            {
                visualComponentXml = value;
            }
            

        }

        private string visualComponentTypeName;

        [PropertyStorageHint(Length = 255, IsMandatory = false)]
        public virtual string VisualComponentTypeName
        {
            get
            {
                if (visualComponent != null)
                    return visualComponent.GetType().FullName;
                else
                    return visualComponentTypeName;
            }
            set
            {
                visualComponentTypeName = value;
            }
        }

        private UXComponent visualComponent;

        [PropertyStorageHint(Ignore = true)]
        public virtual UXComponent VisualComponent
        {
            get
            {
                if ((visualComponent == null) && (!string.IsNullOrEmpty(VisualComponentTypeName)))
                {
                    if (string.IsNullOrEmpty(VisualComponentXml)) { return null; }
                    XmlSerializer xml = new XmlSerializer(Type.GetType(VisualComponentTypeName));
                    visualComponent = xml.Deserialize(new StringReader(VisualComponentXml)) as UXComponent;

                }

                return visualComponent;
            }
            set
            {
                visualComponent = value;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", GetType().Name, Id);
        }

        public virtual bool CanChangeDisplayFormat()
        {
            return (Type != null &&
                    (Type == typeof(DateTime) ||
                     Type == typeof(decimal) ||
                     Type == typeof(double) ||
                     Type == typeof(long) ||
                     Type == typeof(int)));
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
