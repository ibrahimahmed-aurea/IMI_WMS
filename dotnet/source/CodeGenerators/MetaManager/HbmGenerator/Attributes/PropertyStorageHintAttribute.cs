using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.HbmGenerator.Attributes
{
    public enum CascadeAssociationOperation { Default, All, None, SaveUpdate, Delete };

    public enum FetchOperation { NotSet, Select, Join };

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PropertyStorageHintAttribute : Attribute
    {
        public PropertyStorageHintAttribute()
        {
            Lazy = true;
            Column = null;
            IsMandatory = true;
            Length = 255;
            Ignore = false;
            UniqueKey = string.Empty;
            Type = string.Empty;
            SqlType = string.Empty;
            Fetch = FetchOperation.NotSet;
            Cascade = CascadeAssociationOperation.Default;
        }

        public bool Lazy { get; set; }
        public string Column { get; set; }
        public string ForeignKey { get; set; }
        public bool IsMandatory { get; set; }
        public int Length { get; set; }
        public bool Ignore { get; set; }
        public string UniqueKey { get; set; }
        public string Type { get; set; }
        public string SqlType { get; set; }
        public FetchOperation Fetch { get; set; }
        public CascadeAssociationOperation Cascade { get; set; }
    }
}
