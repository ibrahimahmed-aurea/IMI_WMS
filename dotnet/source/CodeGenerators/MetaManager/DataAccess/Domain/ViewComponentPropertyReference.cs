using System;
using System.Collections.Generic;
using System.Text;
using Cdc.HbmGenerator.Attributes;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "ViewComponentPropRef")]
    public class ViewComponentPropertyReference
    {
        public virtual int Id { get; set; }

        [PropertyStorageHint(Column = "PropertyId", IsMandatory = true, Lazy = false, ForeignKey = "FK_ViewCompPropRef_Prop")]
        public virtual Property Property { get; set; }

        [PropertyStorageHint(Column = "ViewComponentId", IsMandatory = true, Lazy = false, ForeignKey = "FK_ViewCompPropRef_Comp")]
        public virtual ViewComponent ViewComponent { get; set; }
    }
}
