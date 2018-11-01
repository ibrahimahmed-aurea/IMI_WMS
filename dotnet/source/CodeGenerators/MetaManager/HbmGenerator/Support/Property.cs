using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.HbmGenerator.Attributes;

namespace Cdc.HbmGenerator.Support
{
    public class Property
    {
        public Property()
        {
            StorageHint = new PropertyStorageHintAttribute();
            CollectionHint = new CollectionStorageHintAttribute();
        }

        // public virtual int Id { get; set; } not needed right now
        public virtual Class ParentClass { get; set; }
        public virtual string Name { get; set; }
        public virtual string TypeName { get; set; }
        //public virtual int Length { get; set; }
        //public virtual bool IsMandatory { get; set; }
        public virtual bool IsCollection { get; set; }
        public virtual bool IsReference { get; set; }

        public PropertyStorageHintAttribute StorageHint;
        public CollectionStorageHintAttribute CollectionHint;
        //public virtual Class ReferencedClass { get; set; } not needed right now

    }
}
