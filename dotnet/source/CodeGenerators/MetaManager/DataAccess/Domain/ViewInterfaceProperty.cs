using System;
using System.Collections.Generic;
using System.Text;
using Cdc.HbmGenerator.Attributes;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "ViewInterfaceProperty")]
    public class ViewInterfaceProperty : IMappableProperty
    {
        public ViewInterfaceProperty()
        {
            
        }

        public virtual int Id { get; set; }

        [PropertyStorageHint(Length = 50, IsMandatory = true)]
        public virtual string Name { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual ViewInterfacePropertyDirection Direction { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual bool IsSearchable { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual bool IsVisible { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual int Sequence { get; set; }

        [PropertyStorageHint(Length = 50, IsMandatory = true)]
        public virtual string Caption { get; set; }

        [PropertyStorageHint(Column = "ViewId", IsMandatory = true, Lazy = false, ForeignKey = "FK_ViewInterfaceProperty_View")]
        public virtual View View { get; set; }

        [PropertyStorageHint(Length = 255, IsMandatory = false)]
        public virtual string TypeName { get; set; }

        [PropertyStorageHint(Length = 255, IsMandatory = false)]
        public virtual string DefaultValue { get; set; }

        [PropertyStorageHint(Length = 255, IsMandatory = false)]
        public virtual string FieldTypeHint { get; set; }

        [PropertyStorageHint(Length = 255, IsMandatory = false)]
        public virtual string MapToUserSessionField { get; set; }

        private Type type;

        /// <summary>
        /// Used for type conversion
        /// </summary>
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

        [PropertyStorageHint(Column = "MappedPropertyReferenceId", IsMandatory = false, ForeignKey = "FK_ViewInterfaceProperty_MappedProperty")]
        public virtual MappedProperty MappedPropertyReference { get; set; }

        [PropertyStorageHint(Length = 255, IsMandatory = false)]
        public virtual string DisplayFormat { get; set; }
    }
}
