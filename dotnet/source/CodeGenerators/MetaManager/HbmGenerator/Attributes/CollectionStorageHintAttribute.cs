using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.HbmGenerator.Attributes
{
    public enum CascadeOperation { All, None, SaveUpdate, Delete, AllDeleteOrphan };

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class CollectionStorageHintAttribute : Attribute
    {
        public CollectionStorageHintAttribute()
        {
            Lazy = true;
            Column = null;
            Inverse = true;
            Cascade = CascadeOperation.All;
            Fetch = FetchOperation.NotSet;
        }

        public bool Lazy { get; set; }
        public string Column { get; set; }
        public bool Inverse { get; set; }
        public CascadeOperation Cascade { get; set; }
        public FetchOperation Fetch { get; set; }

        // TODO Inverse kan man analysera fram ?

    }
}
