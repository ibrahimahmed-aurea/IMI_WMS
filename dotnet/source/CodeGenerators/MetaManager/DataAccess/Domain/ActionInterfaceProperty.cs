using System;
using System.Collections.Generic;
using System.Text;
using Cdc.HbmGenerator.Attributes;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "ActionInterfaceProperty")]
    public class ActionInterfaceProperty : IMappableProperty
    {
        public ActionInterfaceProperty()
        {
        }

        public virtual int Id { get; set; }

        [PropertyStorageHint(IsMandatory = true, Length = 50)]
        public virtual string Name { get; set; }

        [PropertyStorageHint(Column = "UXActionId", IsMandatory = true, Lazy = false, ForeignKey = "FK_ActionInterfaceProperty_UXAction")]
        public virtual UXAction UXAction { get; set; }

        [PropertyStorageHint(Length = 255, IsMandatory = false)]
        public virtual string TypeName { get; set; }

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


    }
}
