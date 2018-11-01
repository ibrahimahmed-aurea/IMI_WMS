using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "MappedProperty")]
    public class MappedProperty : IMappableProperty, ICloneable, IXmlSerializable
    {
        public MappedProperty()
        {
            IsSearchable = true;
            IsEnabled = true;
            IsTransient = true;
        }

        [ReadOnly(true)]
        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(IsMandatory = true, ForeignKey = "FK_MapProp_PropMap")]
        public virtual PropertyMap PropertyMap { get; set; }

        /// <summary>
        /// Determines the order of the properties in the method signature.
        /// </summary>
        /// 
        [ReadOnly(true)]
        [PropertyStorageHint(IsMandatory = true)]
        public virtual int Sequence { get; set; }

        private MappedProperty mappedProperty;

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "MappedPropertyId", IsMandatory = false, Lazy = false, ForeignKey = "FK_MapProp_MapProp", Cascade = CascadeAssociationOperation.None)]
        public virtual MappedProperty MapProperty
        {
            get
            {
                return mappedProperty;
            }
            set
            {
                if (value != null)
                {
                    ProcedureProperty = null;
                    QueryProperty = null;
                }

                mappedProperty = value;
            }
        }

        private ProcedureProperty procedureProperty;

        [Browsable(false)]
        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "ProcedurePropertyId", IsMandatory = false, Lazy = false, ForeignKey = "FK_MapProp_ProcProp", Cascade = CascadeAssociationOperation.None)]
        public virtual ProcedureProperty ProcedureProperty
        {
            get
            {
                return procedureProperty;
            }
            set
            {
                if (value != null)
                {
                    MapProperty = null;
                    QueryProperty = null;
                }

                procedureProperty = value;
            }
        }

        private QueryProperty queryProperty;

        [Browsable(false)]
        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "QueryPropertyId", IsMandatory = false, Lazy = false, ForeignKey = "FK_MapProp_QryProp", Cascade = CascadeAssociationOperation.None)]
        public virtual QueryProperty QueryProperty
        {
            get
            {
                return queryProperty;
            }
            set
            {
                if (value != null)
                {
                    MapProperty = null;
                    ProcedureProperty = null;
                }

                queryProperty = value;
            }
        }
                
        /// <summary>
        /// References the SchemaObject being mapped
        /// </summary>
        /// 
        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual IMappableProperty Source
        {
            get
            {
                if (mappedProperty != null)
                    return mappedProperty;
                else if (procedureProperty != null)
                    return procedureProperty;
                else if (queryProperty != null)
                    return queryProperty;
                else
                    return null;
            }
            set
            {

                if (value is MappedProperty)
                {
                    MapProperty = value as MappedProperty;
                }
                else if (value is QueryProperty)
                {
                    QueryProperty = value as QueryProperty;
                }
                else if (value is ProcedureProperty)
                {
                    ProcedureProperty = value as ProcedureProperty;
                }
                else
                {
                    MapProperty = null;
                    ProcedureProperty = null;
                    QueryProperty = null;
                }

                
            }
        }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual IMappableProperty Target
        {
            get
            {
                if (targetProperty != null)
                    return targetProperty;
                else if (targetMappedProperty != null)
                    return targetMappedProperty;
                else
                    return null;
            }
            set
            {
                if (value is MappedProperty)
                {
                    TargetMappedProperty = value as MappedProperty;
                }
                else if (value is Property)
                {
                    TargetProperty = value as Property;
                }
                else
                {
                    TargetMappedProperty = null;
                    TargetProperty = null;
                }

                name = GetTargetName();
            }
        }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "RequestMappedPropertyId", IsMandatory = false, Lazy = false, ForeignKey = "FK_MapProp_ReqMapProp")]
        public virtual MappedProperty RequestMappedProperty { get; set; }

        private string defaultValue;

        /// <summary>
        /// Value representing the default value for the property
        /// </summary>
        [PropertyStorageHint(Length = 255, IsMandatory = false)]
        public virtual string DefaultValue
        {
            get
            {
                return defaultValue;
            }
            set
            {
                defaultValue = value;

                if (!string.IsNullOrEmpty(defaultValue))
                    DefaultSessionProperty = null;
            }
        }

        /// <summary>
        /// Only applies to properties that are input fields.
        /// </summary>
        [ReadOnly(true)]
        [PropertyStorageHint(IsMandatory = true)]
        public virtual bool IsMandatory { get; set; }

        /// <summary>
        /// Allows us to keep track of fields that are in the schema object 
        /// that we dont want to propagate up the architecture, i.e. we 
        /// know this StoredProcedure exists but we elected not to map it as opposed to 
        /// just leaving it off the map
        /// </summary>
        /// <remarks></remarks>
        /// <value></value>
        [PropertyStorageHint(IsMandatory = true)]
        public virtual bool IsEnabled { get; set; }

        /// <summary>
        /// Signal if the StoredProcedure is mapped to a schema object StoredProcedure or not.
        /// If true there is no Source.
        /// </summary>
        /// <remarks></remarks>
        /// <value></value>
        [ReadOnly(true)]
        [PropertyStorageHint(IsMandatory = true)]
        public virtual bool IsCustom { get; set; }

        private string name;

        [PropertyStorageHint(Length = 50, IsMandatory = false)]
        public virtual string Name 
        { 
            get 
            {
                if (string.IsNullOrEmpty(name))
                    return GetTargetName();
                else
                    return name;
            }
            set
            {
                if (value == GetTargetName())
                    name = null;
                else
                    name = value;
            }
        }

        private string GetTargetName()
        {
            if (Target != null)
                return Target.Name;
            else
                return null;
        }

        private string GetTargetDisplayFormat()
        {
            if (Target is Property)
                return (Target as Property).DisplayFormat;
            else
                return null;
        }
                
        /// <summary>
        /// Used for type conversion, see DbDatatype
        /// </summary>
        /// 
        [Browsable(false)]
        [PropertyStorageHint(Length = 255, IsMandatory = false)]
        public virtual string ValueConverterTypeName { get; set; }

        private IValueConverter valueConverter;

        /// <summary>
        /// Used for type conversion, see DbDatatype
        /// </summary>
        /// 
        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual IValueConverter ValueConverter 
        {
            get
            {
                if ((valueConverter == null) && (!string.IsNullOrEmpty(ValueConverterTypeName)))
                    valueConverter = Activator.CreateInstance(Type.GetType(ValueConverterTypeName)) as IValueConverter;

                return valueConverter;
            }
            set
            {
                valueConverter = value;
                if (value != null)
                    ValueConverterTypeName = valueConverter.GetType().AssemblyQualifiedName;
                else
                    ValueConverterTypeName = null;

            }
        }
        
        private IValueCalculator valueCalculator;

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual IValueCalculator ValueCalculator
        {
            get
            {
                if ((valueCalculator == null) && (!string.IsNullOrEmpty(ValueCalculatorTypeName)))
                    valueCalculator = Activator.CreateInstance(Type.GetType(ValueCalculatorTypeName)) as IValueCalculator;

                return valueCalculator;
            }
            set
            {
                valueCalculator = value;
                if (value != null)
                    ValueCalculatorTypeName = valueCalculator.GetType().AssemblyQualifiedName;
                else
                    ValueCalculatorTypeName = null;

            }
        }

        /// <summary>
        /// Used for type conversion, see DbDatatype
        /// </summary>
        [Browsable(false)]
        [PropertyStorageHint(Length = 255, IsMandatory = false)]
        public virtual string ValueCalculatorTypeName { get; set; }

        
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual Type Type 
        {
            get
            {
                if (Target != null)
                    return Target.Type;
                else
                    return null;
            }
        }
        


        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
        
        
        private MappedProperty targetMappedProperty;

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "TargetMappedPropertyId", IsMandatory = false, Lazy = false, ForeignKey = "FK_MapProp_TgtMapProp", Cascade = CascadeAssociationOperation.None)]
        public virtual MappedProperty TargetMappedProperty
        {
            get
            {
                return targetMappedProperty;
            }
            set
            {
                if (value != null)
                {
                    TargetProperty = null;
                }

                targetMappedProperty = value;
            }
        }
                
        private Property targetProperty;

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "TargetPropertyId", IsMandatory = false, Lazy = false, ForeignKey = "FK_MapProp_TgtProp", Cascade = CascadeAssociationOperation.None)]
        public virtual Property TargetProperty
        {
            get
            {
                return targetProperty;
            }
            set
            {
                if (value != null)
                {
                    TargetMappedProperty = null;
                }
                
                targetProperty = value;
            }
        }

        private UXSessionProperty uxSessionProperty;

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "DefaultSessionPropertyId", IsMandatory = false, Lazy = false, ForeignKey = "FK_MapProp_UXSessProp", Cascade = CascadeAssociationOperation.None)]
        public virtual UXSessionProperty DefaultSessionProperty
        {
            get
            {
                return uxSessionProperty;
            }
            set
            {
                uxSessionProperty = value;

                if (uxSessionProperty != null)
                    DefaultValue = null;
            }
        }

        private string displayFormat;

        [PropertyStorageHint(Length = 255, IsMandatory = false)]
        public virtual string DisplayFormat
        { 
            get
            {
                if (string.IsNullOrEmpty(displayFormat))
                    return GetTargetDisplayFormat();
                else
                    return displayFormat;
            }
            set
            {
                if (value == GetTargetDisplayFormat())
                    displayFormat = null;
                else
                    displayFormat = value;
            }
        }

        [ReadOnly(true)]
        [PropertyStorageHint(IsMandatory = true)]
        public virtual bool IsSearchable { get; set; }

        public virtual bool CanChangeDisplayFormat()
        {
            return (TargetProperty != null &&
                    TargetProperty.CanChangeDisplayFormat());
        }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsMapped
        {
            get
            {
                return (this.Target != null) || (!string.IsNullOrEmpty(this.DefaultValue)) || (this.DefaultSessionProperty != null);
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
